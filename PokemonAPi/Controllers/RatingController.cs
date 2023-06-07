using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
[ApiController]
[Authorize]
public class RatingController : ControllerBase
{
    private readonly PokemonsContext _context;

    public RatingController(PokemonsContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/rating")]
    public IActionResult PostRating([FromForm] int id_pokemon, [FromForm] int? rating)
    {
        //существует ли покемон с таким id
        var pokemon = _context.Pokemons.FirstOrDefault(p => p.IdPokemon == id_pokemon);
        if (pokemon == null)
        {
            return Unauthorized(); // покемон не найден
        }

        // Получите идентификатор авторизованного пользователя из JWT-токена
        var userId = GetUserIdFromToken();
        
        //исключения для рейтинга

        if (rating < 0)
        {
            return BadRequest("The rating value must be positive!");
        }

        if (rating > 5)
        {
            return BadRequest("The rating value must be less than or equal to 5");
        }

       

        var ratingValue = new Rating
        {
            UserId = userId,
            PokemonId = id_pokemon,
            Rating1 = rating,
            Ratingdate = DateTime.UtcNow
        };

        // Сохраните рейтинг в базу данных
        _context.Ratings.Add(ratingValue);
        _context.SaveChanges();

        return Ok();

    }

    //получение id пользователя из токена
    private int GetUserIdFromToken()
    {
        var token = GetTokenFromAuthorizationHeader(); //получаем токен
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        //полчение срока действия токена
        var now = DateTime.UtcNow;
        if (jwtToken.ValidTo < now)
        {
            // Токен истек, выполните необходимые действия, например, вызовите исключение
            throw new Exception("Expired token.");
        }
        // Извлечение идентификатора пользователя из полезной нагрузки токена
        var userId = int.Parse(jwtToken.Claims.First(c => c.Type == "userId").Value);

        return userId;
    }

    //получение токена из запроса
    private string GetTokenFromAuthorizationHeader()
    {
        var autorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

        if (autorizationHeader != null && autorizationHeader.StartsWith("Bearer "))
        {
            var token = autorizationHeader.Substring("Bearer ".Length).Trim();
            return token;
        }

        return null;
    }

}