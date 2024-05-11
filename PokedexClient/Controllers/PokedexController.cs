using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PokedexClient.Models;

namespace PokedexClient.Controllers
{
    public class PokedexController : Controller
    {
        private readonly HttpClient _httpClient;

        public PokedexController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44326/api/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("pokemons");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pokemons = JsonConvert.DeserializeObject<IEnumerable<PokemonViewModel>>(content);
                return View(pokemons);
            }

            return View(new List<PokemonViewModel>());
        }
    }
}
