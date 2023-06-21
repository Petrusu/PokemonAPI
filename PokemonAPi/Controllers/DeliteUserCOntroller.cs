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
    public IActionResult currentDeliteUser(string username)
    {
        // Проверяем, существует ли пользователь с заданным идентификатором
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return NotFound("User not found"); // Возвращаем 404 Not Found, если пользователь не найден
        }

        _context.Users.Remove(user); // Удаляем пользователя из контекста базы данных
        _context.SaveChanges(); // Сохраняем изменения в базе данных

        return Ok("User delited"); // Возвращаем 200 OK, чтобы показать успешное удаление пользователя
    }
}