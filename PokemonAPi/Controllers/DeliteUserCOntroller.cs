using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;

namespace PokemonAPi.Controllers;
[ApiController]
[Authorize(Policy = "UserIdPolicy")]
public class DeliteUserController : ControllerBase
{
    private readonly PokemonsContext _context;

    public DeliteUserController(PokemonsContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/deliteuser")]
    public IActionResult currentDeliteUser()
    {
        
    }
}