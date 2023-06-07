using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Characteristic
{
    public int IdCharacteristics { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
