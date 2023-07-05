using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly WhrgmhraContext _context;
    private readonly IConfiguration _configuration;
    private int TokenTimeoutMinutes = 5; // Время истечения срока действия токена в минутах
    private DateTime _tokenCreationTime;
    private string _pokemonOfTheDay;
    private DateTime _currentDay;
    
    public UserController(WhrgmhraContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    //вывод рейтинга покемонов
    private IEnumerable<Rating> GetRating() //подключение к базе данных
    {
        using (var context = new WhrgmhraContext())
        {
            return context.Ratings.ToList();
        }
    }

    [HttpGet ("GetRating")]
    [Authorize]
    public ActionResult GetDataApi()
    {
        var Data = GetRating(); //вывод данных в api
        return Ok(Data);
    }

    [HttpPost("login")]
    public IActionResult Authenticate(string username, string password)
    {
        // Проверяем, существует ли пользователь
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return Unauthorized(); // Пользователь не найден
        }

        var loginResponse = new LoginResponse();

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

        // Если пароль действителен
        if (isPasswordValid)
        {
            string token = CreateToken(user.UserId);

            loginResponse.Token = token;
            loginResponse.ResponseMsg = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            // Возвращаем токен
            return Ok(new { loginResponse });
        }
        else
        {
            // Если имя пользователя или пароль недействительны, отправляем статус-код "BadRequest" в ответе
            return BadRequest("Username or Password Invalid!");
        }
    }
    //покемон дня
     [HttpGet("pokemonofday")]
     [Authorize]

     public async Task<ActionResult> GetPokemonOfTheDay()
     {
         using (FileStream fileStream = new FileStream("pokemonofday/pokemonofday.txt", FileMode.Open))
         {
             DateTime currentDate = DateTime.Now.Date;
    
             string date = currentDate.ToString("dd/MM/yyyy");
             byte[] bytes = new Byte[fileStream.Length];
             await fileStream.ReadAsync(bytes, 0, bytes.Length);
    
             string[] s = new string[2];
             string line = Encoding.Default.GetString(bytes);
             s = line.Split(' ');
    
             if (date == s[0])
             {
                 fileStream.Close();
                 return Ok(s[1]);
             }
             else
             {
                 fileStream.Close();
                 FileStream newFileStream = new FileStream("pokemonofday/pokemonofday.txt", FileMode.Truncate);
                 _pokemonOfTheDay = GetRandomPokemon();
    
                 byte[] addnewpokemon = Encoding.Default.GetBytes(currentDate.ToString("dd/MM/yyyy") + " " + _pokemonOfTheDay);
                 await newFileStream.WriteAsync(addnewpokemon, 0, addnewpokemon.Length);
                 newFileStream.Close();
                 return Ok(s[1]);
             }
         }
     }
     //покемон недели
     [HttpGet("pokemonofweek")]
     [Authorize]
     public IActionResult GetPokemonOfWeek()
     {
         DateTime currentDate = DateTime.Now.ToUniversalTime();
         DateTime weekAgoDate = currentDate.AddDays(-7);

         var pokemonRatings = _context.Ratings
             .Include(r => r.Pokemon)
             .Where(r => r.Ratingdate >= weekAgoDate && r.Ratingdate <= currentDate)
             .GroupBy(r => r.PokemonId)
             .Select(g => new
             {
                 PokemonId = g.Key,
                 TotalRating = g.Sum(r => r.Rating1)
             })
             .OrderByDescending(g => g.TotalRating)
             .FirstOrDefault();

         if (pokemonRatings == null)
         {
             return NotFound("No pokemons found");
         }

         var pokemon = _context.Pokemons.FirstOrDefault(p => p.IdPokemon == pokemonRatings.PokemonId);

         if (pokemon == null)
         {
             return NotFound("Pokemon not found");
         }

         return Ok(pokemon.NamePokemon);
     }
     //покемон месяца
     [HttpGet ("pokemonofmonth")]
     [Authorize]
     public IActionResult GetPokemonOfMonth()
        {
            DateTime currentDate = DateTime.Now.ToUniversalTime();
            DateTime monthAgoDate = currentDate.AddMonths(-1);

            var pokemonRatings = _context.Ratings
                .Include(r => r.Pokemon)
                .Where(r => r.Ratingdate >= monthAgoDate && r.Ratingdate <= currentDate)
                .GroupBy(r => r.PokemonId)
                .Select(g => new
                {
                    PokemonId = g.Key,
                    TotalRating = g.Sum(r => r.Rating1)
                })
                .OrderByDescending(g => g.TotalRating)
                .FirstOrDefault();

            if (pokemonRatings == null)
            {
                return NotFound("No pokemons found");
            }

            var pokemon = _context.Pokemons.FirstOrDefault(p => p.IdPokemon == pokemonRatings.PokemonId);

            if (pokemon == null)
            {
                return NotFound("Pokemon not found");
            }

            return Ok(pokemon.NamePokemon);
        }
        //выставление рейтинга покемона
        [HttpPost ("rating")]
        [Authorize]
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
        
        private string CreateToken(int userId)
        {
            var claims = new List<Claim>()
            {
                // Список претензий (claims) - мы проверяем только id пользователя, можно добавить больше претензий.
                new Claim("userId", Convert.ToString(userId)),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(TokenTimeoutMinutes),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

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
        private string GetRandomPokemon()
        {
            using (var dbContext = new WhrgmhraContext())
            {
                var random = new Random();
                var pokemonCount = dbContext.Pokemons.Count();
                var randomIndex = random.Next(0, pokemonCount);
                var randomPokemon = dbContext.Pokemons.Skip(randomIndex).FirstOrDefault()?.NamePokemon;
                return randomPokemon;
            }
        }
}