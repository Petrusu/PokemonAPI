using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Egggroup
{
    public int IdEgggroup { get; set; }

    public string? Group { get; set; }

    public virtual ICollection<Pokemonegggroup> Pokemonegggroups { get; set; } = new List<Pokemonegggroup>();
}
