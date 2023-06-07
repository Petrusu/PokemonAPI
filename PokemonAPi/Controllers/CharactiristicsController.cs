using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;

[ApiController]
[Authorize]

public class CharactiristicsController : ControllerBase
{
    private IEnumerable<Characteristic> GetCharactiristics() //подключние к бызе данных
    {
        using (var context = new PokemonsContext())
        {
            return context.Characteristics.ToList();
        }
    }
    [HttpGet]
    [Route("api/charactiristics")]
    public ActionResult GetDataApi()
    {
        var charactiristicsData = GetCharactiristics(); //вывод данных в api
        return Ok(charactiristicsData);
    }
}