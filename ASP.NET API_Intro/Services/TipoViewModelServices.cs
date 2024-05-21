using ASP.NET_API_Intro.Models;
using ASP.NET_API_Intro.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Net.Sockets;

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
            try
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
            catch (MySqlException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }
        }

        // GET: api/Tipos/id
        public async Task<ActionResult<TipoViewModel>> GetTipoById(int id)
        {
            try
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
            catch (MySqlException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }
        }

        // PUT: api/Tipos/id
        public async Task UpdateTipo(int id, TipoViewModel viewModel)
        {
            try
            {
                var tipo = await _context.Tipos
                .FirstOrDefaultAsync(t => t.IdTipo == id);

                if (tipo != null)
                {
                    tipo.Nombre = viewModel.Nombre;
                }

                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // POST: api/Tipos
        public async Task CrearTipo(TipoViewModel viewModel)
        {
            try
            {
                Tipo tipo = new()
                {
                    Nombre = viewModel.Nombre
                };

                await _context.Tipos.AddAsync(tipo);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }
        }

        // DELETE: api/Tipos/id
        public async Task DeleteTipo(int id)
        {
            try
            {
                var tipo = await _context.Tipos.FindAsync(id);

                _context.Tipos.Remove(tipo);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }
        }
    }
}
