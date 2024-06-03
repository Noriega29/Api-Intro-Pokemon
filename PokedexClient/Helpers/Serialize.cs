using Newtonsoft.Json;
using PokedexClient.Models;
using System.Text;

namespace PokedexClient.Helpers
{
    public class Serialize
    {
        public static StringContent SerializePokemon(Pokemon pokemon)
        {
            var json = JsonConvert.SerializeObject(pokemon);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }
}
