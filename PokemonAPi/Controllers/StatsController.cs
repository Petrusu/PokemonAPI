/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;

[ApiController]
[Authorize]

public class StatsController : ControllerBase
{
    private IEnumerable<Stat> GetStats() //подключение к базе данных
    {
        using (var context = new PokemonsContext())
        {
            return context.Stats.ToList();
        }
    }
    [HttpGet]
    [Route("api/stats")]
    public ActionResult GetDataApi()
    {
        var statsData = GetStats(); //вывод данных в api
        return Ok(statsData);
    }
}*/