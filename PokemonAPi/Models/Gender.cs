using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Gender
{
    public int IdGender { get; set; }

    public string? Gender1 { get; set; }

    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
