using ASP.NET_API_Intro.Helpers;
using ASP.NET_API_Intro.Models.ViewModels;
using ASP.NET_API_Intro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Net.Sockets;

namespace ASP.NET_API_Intro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposController : ControllerBase
    {
        private readonly TipoViewModelServices _services;
        private readonly FindEntities _find;

        public TiposController(TipoViewModelServices services, FindEntities find)
        {
            _services = services;
            _find = find;
        }
        
        // GET: api/tipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoViewModel>>> GetAllTipos()
        {
            try
            {
                return await _services.GetAllTipos();
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

        // GET: api/tipos/id
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoViewModel>> GetTipoById(int id)
        {
            try
            {
                var tipo = await _services.GetTipoById(id);

                if (tipo == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ApiResponses.Status404NotFoundEntity("tipo", id));
                }
                return tipo;
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

        // PUT: api/tipos/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipo(int id, TipoViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    return BadRequest(ApiResponses.Status400IncongruentId(id, viewModel.Id));
                }

                if (!await _find.FindTipoByAnyParameter(id: id))
                {
                    return NotFound(ApiResponses.Status404NotFoundEntity("tipo", id));
                }

                if (await _find.FindTipoByAnyParameter(nombre: viewModel.Nombre))
                {
                    return BadRequest(ApiResponses.Status400ExistingEntityByName("tipo", viewModel.Nombre));
                }

                await _services.UpdateTipo(id, viewModel);
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
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status409Conflict, ApiResponses.Status409DbUpdateConcurrencyException());
            }
        }

        // POST: api/tipos
        [HttpPost]
        public async Task<ActionResult<TipoViewModel>> CreateTipo(TipoViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return BadRequest();
                }

                if (await _find.FindTipoByAnyParameter(nombre: viewModel.Nombre))
                {
                    return BadRequest(ApiResponses.Status400ExistingEntityByName("tipo", viewModel.Nombre));
                }

                await _services.CrearTipo(viewModel);
                return CreatedAtAction("GetTipoById", new { id = viewModel.Id }, viewModel);
                //return StatusCode(StatusCodes.Status201Created, ApiResponses.Status201CreatedEntity("Tipo", viewModel));
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

        // DELETE: api/tipos/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            try
            {
                if (!await _find.FindTipoByAnyParameter(id: id))
                {
                    return NotFound(ApiResponses.Status404NotFoundEntity("tipo", id));
                }

                await _services.DeleteTipo(id);
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
