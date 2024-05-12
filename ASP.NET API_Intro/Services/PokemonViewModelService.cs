using ASP.NET_API_Intro.Models;
using ASP.NET_API_Intro.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_API_Intro.Services
{
    public class PokemonViewModelService
    {
        private readonly PokedexContext _context;

        public PokemonViewModelService (PokedexContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<PokemonViewModel>>> GetAllPokemons()
        {
            // Primero obtenemos la lista de Pokemons y luego aplicamos las transformaciones en memoria
            var pokemons = await _context.Pokemons
                .Include(p => p.PokemonsTipos)
                .ThenInclude(pt => pt.IdTipoNavigation)
                .AsNoTracking()
                .OrderBy(p => p.N)
                .ToListAsync();

            var viewModel = pokemons.Select(p => new PokemonViewModel
            {
                IdPokemon = p.IdPokemon,
                Numero = p.N,
                Name = p.Nombre,
                Tipos = p.PokemonsTipos
                    .ToDictionary(pt => pt.IdTipo, pt => pt.IdTipoNavigation.Nombre),
                Description = p.Descripcion
            }).ToList();

            return viewModel;
        }

        public async Task<PokemonViewModel?> GetPokemonById(int id)
        {
            var pokemon = await _context.Pokemons
            .Where(p => p.IdPokemon == id)
            .Include(p => p.PokemonsTipos)
            .ThenInclude(pt => pt.IdTipoNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync();

            if (pokemon == null)
            {
                return null;
            }

            var viewModel = new PokemonViewModel
            {
                IdPokemon = pokemon.IdPokemon,
                Numero = pokemon.N,
                Name = pokemon.Nombre,
                Tipos = pokemon.PokemonsTipos
                    .ToDictionary(pt => pt.IdTipo, pt => pt.IdTipoNavigation.Nombre),
                Description = pokemon.Descripcion
            };

            return viewModel;
        }

        public async Task CreatePokemon(PokemonViewModel viewModel)
        {
            var pokemon = new Pokemon
            {
                N = viewModel.Numero,
                Nombre = viewModel.Name,
                Descripcion = viewModel.Description
            };

            List<PokemonsTipo> tipos = new List<PokemonsTipo>();

            foreach (var par in viewModel.Tipos)
            {
                tipos.Add( new PokemonsTipo {
                        IdTipo = par.Key,
                        IdPokemonNavigation = pokemon
                    }
                );
            }

            await _context.AddAsync(pokemon);
            await _context.AddRangeAsync(tipos);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePokemon(int id, PokemonViewModel viewModel)
        {
            var pokemon = await _context.Pokemons
                .FirstOrDefaultAsync(p => p.IdPokemon == id);

            if (pokemon != null)
            {
                pokemon.N = viewModel.Numero;
                pokemon.Nombre = viewModel.Name;
                pokemon.Descripcion = viewModel.Description;
            }

            // Recuperar los tipos existentes
            var oldTipos = await _context.PokemonsTipos
                .Include(pt => pt.IdPokemonNavigation)
                .Where(pt => pt.IdPokemon == id)
                .ToListAsync();

            // Agregar nuevos tipos o no según sea necesario
            foreach (var par in viewModel.Tipos)
            {
                // Comprobar existencia
                if (!oldTipos.Any(pt => pt.IdTipo == par.Key))
                {
                    // Agregar tipo nuevo
                    var newTipo = new PokemonsTipo
                    {
                        IdTipo = par.Key,
                        IdPokemonNavigation = pokemon
                    };

                    _context.Add(newTipo);
                }
            }

            // Obtener los IdTipo de viewModel.Tipos
            var tiposViewModel =  viewModel.Tipos.Select(t => t.Key).ToList();

            // Filtrar los PokemonsTipo que no están en tiposViewModel
            var tiposARemover = oldTipos.Where(pt => !tiposViewModel.Contains(pt.IdTipo)).ToList();

            // Eliminar tipos que ya no están presentes
            _context.PokemonsTipos.RemoveRange(tiposARemover);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);

            if (pokemon != null)
            {
                _context.Pokemons.Remove(pokemon);
                await _context.SaveChangesAsync();
            }
        }
    }
}
