using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;
namespace PokemonAPi.Controllers;

[ApiController]
[Authorize]

public class EvalutionPokemon : ControllerBase
{
    private IEnumerable<Pokemonegggroup> GetEvalution() //подключение к базе данных
    {
        using (var context = new PokemonsContext())
        {
            return context.Pokemonegggroups.ToList();
        }
    }
    [HttpGet]
    [Route("api/evalution")]
    public ActionResult GetDataApi()
    {
        var evalutionData = GetEvalution(); //вывод данных в api
        return Ok(evalutionData);
    }   
}