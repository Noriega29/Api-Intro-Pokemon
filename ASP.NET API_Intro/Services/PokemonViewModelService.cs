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
            var viewModel = await _context.Pokemons
                .AsNoTracking()
                .OrderBy(p => p.N)
                .Select(p => new PokemonViewModel
                {
                    IdPokemon = p.IdPokemon,
                    Numero = p.N,
                    Name = p.Nombre,
                    Tipos = p.PokemonsTipos
                    .OrderBy(pt => pt.Id)
                    .Select(pt => pt.IdTipo)
                    .ToList(),
                    Description = p.Descripcion
                }).ToListAsync();

            return viewModel;
        }

        public async Task<PokemonViewModel?> GetPokemonById(int id)
        {
            var pokemon = await _context.Pokemons
            .AsNoTracking()
            .Where(p => p.IdPokemon == id)
            .Include(p => p.PokemonsTipos)
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
                .OrderBy(pt => pt.Id)
                .Select(pt => pt.IdTipo)
                .ToList(),
                Description = pokemon.Descripcion
            };

            return viewModel;
        }

        public async Task CreatePokemon(PokemonViewModel viewModel)
        {
            var pokemon = new Pokemon
            {
                IdPokemon = viewModel.IdPokemon,
                N = viewModel.Numero,
                Nombre = viewModel.Name,
                Descripcion = viewModel.Description
            };

            if (viewModel.Tipos.Count == 1)
            {
                var tipo1 = new PokemonsTipo
                {
                    IdPokemonNavigation = pokemon,
                    IdTipo = viewModel.Tipos[0]
                };

                await _context.AddRangeAsync(pokemon, tipo1);
            }
            if (viewModel.Tipos.Count == 2)
            {
                var tipo1 = new PokemonsTipo
                {
                    IdPokemonNavigation = pokemon,
                    IdTipo = viewModel.Tipos[0]
                };
                var tipo2 = new PokemonsTipo
                {
                    IdPokemonNavigation = pokemon,
                    IdTipo = viewModel.Tipos[1]
                };

                await _context.AddRangeAsync(pokemon, tipo1, tipo2);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePokemon(int id, PokemonViewModel viewModel)
        {
            var pokemon = new Pokemon
            {
                IdPokemon = id,
                N = viewModel.Numero,
                Nombre = viewModel.Name,
                Descripcion = viewModel.Description
            };

            int countOldTipos = await _context.PokemonsTipos
                .AsNoTracking()
                .OrderBy(pt => pt.IdTipo)
                .Where(pt => pt.IdPokemon == id)
                .CountAsync();
            
            int countNewTipos = viewModel.Tipos.Count;

            try
            {
                if (countOldTipos == 1)
                {
                    //Id del primer tipo previo a actualizar.
                    var idTemp = await _context.PokemonsTipos
                        .AsNoTracking()
                        .OrderBy(i => i.Id)
                        .Where(p => p.IdPokemon == id)
                        .Select(i => i.Id)
                        .FirstOrDefaultAsync();

                    if (countNewTipos == 1)
                    {
                        //Asigna el nuevo valor al primer tipo
                        var a = new PokemonsTipo
                        {
                            Id = idTemp,
                            IdPokemonNavigation = pokemon,
                            IdTipo = viewModel.Tipos[0]
                        };

                        _context.UpdateRange(pokemon, a);
                        await _context.SaveChangesAsync();
                    }
                    if (countNewTipos == 2)
                    {
                        //Asigna el primer tipo nuevo
                        var a = new PokemonsTipo
                        {
                            Id = idTemp,
                            IdPokemonNavigation = pokemon,
                            IdTipo = viewModel.Tipos[0]
                        };

                        //Asigna el segundo tipo nuevo
                        var b = new PokemonsTipo
                        {
                            IdPokemonNavigation = pokemon,
                            IdTipo = viewModel.Tipos[1]
                        };

                        _context.UpdateRange(pokemon, a);
                        _context.Add(b);
                        await _context.SaveChangesAsync();
                    }
                }
                if (countOldTipos == 2)
                {
                    //Id del primer tipo previo a actualizar.
                    var idTemp1 = await _context.PokemonsTipos
                        .AsNoTracking()
                        .OrderBy(i => i.Id)
                        .Where(p => p.IdPokemon == id)
                        .Select(i => i.Id)
                        .FirstAsync();
                    //Id del segundo tipo previo a actualizar.
                    var idTemp2 = await _context.PokemonsTipos
                        .AsNoTracking()
                        .OrderBy(i => i.Id)
                        .Where(p => p.IdPokemon == pokemon.IdPokemon)
                        .Select(i => i.Id)
                        .LastAsync();

                    if (countNewTipos == 1)
                    {
                        //Asigna el primer tipo nuevo
                        var a = new PokemonsTipo
                        {
                            Id = idTemp1,
                            IdPokemonNavigation = pokemon,
                            IdTipo = viewModel.Tipos[0]
                        };

                        //Asigna el segundo tipo nuevo
                        var b = new PokemonsTipo
                        {
                            Id = idTemp2
                        };

                        _context.UpdateRange(pokemon, a);
                        _context.Remove(b);
                        await _context.SaveChangesAsync();
                    }
                    if (countNewTipos == 2)
                    {
                        //Asigna el primer tipo nuevo
                        var a = new PokemonsTipo
                        {
                            Id = idTemp1,
                            IdPokemonNavigation = pokemon,
                            IdTipo = viewModel.Tipos[0]
                        };

                        //Asigna el segundo tipo nuevo
                        var b = new PokemonsTipo
                        {
                            Id = idTemp2,
                            IdPokemonNavigation = pokemon,
                            IdTipo = viewModel.Tipos[1]
                        };

                        _context.UpdateRange(pokemon, a, b);
                        await _context.SaveChangesAsync();
                    }
                }
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
