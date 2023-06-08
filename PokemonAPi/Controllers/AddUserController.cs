using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
[ApiController]
[Authorize]
public class AddUserController : ControllerBase
{
    private readonly PokemonsContext _context;

    public AddUserController(PokemonsContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/adduser")]
    public void AddUser(ConstUser user)
    {
        User userModel = new User();

        userModel.Username = user.Username;
        userModel.Password = user.Password;
        userModel.Role = 2;
    }
}