using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;

[ApiController]
[Authorize]

public class TypeController : ControllerBase
{
    private IEnumerable<TypePokemon> GetTypePokemon() //подключение к базе данных
    {
        using (var context = new PokemonsContext())
        {
            return context.TypePokemons.ToList();
        }
    }
    [HttpGet]
    [Route("api/type")]
    public ActionResult GetDataApi()
    {
        var typeData = GetTypePokemon(); //вывод данных в api
        return Ok(typeData);
    }
}