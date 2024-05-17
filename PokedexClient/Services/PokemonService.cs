using Newtonsoft.Json;
using System.Text;
using PokedexClient.Models;
using PokedexClient.Models.ViewModels;

namespace PokedexClient.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(IHttpClientFactory httpClientFactoy)
        {
            _httpClient = httpClientFactoy.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44326/api/");
        }

        // GET: pokemons/
        public async Task<HttpResponseMessage> GetPokemonsList()
        {
            var response = await _httpClient.GetAsync("pokemons");
            return response;
        }

        // GET: pokemons/id
        public async Task<HttpResponseMessage> GetPokemonById(int id)
        {
            var response = await _httpClient.GetAsync($"pokemons/{id}");
            return response;
        }

        // POST: pokemon/
        public async Task<HttpResponseMessage> CreatePokemon(PokemonViewModel viewModel)
        {
            Pokemon pokemon = new Pokemon();

            List<int> tiposID = new List<int> { viewModel.PrimerTipo };
            if (viewModel.SegundoTipo != 0)
            {
                tiposID.Add(viewModel.SegundoTipo);
            }

            pokemon.Numero = viewModel.Numero;
            pokemon.Name = viewModel.Name;
            pokemon.Tipos = new Dictionary<int, string>();
            foreach (var par in tiposID)
            {
                pokemon.Tipos.Add(par, "");
            }
            pokemon.Description = viewModel.Description;

            var json = JsonConvert.SerializeObject(pokemon);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("pokemons/", content);

            return response;
        }

        // POST: pokemon/id
        public async Task<HttpResponseMessage> EditPokemon(int id, PokemonViewModel viewModel)
        {
            Pokemon pokemon = new Pokemon();

            List<int> tiposID = new List<int> { viewModel.PrimerTipo };
            if (viewModel.SegundoTipo != 0)
            {
                tiposID.Add(viewModel.SegundoTipo);
            }

            pokemon.IdPokemon = viewModel.IdPokemon;
            pokemon.Numero = viewModel.Numero;
            pokemon.Name = viewModel.Name;
            pokemon.Tipos = new Dictionary<int, string>();
            foreach (var par in tiposID)
            {
                pokemon.Tipos.Add(par, "");
            }
            pokemon.Description = viewModel.Description;

            var json = JsonConvert.SerializeObject(pokemon);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"pokemons/{id}", content);

            return response;
        }
        
        // POST: pokemon/id
        public async Task DeletePokemon(int id)
        {
            await _httpClient.DeleteAsync($"pokemons/{id}");
        }

        public async Task<IEnumerable<Pokemon>> DeserializePokemonsList(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var pokemons = JsonConvert.DeserializeObject<IEnumerable<Pokemon>>(content);

            return pokemons;
        }

        public async Task<Pokemon> DeserializePokemonById(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);

            return pokemon;
        }
    }
}
