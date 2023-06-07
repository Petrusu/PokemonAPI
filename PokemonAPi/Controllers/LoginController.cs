using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using PokemonAPi.Context;
using PokemonAPi.Models;


namespace PokemonAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PokemonsContext _context;
        private int TokenTimeoutMinutes = 5; // Время истечения срока действия токена в минутах
        private DateTime _tokenCreationTime;

        public LoginController(IConfiguration configuration, PokemonsContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromForm] string username, [FromForm] string password)
        {
            // Проверяем, существует ли пользователь
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                return Unauthorized(); // Пользователь не найден
            }

            var loginResponse = new LoginResponse();

            bool isUsernamePasswordValid = user.Password == password;

            // Если учетные данные действительны
            if (isUsernamePasswordValid)
            {
                string token = CreateToken(user.UserId);

                loginResponse.Token = token;
                loginResponse.ResponseMsg = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                // Возвращаем токен
                return Ok(new { loginResponse });
            }
            else
            {
                // Если имя пользователя или пароль недействительны, отправляем статус-код "BadRequest" в ответе
                return BadRequest("Username or Password Invalid!");
            }
        }
        
        private string CreateToken(int userId)
        {
            
            var claims = new List<Claim>()
            {
                // Список претензий (claims) - мы проверяем только id пользователя, можно добавить больше претензий.
                new Claim("userId", Convert.ToString(userId)),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(TokenTimeoutMinutes),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}