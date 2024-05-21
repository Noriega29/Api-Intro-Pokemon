using ASP.NET_API_Intro.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_API_Intro.Helpers
{
    public class FindEntities
    {
        private readonly PokedexContext _context;

        public FindEntities(PokedexContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca un Pokémon en la base de datos utilizando un ID, número o nombre.
        /// </summary>
        /// <param name="id">El ID del Pokémon a buscar.</param>
        /// <param name="numero">El número del Pokémon a buscar.</param>
        /// <param name="nombre">El nombre del Pokémon a buscar.</param>
        /// <returns>Devuelve true si se encuentra un Pokémon con los parámetros especificados; de lo contrario, false.</returns>
        /// <remarks>
        /// Este método realiza una búsqueda flexible que permite encontrar un Pokémon
        /// incluso si solo se proporciona uno de los parámetros de búsqueda a traves de los parametros nombrados.
        /// </remarks>
        public async Task<bool> FindPokemonByAnyParameter(int? id = null, int? numero = null, string? nombre = null)
        {
            // Realiza una consulta a la base de datos buscando un pokémon por el ID, el número o el nombre
            // y buscando coincidencias con el ID, el número o el nombre en los elementos.
            var existing = await _context.Pokemons
            .Where(p => (id.HasValue && p.IdPokemon == id) ||
                        (numero.HasValue && p.N == numero) ||
                        (nombre != null && p.Nombre == nombre))
            .AsNoTracking()
            .FirstOrDefaultAsync();

            // Si el resultado es nulo, retorna 'false'.
            return existing != null;
        }

        /// <summary>
        /// Busca la existencia de un Pokémon en la base de datos por número o nombre, excluyendo los elementos con un ID específico.
        /// </summary>
        /// <param name="id">El ID del Pokémon a excluir en la búsqueda.</param>
        /// <param name="numero">El número del Pokémon a buscar.</param>
        /// <param name="nombre">El nombre del Pokémon a buscar.</param>
        /// <returns>Devuelve true si existe un Pokémon que coincida con el número o nombre proporcionado, excluyendo el ID especificado; de lo contrario, false.</returns>
        /// <remarks>
        /// Este método es útil cuando se necesita verificar la unicidad de un número o nombre de Pokémon
        /// en operaciones de actualización, asegurando que no se considere el registro actualmente en edición.
        /// </remarks>
        public async Task<bool> FindPokemonByANumberOrNameExcludingId(int? id = null, int? numero = null, string? nombre = null)
        {
            // Realiza una consulta a la base de datos buscando un pokémon por el Número o el Nombre
            // y excluyendo el elemento con el ID proporcionado.
            var existing = await _context.Pokemons
            .Where(p => (id.HasValue && p.IdPokemon != id) &&
            ((numero.HasValue && p.N == numero) ||
            (nombre != null && p.Nombre == nombre)))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // Si el resultado es nulo, retorna 'false'.
            return existing != null;
        }

        /// <summary>
        /// Busca un Tipo en la base de datos utilizando un ID o un Nombre.
        /// </summary>
        /// <param name="id">El ID del Tipo a buscar.</param>
        /// <param name="nombre">El nombre del Tipo a buscar.</param>
        /// <returns>Devuelve true si se encuentra un Tipo con los parámetros especificados; de lo contrario, false.</returns>
        /// <remarks>
        /// Este método realiza una búsqueda flexible que permite encontrar un Tipo
        /// incluso si solo se proporciona uno de los parámetros de búsqueda a traves de los parametros nombrados.
        /// </remarks>
        public async Task<bool> FindTipoByAnyParameter(int? id = null, string? nombre = null)
        {
            // Realiza una consulta a la base de datos buscando un Tipo por el ID o el Nombre
            // y buscando coincidencias con el ID o el Nombre en los elementos.
            var existing = await _context.Tipos
                .Where(t => (id.HasValue && t.IdTipo == id) || (nombre != null && t.Nombre == nombre))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // Si el resultado es nulo, retorna 'false'.
            return existing != null;
        }
    }
}
