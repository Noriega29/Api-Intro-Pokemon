using ASP.NET_API_Intro.Models;
using ASP.NET_API_Intro.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_API_Intro.Services
{
    public class TipoViewModelServices
    {
        private readonly PokedexContext _context;

        public TipoViewModelServices (PokedexContext context)
        {
            _context = context;
        }

        // GET: api/Tipos
        public async Task<ActionResult<IEnumerable<TipoViewModel>>> GetAllTipos()
        {
            var tipos = await _context.Tipos
                .AsNoTracking()
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            var viewModel = tipos.Select(t => new TipoViewModel
            {
                Id = t.IdTipo,
                Nombre = t.Nombre
            }).ToList();

            return viewModel;
        }

        // GET: api/Tipos/id
        public async Task<ActionResult<TipoViewModel>> GetTipoById(int id)
        {
            var tipo = await _context.Tipos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdTipo == id);

            if (tipo == null)
            {
                return null;
            }

            var viewModel = new TipoViewModel()
            {
                Id = tipo.IdTipo,
                Nombre = tipo.Nombre
            };

            return viewModel;
        }

        // GET: api/Tipos/name
        public async Task<ActionResult<TipoViewModel>> GetTipoByName(string name)
        {
            var tipo = await _context.Tipos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Nombre == name);

            if (tipo == null)
            {
                return null;
            }

            var viewModel = new TipoViewModel()
            {
                Id = tipo.IdTipo,
                Nombre = tipo.Nombre
            };

            return viewModel;
        }

        // PUT: api/Tipos/id
        public async Task UpdateTipo(int id, TipoViewModel viewModel)
        {
            var tipo = await _context.Tipos
                .FirstOrDefaultAsync(t => t.IdTipo == id);

            if (tipo != null)
            {
                tipo.Nombre = viewModel.Nombre;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // POST: api/Tipos
        public async Task CrearTipo(TipoViewModel viewModel)
        {
            Tipo tipo = new()
            {
                Nombre = viewModel.Nombre
            };

            await _context.Tipos.AddAsync(tipo);
            await _context.SaveChangesAsync();
        }

        // DELETE: api/Tipos/id
        public async Task DeleteTipo(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);

            _context.Tipos.Remove(tipo);
            await _context.SaveChangesAsync();
        }
    }
}
