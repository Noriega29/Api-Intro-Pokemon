using Microsoft.AspNetCore.Mvc;

namespace PokedexClient.Controllers
{
    public class PokedexController : Controller
    {
        private readonly HttpClient _httpClient;

        public PokedexController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7249/api");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
