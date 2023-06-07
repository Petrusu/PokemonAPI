using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
[ApiController]
[Authorize]
public class GetRatingController : ControllerBase
{
    private IEnumerable<Rating> GetRating() //подключение к базе данных
    {
        using (var context = new PokemonsContext())
        {
            return context.Ratings.ToList();
        }
    }
    [HttpGet]
    [Route("api/GetRating")]
    public ActionResult GetDataApi()
    {
        var Data = GetRating(); //вывод данных в api
        return Ok(Data);
    }
}