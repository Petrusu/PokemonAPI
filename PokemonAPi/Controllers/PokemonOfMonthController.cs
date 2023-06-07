using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonAPi.Context;
using PokemonAPi.Models;
using System;

namespace PokemonAPi.Controllers
{
    [ApiController]
    [Authorize]
    public class PokemonOfMonthController : ControllerBase
    {
        private readonly PokemonsContext _context;

        public PokemonOfMonthController(PokemonsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/pokemonofmonth")]
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
    }
}