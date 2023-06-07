using System;
using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonAPi.Context;
using PokemonAPi.Models;

namespace PokemonAPi.Controllers
{
    [ApiController]
    [Authorize]
    public class PokemonOfDayController : ControllerBase
    {
        private readonly PokemonsContext _context;
        private const string CsvFilePath = "pokemon.csv";
        private readonly IConfiguration _configuration;
        private readonly Random _random;

        public PokemonOfDayController(PokemonsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _random = new Random();
        }

        [HttpGet]
        [Route("api/pokemonofday")]
        public IActionResult GetPokemonOfDay()
        {
            var csvPath = _configuration.GetValue<string>("CsvPath");
            var pokemons = ReadPokemonsFromCsv(csvPath);
            var pokemonOfDay = GetPokemonOfDayFromCsv(csvPath);

            if (pokemonOfDay != null)
            {
                return Ok(pokemonOfDay);
            }

            pokemonOfDay = GetRandomPokemon(pokemons);

            WritePokemonOfDayToCsv(csvPath, pokemonOfDay);

            return Ok(pokemonOfDay);
        }
        private List<Pokemon> ReadPokemonsFromCsv(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Pokemon>().ToList();
            }
        }
        private Pokemon GetPokemonOfDayFromCsv(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Pokemon>().SingleOrDefault();
            }
        }

        private Pokemon GetRandomPokemon(List<Pokemon> pokemons)
        {
            var index = _random.Next(0, pokemons.Count);
            return pokemons[index];
        }

        private void WritePokemonOfDayToCsv(string csvPath, Pokemon pokemon)
        {
            using (var writer = new StreamWriter(csvPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(pokemon);
            }
        }

    }
}
