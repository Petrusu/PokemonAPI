using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Abilitypokemon
{
    public int IdAbilitypokemon { get; set; }

    public int? IdAbility { get; set; }

    public int? IdPokemon { get; set; }

    public virtual Ability? IdAbilityNavigation { get; set; }

    public virtual Pokemon? IdPokemonNavigation { get; set; }
}
