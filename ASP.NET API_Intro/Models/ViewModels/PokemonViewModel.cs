using System.ComponentModel.DataAnnotations;

namespace ASP.NET_API_Intro.Models.ViewModels
{
    public class PokemonViewModel
    {
        public int IdPokemon { get; set; }
        public int Numero { get; set; }
        public string Name { get; set; }
        public Dictionary<int, string> Tipos { get; set; }
        public string Description { get; set; }
    }
}
