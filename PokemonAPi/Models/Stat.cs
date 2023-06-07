using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Stat
{
    public int IdStats { get; set; }

    public int? Eggcycle { get; set; }

    public int? Chancetocache { get; set; }

    public int? Exp { get; set; }

    public int? Health { get; set; }

    public int? Attack { get; set; }

    public int? Protection { get; set; }

    public int? Speed { get; set; }

    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
