using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonAPi.Context;
using PokemonAPi.Models;
using System;
using System.IO;

namespace PokemonAPi.Controllers
{
    [ApiController]
    [Authorize]
    public class PokemonOfDayController : ControllerBase
    {
        private readonly string _csvFilePath = "pokemon.csv"; // Путь к CSV файлу
        private string _pokemonOfTheDay;
        private DateTime _currentDay;

        [HttpGet]
        [Route("api/pokemonofday")]
        public ActionResult<string> GetPokemonOfTheDay()
        {
            var currentDate = DateTime.Now.Date;

            if (_currentDay == null || currentDate > _currentDay)
            {
                _currentDay = currentDate;
                _pokemonOfTheDay = GetPokemonFromCsv();
            }

            if (string.IsNullOrEmpty(_pokemonOfTheDay))
            {
                _pokemonOfTheDay = GetRandomPokemon();
                WritePokemonToCsv(_pokemonOfTheDay);
            }

            return _pokemonOfTheDay;
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

        private string GetPokemonFromCsv()
        {
            if (!System.IO.File.Exists(_csvFilePath))
            {
                return null;
            }

            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
            }))
            {
                if (csv.Read())
                {
                    return csv.GetField(0);
                }
            }

            return null;
        }

        private void WritePokemonToCsv(string pokemon)
        {
            using (var writer = new StreamWriter(_csvFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = false
            }))
            {
                csv.WriteField(pokemon);
                csv.NextRecord();
            }
        }
    }
}
