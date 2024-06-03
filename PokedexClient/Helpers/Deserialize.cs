using Newtonsoft.Json;
using PokedexClient.Models;

namespace PokedexClient.Helpers
{
    public class Deserialize
    {
        public static async Task<Pokemon> DeserializePokemonAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
            return pokemon;
        }

        public static async Task<IEnumerable<Pokemon>> DeserializePokemonsListAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var pokemons = JsonConvert.DeserializeObject<IEnumerable<Pokemon>>(content);
            return pokemons;
        }

        public static async Task<ErrorApiResponses> DeserializeErrorApiResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<ErrorApiResponses>(content);
            return errorMessage;
        }
    }
}
