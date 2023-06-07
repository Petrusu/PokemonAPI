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
    public class PokemonOfWeekController : ControllerBase
    {
        private readonly PokemonsContext _context;

        public PokemonOfWeekController(PokemonsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/pokemonofweek")]
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
    }
}