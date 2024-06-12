using PokedexClient.Models;
using PokedexClient.Models.ViewModels;
using PokedexClient.Helpers;
using System.Net.Sockets;

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
            try
            {
                var response = await _httpClient.GetAsync("pokemons");
                return response;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // GET: pokemons/id
        public async Task<HttpResponseMessage> GetPokemonById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"pokemons/{id}");
                return response;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // POST: pokemon/
        public async Task<HttpResponseMessage> CreatePokemon(PokemonViewModel viewModel)
        {
            try
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

                var response = await _httpClient.PostAsync("pokemons/", Serialize.SerializePokemon(pokemon));
                return response;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        // POST: pokemon/id
        public async Task<HttpResponseMessage> EditPokemon(int id, PokemonViewModel viewModel)
        {
            try
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

                var response = await _httpClient.PutAsync($"pokemons/{id}", Serialize.SerializePokemon(pokemon));
                return response;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }
        
        // POST: pokemon/id
        public async Task DeletePokemon(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"pokemons/{id}");
            }
            catch (SocketException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}
