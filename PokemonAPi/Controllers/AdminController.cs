using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers;
[Route("api/[controller]")]
[Authorize(Policy = "UserIdPolicy")]
public class AdminController : ControllerBase
{
    private readonly WhrgmhraContext _context;
    
    public AdminController(WhrgmhraContext context)
    {
        _context = context;
    }
    
    // все о покемонах 
    
    
    // Вывод покемонов с пагинацией
    private IEnumerable<Pokemon> GetPokemon(int page, int pageSize)
    {
        using (var context = new WhrgmhraContext())
        {
            return context.Pokemons
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }

    [HttpGet("api/pokemons")]
    public ActionResult GetPokemonGetDataApi(int page, int pageSize)
    {
        var pokemonsData = GetPokemon(page, pageSize);
        return Ok(pokemonsData);
    }
    

    [HttpPost("addpokemon")]
    public IActionResult AddPokemon(string name, int? IdGrowth, int? Gender, int? Gen, 
        int? Eggcycle, int? Chancetocache, int? Exp, int? Health, int? Attack, int? Protection, int? Speed,
        decimal? height, decimal? weight, string? image, string[] abilityNames, string[] typeName)
    {
        Stat stat = new Stat();
        stat.Eggcycle = Eggcycle;
        stat.Chancetocache = Chancetocache;
        stat.Exp = Exp;
        stat.Health = Health;
        stat.Attack = Attack;
        stat.Protection = Protection;
        stat.Speed = Speed;
        _context.Stats.Add(stat);
        _context.SaveChanges();

        Characteristic characteristic = new Characteristic();
        characteristic.Height = height;
        characteristic.Weight = weight;
        _context.Characteristics.Add(characteristic);
        _context.SaveChanges();

        Pokemon pokemonModel = new Pokemon();
        pokemonModel.NamePokemon = name;
        pokemonModel.IdStats = stat.IdStats;
        pokemonModel.IdCharacteristics = characteristic.IdCharacteristics;
        pokemonModel.IdGrowth = IdGrowth;
        pokemonModel.Gender = Gender;
        pokemonModel.Gen = Gen;
        pokemonModel.Image = image;

        _context.Pokemons.Add(pokemonModel);
        _context.SaveChanges();

        string[] abilityArray = abilityNames;
        foreach (var abilityName in abilityArray)
        {
            Ability ability = _context.Abilities.FirstOrDefault(a => a.Ability1 == abilityName);
            if (ability != null)
            {
                Abilitypokemon abilitypokemon = new Abilitypokemon();
                abilitypokemon.IdAbility = ability.IdAbility;
                abilitypokemon.IdPokemon = pokemonModel.IdPokemon;
                _context.Abilitypokemons.Add(abilitypokemon);
            }
        }

        string[] typeArray = typeName;
        foreach (var type in typeArray)
        {
            TypePokemon types = _context.TypePokemons.FirstOrDefault(t => t.Type == type);
            if (types != null)
            {
                Pokemontype pokemontype = new Pokemontype();
                pokemontype.IdPokemontype = pokemonModel.IdPokemon;
                pokemontype.IdType = types.IdType;
                _context.Pokemontypes.Add(pokemontype);
            }
        }

        _context.SaveChanges();

        return Ok("Pokemon added.");
    }

    
    // Редактирование конткретного поля у покемона
    [HttpPut("changepokemon")]
    public IActionResult ChangePokemon(int id_pokemon, string? name,
        int? Eggcycle, int? Chancetocache, int? Exp, int? Health, int? Attack, int? Protection, int? Speed,
        decimal? height, decimal? weight, string? image)
    {
        var pokemon = _context.Pokemons
            .Include(p => p.IdStatsNavigation)
            .Include(p => p.IdCharacteristicsNavigation)
            .FirstOrDefault(p => p.IdPokemon == id_pokemon);

        if (pokemon == null)
        {
            return NotFound();
        }

        // Обновление выбранных полей
        if (!string.IsNullOrEmpty(name))
        {
            pokemon.NamePokemon = name;
        }

        if (pokemon.IdStatsNavigation != null)
        {
            pokemon.IdStatsNavigation.Eggcycle = Eggcycle;
            pokemon.IdStatsNavigation.Chancetocache = Chancetocache;
            pokemon.IdStatsNavigation.Exp = Exp;
            pokemon.IdStatsNavigation.Health = Health;
            pokemon.IdStatsNavigation.Attack = Attack;
            pokemon.IdStatsNavigation.Protection = Protection;
            pokemon.IdStatsNavigation.Speed = Speed;
        }

        if (pokemon.IdCharacteristicsNavigation != null)
        {
            pokemon.IdCharacteristicsNavigation.Height = height;
            pokemon.IdCharacteristicsNavigation.Weight = weight;
        }

        if (!string.IsNullOrEmpty(image))
        {
            pokemon.Image = image;
        }

        _context.SaveChanges();

        return Ok();
    }
    private IEnumerable<Characteristic> GetCharactiristics() //подключние к бызе данных
    {
        using (var context = new WhrgmhraContext())
        {
            return context.Characteristics.ToList();
        }
    }
    //вывод характиристик покемона
    [HttpGet ("api/charactiristics")]
    public ActionResult GetCharactiristicsDataApi()
    {
        var charactiristicsData = GetCharactiristics(); //вывод данных в api
        return Ok(charactiristicsData);
    }
    //создание списка пользователей
    private List<User> GetUsers() //подключение к базе данных
    {
        using (var context = new WhrgmhraContext())
        {
            return context.Users.ToList();
        }
    }
    //вывод параментров покемона
    private IEnumerable<Stat> GetStats() //подключение к базе данных
    {
        using (var context = new WhrgmhraContext())
        {
            return context.Stats.ToList();
        }
    }
    [HttpGet ("api/stats")]
    public ActionResult GetStatsGetDataApi()
    {
        var statsData = GetStats(); //вывод данных в api
        return Ok(statsData);
    }
    
    //вывод типов покемона
    private IEnumerable<TypePokemon> GetTypePokemon() //подключение к базе данных
    {
        using (var context = new WhrgmhraContext())
        {
            return context.TypePokemons.ToList();
        }
    }
    [HttpGet ("api/type")]
    public ActionResult GetTypePokemonGetDataApi()
    {
        var typeData = GetTypePokemon(); //вывод данных в api
        return Ok(typeData);
    }
    //удаление пользователя
    [HttpDelete("delitepokemon")]
    public async Task<IActionResult> DeletePokemon(int id_pokemon)
    {
        var pokemon = await _context.Pokemons.FindAsync(id_pokemon); //находим пользователя по id

        if (pokemon == null)
        {
            return NotFound("User not found"); // Если покемон с указанным id не найден, возвращаем 404 Not Found
        }

        _context.Pokemons.Remove(pokemon); //удаляем 
        await _context.SaveChangesAsync(); //сохраняем

        return Ok("User delited"); 
    }
    
    //все о пользователях
    
    //вывод всех пользователей
    [HttpGet("getusers")]
    public ActionResult GetDataApi()
    {
        var usersData = GetUsers(); //вывод данных в api
        return Ok(usersData);
    }
    //запрос на изменения пароля
    [HttpPut("changepassword")]
    public IActionResult ChangePassword(int id_user, string password)
    {
        
        // Проверяем, существует ли пользователь
        var user = _context.Users.FirstOrDefault(u => u.UserId == id_user);
        if (user == null)
        {
            return Unauthorized(); // Пользователь не найден
        }
        
        // Шифрование пароля
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        user.Password = hashedPassword;
        _context.SaveChanges(); //сохранения нового пароля
        return Ok("Password changed");
    }
    

    [HttpPost ("adduser")]
    public async Task<IActionResult> AddUser(string Username, string Password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Проверяем, существует ли пользователь с таким же именем пользователя 
        if (await _context.Users.AnyAsync(u => u.Username == Username ))
        {
            return Conflict("A user with the same username or email address already exists");
        }
            
        // Шифрование пароля
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
        // Создаем нового пользователя
        var user = new User
        {
            Username = Username,
            Password = hashedPassword,
            Role = "2"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User successfully registered");
        
    }
    //удаление пользователя
    [HttpDelete("deliteuser")]
    public async Task<IActionResult> DeleteUser(int id_user)
    {
        var user = await _context.Users.FindAsync(id_user); //находим пользователя по id

        if (user == null)
        {
            return NotFound("User not found"); // Если пользователь с указанным id не найден, возвращаем 404 Not Found
        }

        _context.Users.Remove(user); //удаляем пользователя
        await _context.SaveChangesAsync(); //сохраняем

        return Ok("User delited"); 
    }
}