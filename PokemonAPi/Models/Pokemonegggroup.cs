using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Pokemonegggroup
{
    public int IdPokemonegg { get; set; }

    public int? IdPokemon { get; set; }

    public int? IdEgggroup { get; set; }

    public virtual Egggroup? IdEgggroupNavigation { get; set; }

    public virtual Pokemon? IdPokemonNavigation { get; set; }
}
