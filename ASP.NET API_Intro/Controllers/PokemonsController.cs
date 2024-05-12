using Microsoft.AspNetCore.Mvc;
using ASP.NET_API_Intro.Models.ViewModels;
using ASP.NET_API_Intro.Services;

namespace ASP.NET_API_Intro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly PokemonViewModelService _service;

        public PokemonsController(PokemonViewModelService service)
        {
            _service = service;
        }

        // GET: api/pokemons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonViewModel>>> GetPokemons()
        {
            return await _service.GetAllPokemons();
        }

        // GET: api/pokemon/id
        [HttpGet("{id}")]
        public async Task<ActionResult<PokemonViewModel>> GetPokemon(int id)
        {
            var pokemon = await _service.GetPokemonById(id);

            if (pokemon == null)
            {
                return NotFound();
            }

            return Ok(pokemon);
        }

        // PUT: api/pokemons/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPokemon(int id, PokemonViewModel viewModel)
        {
            if (id != viewModel.IdPokemon || viewModel.Tipos.Count <= 0 || viewModel.Tipos.Count >= 3)
            {
                return BadRequest();
            }

            if (await _service.GetPokemonById(id) == null)
            {
                return NotFound();
            }

            await _service.UpdatePokemon(id, viewModel);
            return NoContent();
        }

        // POST: api/Pokemons
        [HttpPost]
        public async Task<ActionResult<PokemonViewModel>> PostPokemon(PokemonViewModel viewModel)
        {
            if (viewModel.Tipos.Count <= 0 || viewModel.Tipos.Count >= 3)
            {
                return BadRequest();
            }

            await _service.CreatePokemon(viewModel);

            return CreatedAtAction("GetPokemon", new { id = viewModel.IdPokemon }, viewModel);
        }

        // DELETE: api/Pokemons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            var pokemon = await _service.GetPokemonById(id);

            if (pokemon == null)
            {
                return NotFound();
            }

            await _service.DeletePokemon(id);
            return NoContent();
        }
    }
}
