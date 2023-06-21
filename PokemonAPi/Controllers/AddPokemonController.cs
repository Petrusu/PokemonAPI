using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
[ApiController]
[Authorize(Policy = "UserIdPolicy")]
public class AddPokemonController : ControllerBase
{
    private readonly PokemonsContext _context;

    public AddPokemonController(PokemonsContext context)
    {
        _context = context;
    }
    [HttpPost]
    [Route("api/addpokemon")]
    public IActionResult AddUser(ConstPokemon pokemon)
    {
        Pokemon pokemonModel = new Pokemon();

        pokemonModel.NamePokemon = pokemon.NamePokemon;
        pokemonModel.IdStats = pokemon.IdStats;
        pokemonModel.IdCharacteristics = pokemon.IdCharacteristics;
        pokemonModel.IdGrowth = pokemon.IdGrowth;
        pokemonModel.Gender = pokemon.Gender;
        pokemonModel.Gen = pokemon.Gen;
        pokemonModel.Image = pokemon.Image;

        _context.Pokemons.Add(pokemonModel);
        _context.SaveChanges();

        return Ok("Pokemon add.");
    }
}