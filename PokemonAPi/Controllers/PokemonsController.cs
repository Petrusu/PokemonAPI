using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;

[ApiController]
[Authorize]

public class PokemonController : ControllerBase
{

    private IEnumerable<Pokemon> GetPokemon() //подключение к базе данных
    {
        using (var context = new PokemonsContext())
        {
            return context.Pokemons.ToList();
        }
    }
    [HttpGet]
    [Route("api/pokemons")]
    public ActionResult GetDataApi()
    {
        var pokemonsData = GetPokemon(); //вывод данных в api
        return Ok(pokemonsData);
    }

}
    