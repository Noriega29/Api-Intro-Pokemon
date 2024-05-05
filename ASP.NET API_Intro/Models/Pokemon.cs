using System;
using System.Collections.Generic;

namespace ASP.NET_API_Intro.Models;

public partial class Pokemon
{
    public int IdPokemon { get; set; }

    public int N { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<PokemonsTipo> PokemonsTipos { get; set; } = new List<PokemonsTipo>();
}
