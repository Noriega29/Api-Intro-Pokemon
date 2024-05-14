using ASP.NET_API_Intro.Models.ViewModels;
using ASP.NET_API_Intro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_API_Intro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposController : ControllerBase
    {
        private readonly TipoViewModelServices _tiposServices;

        public TiposController(TipoViewModelServices tiposServices)
        {
            _tiposServices = tiposServices;
        }

        // GET: api/tipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoViewModel>>> GetAllTipos()
        {
            return await _tiposServices.GetAllTipos();
        }

        // GET: api/tipos/id
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoViewModel>> GetTipoById(int id)
        {
            var tipo = await _tiposServices.GetTipoById(id);

            if (tipo == null)
            {
                return NotFoundTipo(id);
            }

            return tipo;
        }

        // PUT: api/tipos/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipo(int id, TipoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequestIncongruent(id, viewModel.Id);
            }

            if (await _tiposServices.GetTipoById(id) == null)
            {
                return NotFoundTipo(id);
            }

            if (await _tiposServices.GetTipoByName(viewModel.Nombre) != null)
            {
                return BadRequestExisting(viewModel.Nombre);
            }

            await _tiposServices.UpdateTipo(id, viewModel);

            return NoContent();
        }

        // POST: api/tipos
        [HttpPost]
        public async Task<ActionResult<TipoViewModel>> CreateTipo(TipoViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            if (await _tiposServices.GetTipoByName(viewModel.Nombre) != null)
            {
                return BadRequestExisting(viewModel.Nombre);
            }

            await _tiposServices.CrearTipo(viewModel);

            return CreatedAtAction("GetTipoById", new { id = viewModel.Id }, viewModel);
        }

        // DELETE: api/tipos/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            var tipo = await _tiposServices.GetTipoById(id);

            if (tipo == null)
            {
                return NotFoundTipo(id);
            }

            await _tiposServices.DeleteTipo(id);

            return NoContent();
        }

        public BadRequestObjectResult BadRequestIncongruent(int id1, int id2)
        {
            return BadRequest(new { message = $"El ID '{id1}' de la Url no coincide con el ID '{id2}' del cuerpo de la solicitud." });
        }

        public BadRequestObjectResult BadRequestExisting(string tipo)
        {
            return BadRequest(new { message = $"El tipo con el nombre '{tipo}' ya existe." });
        }
        public NotFoundObjectResult NotFoundTipo(int id)
        {
            return NotFound(new { message = $"El tipo con el ID '{id}' no existe." });
        }
    }
}
