using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Growth
{
    public int IdGrowth { get; set; }

    public string? Growth1 { get; set; }

    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
