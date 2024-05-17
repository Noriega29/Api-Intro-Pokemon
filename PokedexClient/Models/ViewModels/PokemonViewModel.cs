using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PokedexClient.Models.ViewModels
{
    public class PokemonViewModel
    {
        public int IdPokemon { get; set; }

        [Required]
        [DisplayName("Número")]
        public int Numero { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Tipo #1")]
        public int PrimerTipo { get; set; }

        [DisplayName("Tipo #2")]
        public int SegundoTipo { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string Description { get; set; }
    }
}
