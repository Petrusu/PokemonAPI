using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonAPi.Context;
using PokemonAPi.Models;
using System;
using System.IO;
using System.Text;

namespace PokemonAPi.Controllers
{
    [ApiController]
    [Authorize]
    public class PokemonOfDayController : ControllerBase
    {
        
        private string _pokemonOfTheDay;
        private DateTime _currentDay;

        [HttpGet]
        [Route("api/pokemonofday")]
        public async Task<ActionResult> GetPokemonOfTheDay()
        {
            using (FileStream fileStream = new FileStream("pokemonofday/pokemonofday.txt", FileMode.Open))
            {
                DateTime currentDate = DateTime.Now.Date;

                string date = currentDate.ToString("dd/MM/yyyy");
                byte[] bytes = new Byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);

                string[] s = new string[2];
                string line = Encoding.Default.GetString(bytes);
                s = line.Split(' ');

                if (date == s[0])
                {
                    fileStream.Close();
                    return Ok(s[1]);
                }
                else
                {
                    fileStream.Close();
                    FileStream newFileStream = new FileStream("pokemonofday/pokemonofday.txt", FileMode.Truncate);
                    _pokemonOfTheDay = GetRandomPokemon();

                    byte[] addnewpokemon = Encoding.Default.GetBytes(currentDate.ToString("dd/MM/yyyy") + " " + _pokemonOfTheDay);
                    await newFileStream.WriteAsync(addnewpokemon, 0, addnewpokemon.Length);
                    newFileStream.Close();
                    return Ok(s[1]);
                }
            }
        }
        private string GetRandomPokemon()
        {
            using (var dbContext = new PokemonsContext())
            {
                var random = new Random();
                var pokemonCount = dbContext.Pokemons.Count();
                var randomIndex = random.Next(0, pokemonCount);
                var randomPokemon = dbContext.Pokemons.Skip(randomIndex).FirstOrDefault()?.NamePokemon;
                return randomPokemon;
            }
        }
        
    }
}
