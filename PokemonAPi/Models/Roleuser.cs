using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Roleuser
{
    public int IdRole { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
