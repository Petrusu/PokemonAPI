using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Pokemon
{
    public int IdPokemon { get; set; }

    public string? NamePokemon { get; set; }

    public int? IdStats { get; set; }

    public int? IdCharacteristics { get; set; }

    public int? IdGrowth { get; set; }

    public int? Gender { get; set; }

    public int? Gen { get; set; }

    public string? Image { get; set; }

    public DateTime Dateselected { get; set; }

    public bool Ispokemonofday { get; set; }

    public virtual ICollection<Abilitypokemon> Abilitypokemons { get; set; } = new List<Abilitypokemon>();

    public virtual Gender? GenderNavigation { get; set; }

    public virtual Characteristic? IdCharacteristicsNavigation { get; set; }

    public virtual Growth? IdGrowthNavigation { get; set; }

    public virtual Stat? IdStatsNavigation { get; set; }

    public virtual ICollection<Pokemonegggroup> Pokemonegggroups { get; set; } = new List<Pokemonegggroup>();

    public virtual ICollection<Pokemontype> Pokemontypes { get; set; } = new List<Pokemontype>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
