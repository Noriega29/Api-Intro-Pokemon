using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokedexClient.Models;
using PokedexClient.Models.ViewModels;
using PokedexClient.Services;

namespace PokedexClient.Controllers
{
    public class PokedexController : Controller
    {
        private readonly PokemonService _pokemonService;
        private readonly TiposService _tiposService;

        public PokedexController(PokemonService pokemonService, TiposService tiposService)
        {
            _pokemonService = pokemonService;
            _tiposService = tiposService;
        }

        // GET: pokemons/
        public async Task<IActionResult> Index()
        {
            var response = await _pokemonService.GetPokemonsList();
            if (response.IsSuccessStatusCode)
            {
                var pokemons = await _pokemonService.DeserializePokemonsList(response);
                return View(pokemons);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: pokemons/id
        public async Task<IActionResult> Details(int id)
        {
            var response = await _pokemonService.GetPokemonById(id);
            if (response.IsSuccessStatusCode)
            {
                var pokemon = await _pokemonService.DeserializePokemonById(response);
                return View(pokemon);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: pokemon/
        public async Task<IActionResult> Create()
        {
            ViewData["Tipos"] = new SelectList(await _tiposService.GetTiposList(), "Id", "Nombre");
            return View();
        }

        // POST: pokemon/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PokemonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _pokemonService.CreatePokemon(viewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha habido un error al intentar cear el pokemon.");
                }
            }
            return View(viewModel);
        }

        // GET: pokemon/id
        public async Task<IActionResult> Edit(int id)
        {
            Pokemon pokemon = new Pokemon();
            var response = await _pokemonService.GetPokemonById(id);
            if (response.IsSuccessStatusCode)
            {
                pokemon = await _pokemonService.DeserializePokemonById(response);
            }
            else
            {
                return NotFound();
            }

            List<int> tiposID = new List<int>();
            foreach (var par in pokemon.Tipos)
            {
                tiposID.Add(par.Key);
            }

            PokemonViewModel viewModel = new()
            {
                IdPokemon = pokemon.IdPokemon,
                Numero = pokemon.Numero,
                Name = pokemon.Name,
                PrimerTipo = tiposID[0],
                SegundoTipo = tiposID.Count > 1 ? tiposID[1] : 0,
                Description = pokemon.Description
            };

            ViewData["Tipos"] = new SelectList(await _tiposService.GetTiposList(), "Id", "Nombre");
            return View(viewModel);
        }

        // POST: pokemon/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PokemonViewModel viewModel)
        {
            if (id != viewModel.IdPokemon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _pokemonService.EditPokemon(id, viewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha habido un error al intentar actualizar el pokemon.");
                }
            }
            return View(viewModel);
        }
        
        // GET: pokemon/id
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _pokemonService.GetPokemonById(id);
            if (response.IsSuccessStatusCode)
            {
                var pokemon = await _pokemonService.DeserializePokemonById(response);
                return View(pokemon);
            }
            else
            {
                return NotFound();
            }
        }
        
        // POST: pokemon/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pokemonService.DeletePokemon(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
