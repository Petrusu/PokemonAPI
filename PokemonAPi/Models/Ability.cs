using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Ability
{
    public int IdAbility { get; set; }

    public string? Ability1 { get; set; }

    public virtual ICollection<Abilitypokemon> Abilitypokemons { get; set; } = new List<Abilitypokemon>();
}
