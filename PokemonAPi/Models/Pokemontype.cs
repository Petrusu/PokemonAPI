using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Pokemontype
{
    public int IdPokemontype { get; set; }

    public int? IdPokemon { get; set; }

    public int? IdType { get; set; }

    public virtual Pokemon? IdPokemonNavigation { get; set; }

    public virtual TypePokemon? IdTypeNavigation { get; set; }
}
