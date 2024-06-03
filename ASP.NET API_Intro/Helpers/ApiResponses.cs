using System.Text.Json.Serialization;

namespace ASP.NET_API_Intro.Helpers
{
    /// <summary>
    /// Clase para estandarizar las respuestas de la API.
    /// </summary>
    public class ApiResponses
    {
        /// <summary>
        /// Obtiene o establece el titulo de la respuesta.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Obtiene o establece el estado de la respuesta.
        /// </summary>
        public string Status {  get; set; }
        /// <summary>
        /// Obtiene o establece el estado de la respuesta.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Obtiene o establece los datos de la respuesta. Se ignora si es nulo.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Data { get; set; }

        /// <summary>
        /// Constructor para inicializar una respuesta de la API con estado, mensaje y datos opcionales.
        /// </summary>
        /// <param name="title">El titulo de la respuesta.</param>
        /// <param name="status">El estado de la respuesta.</param>
        /// <param name="message">El mensaje de la respuesta.</param>
        /// <param name="data">Los datos de la respuesta.</param>
        public ApiResponses(string title, string status, string message, object? data = null)
        {
            Title = title;
            Status = status;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Crea una respuesta para una entidad creada con éxito.
        /// </summary>
        /// <param name="entity">Es el tipo de entidad creada.</param>
        /// <param name="data">Los datos de la entidad creada.</param>
        /// <returns>Una respuesta indicando que la entidad fue creada con éxito.</returns>
        public static ApiResponses Status201CreatedEntity(string entity, object data)
        {
            return new ApiResponses("Created", "201x1", $"{entity} creado con éxito.", data);
        }

        /// <summary>
        /// Crea una respuesta para una entidad borrada con éxito.
        /// </summary>
        /// <param name="entity">Es el tipo de entidad borrada.</param>
        /// <returns>Una respuesta indicando que la entidad fue borrada con éxito.</returns>
        public static ApiResponses Status204DeletedEntity(string entity)
        {
            return new ApiResponses("No Content", "204x1", $"{entity} borrado con éxito.");
        }

        /// <summary>
        /// Crea una respuesta de error cuando hay una incongruencia entre el ID de la URI y el cuerpo de la solicitud.
        /// </summary>
        /// <param name="uriId">El ID del Pokémon en la URI.</param>
        /// <param name="bodyId">El ID del Pokémon en el cuerpo de la solicitud.</param>
        /// <returns>Una respuesta de error indicando la incongruencia de IDs.</returns>
        public static ApiResponses Status400IncongruentId(int uriId, int bodyId)
        {
            return new ApiResponses("Bad Request", "400x1", $"El ID '{uriId}' de la URL no coincide con el ID '{bodyId}' del cuerpo de la solicitud.");
        }

        /// <summary>
        /// Crea una respuesta de error cuando el número del Pokémon no es válido.
        /// </summary>
        /// <returns>Una respuesta de error indicando que el número del Pokémon no es válido.</returns>
        public static ApiResponses Status400NotValidNumber()
        {
            return new ApiResponses("Bad Request", "400x2", "El número del pokémon en la pokédex debe ser mayor a cero.");
        }

        /// <summary>
        /// Crea una respuesta de error cuando la cantidad de tipos de un Pokémon es inválida.
        /// </summary>
        /// <returns>Una respuesta de error indicando que la cantidad de tipos es inválida.</returns>
        public static ApiResponses Status400InvalidTiposCount()
        {
            return new ApiResponses("Bad Request", "400x3", "El pokémon debe tener al menos un tipo, pero no más de dos.");
        }

        /// <summary>
        /// Crea una respuesta de error cuando no se encuentra una Entidad con el ID especificado.
        /// </summary>
        /// <param name="entity">Es el tipo de entidad que no se encuentra.</param>
        /// <param name="id">El ID de la entidad que no se encuentra.</param>
        /// <returns>Una respuesta de error indicando que la Entidad no se encuentra.</returns>
        public static ApiResponses Status404NotFoundEntity(string entity, int id)
        {
            return new ApiResponses("Not Found", "404x1", $"El {entity} con el ID '{id}' no se encuentra en la base de datos.");
        }

        /// <summary>
        /// Crea una respuesta de error para una excepción de concurrencia.
        /// </summary>
        /// <returns>Una respuesta de error indicando un problema de concurrencia al intentar actualizar un campo en la base de datos.</returns>
        public static ApiResponses Status409DbUpdateConcurrencyException()
        {
            return new ApiResponses("Conflict", "409x1", "Esto sucede cuando varios procesos intentan modificar los mismos datos al mismo tiempo.");
        }

        /// <summary>
        /// Crea una respuesta de error cuando ya existe un Pokémon con el mismo número en la base de datos.
        /// </summary>
        /// <param name="number">El número del Pokémon que ya existe.</param>
        /// <returns>Una respuesta de error indicando que el número del Pokémon ya existe.</returns>
        public static ApiResponses Status409ExistingPokemonNumber(int number)
        {
            return new ApiResponses("Conflict", "409x2", $"Ya existe un pokémon en la base de datos con el número '{number}'.");
        }

        /// <summary>
        /// Crea una respuesta de error cuando ya existe una entidad con el mismo nombre en la base de datos.
        /// </summary>
        /// <param name="entity">Es el tipo de entidad existente.</param>
        /// <param name="name">El nombre de la entidad existente.</param>
        /// <returns>Una respuesta de error indicando que el nombre de la entidad ya existe.</returns>
        public static ApiResponses Status409ExistingEntityByName(string entity, string name)
        {
            return new ApiResponses("Conflict", "409x3", $"Ya existe un {entity} en la base de datos con el nombre '{name}'.");
        }

        /// <summary>
        /// Crea una respuesta de error para una excepción de MySQL.
        /// </summary>
        /// <returns>Una respuesta de error indicando un problema al conectar con el hosts MySQL.</returns>
        public static ApiResponses Status500MySqlException()
        {
            return new ApiResponses("Internal Server Error", "500x1", "No se ha podido conectar a ninguno de los hosts MySQL especificados.");
        }

        /// <summary>
        /// Crea una respuesta de error para una excepción de socket.
        /// </summary>
        /// <returns>Una respuesta de error indicando un problema de conexión de red.</returns>
        public static ApiResponses Status500SocketException()
        {
            return new ApiResponses("Internal Server Error", "500x2", "No se puede establecer una conexión ya que el equipo de destino denegó expresamente dicha conexión.");
        }
    }

    ///// <summary>
    ///// Clase estática que contiene mensajes de error constantes.
    ///// </summary>
    //public static class ErrorMessages
    //{
    //    public const string IncongruentId = "El ID '{0}' de la URL no coincide con el ID '{1}' del cuerpo de la solicitud.";

    //    public const string NotFoundEntity = "El {0} con el ID '{1}' no se encuentra en la base de datos.";

    //    public const string ExistingPokemonNumber = "Ya existe un pokémon en la base de datos con el número '{0}'.";

    //    public const string ExistingEntityByName = "Ya existe un {0} en la base de datos con el nombre '{1}'.";

    //    public const string NotValidNumber = "El número del pokémon en la pokédex debe ser mayor a cero.";

    //    public const string InvalidTiposCount = "El pokémon debe tener al menos un tipo, pero no más de dos.";
    //}
}
