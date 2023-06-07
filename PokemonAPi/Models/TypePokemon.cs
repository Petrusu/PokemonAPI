using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class TypePokemon
{
    public int IdType { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Pokemontype> Pokemontypes { get; set; } = new List<Pokemontype>();
}
