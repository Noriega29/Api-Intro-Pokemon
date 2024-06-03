using Microsoft.AspNetCore.Mvc;
using ASP.NET_API_Intro.Models.ViewModels;
using ASP.NET_API_Intro.Services;
using ASP.NET_API_Intro.Helpers;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_API_Intro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly PokemonViewModelService _service;
        private readonly FindEntities _find;

        public PokemonsController(PokemonViewModelService service, FindEntities find)
        {
            _service = service;
            _find = find;
        }

        // GET: api/pokemons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonViewModel>>> GetPokemons()
        {
            try
            {
                return await _service.GetAllPokemons();
            }
            catch (MySqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500MySqlException());
            }
            catch (SocketException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500SocketException());
            }
        }

        // GET: api/pokemon/id
        [HttpGet("{id}")]
        public async Task<ActionResult<PokemonViewModel>> GetPokemon(int id)
        {
            try
            {
                var pokemon = await _service.GetPokemonById(id);
                if (pokemon == null)
                {
                    return NotFound(ApiResponses.Status404NotFoundEntity("Pokémon", id));
                }
                return Ok(pokemon);
            }
            catch (MySqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500MySqlException());
            }
            catch (SocketException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500SocketException());
            }
        }

        // POST: api/pokemons
        [HttpPost]
        public async Task<ActionResult<PokemonViewModel>> PostPokemon(PokemonViewModel viewModel)
        {
            try
            {
                // Valida que el número del Pokémon sea mayor a cero.
                if (viewModel.Numero < 1)
                {
                    return BadRequest(ApiResponses.Status400NotValidNumber());
                }

                // Comprueba si existe un Pokémon en la base de datos basándose en el número del pokémon.
                if (await _find.FindPokemonByAnyParameter(numero: viewModel.Numero))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ApiResponses.Status409ExistingPokemonNumber(viewModel.Numero));
                }

                // Comprueba si existe un Pokémon en la base de datos basándose en el nombre del pokémon.
                if (await _find.FindPokemonByAnyParameter(nombre: viewModel.Name))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ApiResponses.Status409ExistingEntityByName("Pokémon", viewModel.Name));
                }

                // Comprueba si la cantidad de tipos del pokémon es la correcta.
                if (viewModel.Tipos.Count <= 0 || viewModel.Tipos.Count >= 3)
                {
                    return BadRequest(ApiResponses.Status400InvalidTiposCount());
                }

                await _service.CreatePokemon(viewModel);
                return CreatedAtAction("GetPokemon", new { id = viewModel.IdPokemon }, viewModel);
                //return StatusCode(StatusCodes.Status201Created, ApiResponses.Status201CreatedEntity("Pokémon", viewModel));
            }
            catch (MySqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500MySqlException());
            }
            catch (SocketException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500SocketException());
            }
        }

        // PUT: api/pokemons/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPokemon(int id, PokemonViewModel viewModel)
        {
            try
            {
                // Comprobar que el ID del Uri sea la misma que está en el cuerpo de la solicitud.
                if (id != viewModel.IdPokemon)
                {
                    return BadRequest(ApiResponses.Status400IncongruentId(id, viewModel.IdPokemon));
                }

                // Comprueba por ID si el pokémon existe.
                if (!await _find.FindPokemonByAnyParameter(id: id))
                {
                    return NotFound(ApiResponses.Status404NotFoundEntity("Pokémon", id));
                }

                // Valida que el número del Pokémon sea mayor a cero.
                if (viewModel.Numero < 1)
                {
                    return BadRequest(ApiResponses.Status400NotValidNumber());
                }

                // Verificar si existe un pokémon en la base de datos basándote en el número.
                // y excluyendo los elementos con el ID proporcionado.
                if (await _find.FindPokemonByANumberOrNameExcludingId(id: id, numero: viewModel.Numero))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ApiResponses.Status409ExistingPokemonNumber(viewModel.Numero));
                }

                // Verificar si existe un pokémon en la base de datos basándote en el nombre.
                // y excluyendo los elementos con el ID proporcionado.
                if (await _find.FindPokemonByANumberOrNameExcludingId(id: id, nombre: viewModel.Name))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ApiResponses.Status409ExistingEntityByName("Pokémon", viewModel.Name));
                }

                // Comprobar que la cantidad de tipos del pokemon sea la correcta.
                if (viewModel.Tipos.Count < 1 || viewModel.Tipos.Count > 2)
                {
                    return BadRequest(ApiResponses.Status400InvalidTiposCount());
                }

                await _service.UpdatePokemon(id, viewModel);
                return CreatedAtAction("GetPokemon", new { id = viewModel.IdPokemon }, viewModel);
            }
            catch (MySqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500MySqlException());
            }
            catch (SocketException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500SocketException());
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status409Conflict, ApiResponses.Status409DbUpdateConcurrencyException());
            }
        }

        // DELETE: api/pokemons/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            try
            {
                // Verificar si existe un Pokémon en la base de datos basándote en un ID.
                if (!await _find.FindPokemonByAnyParameter(id: id))
                {
                    return NotFound(ApiResponses.Status404NotFoundEntity("Pokémon", id));
                }

                await _service.DeletePokemon(id);
                return NoContent();
            }
            catch (MySqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500MySqlException());
            }
            catch (SocketException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponses.Status500SocketException());
            }
        }
    }
}
