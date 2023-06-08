using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Role { get; set; }

    public int? WhatPokemonAreYou { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Roleuser? RoleNavigation { get; set; }

    public virtual Pokemon? WhatPokemonAreYouNavigation { get; set; }
}
