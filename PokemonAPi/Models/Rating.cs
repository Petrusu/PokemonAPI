using System;
using System.Collections.Generic;

namespace PokemonAPi.Models;

public partial class Rating
{
    public int IdUserrating { get; set; }

    public int? UserId { get; set; }

    public int? PokemonId { get; set; }

    public int? Rating1 { get; set; }

    public DateTime? Ratingdate { get; set; }

    public virtual Pokemon? Pokemon { get; set; }

    public virtual User? User { get; set; }
}
