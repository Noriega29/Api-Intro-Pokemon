using Newtonsoft.Json;
using PokedexClient.Models;

namespace PokedexClient.Services
{
    public class TiposService
    {
        private readonly HttpClient _httpClient;

        public TiposService(IHttpClientFactory httpClientFactoy)
        {
            _httpClient = httpClientFactoy.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44326/api/");
        }

        public async Task<IEnumerable<Tipo>> GetTiposList()
        {
            var response = await _httpClient.GetAsync("tipos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tipos = JsonConvert.DeserializeObject<IEnumerable<Tipo>>(content);

                return tipos;
            }
            return (new List<Tipo>());
        }
    }
}
