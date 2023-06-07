using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
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
    public async Task<ActionResult<Pokemon>> GetPokemonOfWeek()
    {
        DateTime currenrDate = DateTime.Now;
        DateTime startOfWeek = currenrDate.AddDays(-((int)currenrDate.DayOfWeek - 1));
        DateTime endOfWeek = startOfWeek.AddDays(6);

        var pokmonOfWeek = await _context.Ratings
            .Where(r => r.Ratingdate >= startOfWeek && r.Ratingdate <= endOfWeek)
            .OrderByDescending(r => r.Rating1)
            .Select(r => r.Pokemon)
            .FirstOrDefaultAsync();
        if (pokmonOfWeek == null)
        {
            return NotFound();
        }

        return Ok(pokmonOfWeek);
    }
}