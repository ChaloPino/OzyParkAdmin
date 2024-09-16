using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Domain.Franquicias;

/// <summary>
/// Entidad franquicia.
/// </summary>
public sealed class Franquicia
{
    /// <summary>
    /// Identificador único de la franquicia.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Ak de la franquicia.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre de la franquicia.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Descripción de la franquicia.
    /// </summary>
    public string Descripcion { get; private set; } = string.Empty;

    /// <summary>
    /// Moneda predeterminada de la franquicia.
    /// </summary>
    public int MonedaId { get; private set; }

    /// <summary>
    /// Idioma predeterminado de la franquicia.
    /// </summary>
    public int IdiomaId { get; private set; }

    /// <summary>
    /// Ciudad de la franquicia.
    /// </summary>
    public int CiudadId { get; private set; }

    /// <summary>
    /// Ciudad asociada a la franquicia.
    /// </summary>
    public Ciudad Ciudad { get; private set; } = default!;

    /// <summary>
    /// Si la franquicia está activa o no.
    /// </summary>
    public bool EsActivo { get; private set; }

    /// <summary>
    /// Dirección de la franquicia.
    /// </summary>
    public string? Direccion { get; private set; }

    /// <summary>
    /// Dirección de correo electrónico de la franquicia.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Teléfono de contacto de la franquicia.
    /// </summary>
    public string? Telefono { get; private set; }

    /// <summary>
    /// Giro comercial de la franquicia.
    /// </summary>
    public string? Giro { get; private set; }

    /// <summary>
    /// RUT de la franquicia.
    /// </summary>
    public string? Rut { get; private set; }

    /// <summary>
    /// Si la franquicia es un emisor de boletas electrónicas.
    /// </summary>
    public bool EsEmisorElectronico { get; private set; }

    /// <summary>
    /// Crea una franquicia.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="aka">Aka</param>
    /// <param name="nombre">Nombre</param>
    /// <param name="descripcion">Descripción.</param>
    /// <param name="monedaId">Moneda.</param>
    /// <param name="idiomaId">Idioma.</param>
    /// <param name="ciudad">Ciudad.</param>
    /// <param name="direccion">Dirección.</param>
    /// <param name="email">Email.</param>
    /// <param name="telefono">Teléfono.</param>
    /// <param name="giro">Giro comercial.</param>
    /// <param name="rut">Rut.</param>
    /// <param name="esEmisorElectronico">Si es emisor de boletas electrónicas.</param>
    /// <returns>Una nueva <see cref="Franquicia"/>.</returns>
    public static Franquicia Crear(
        int id,
        string aka,
        string nombre,
        string descripcion,
        int monedaId,
        int idiomaId,
        Ciudad ciudad,
        string? direccion,
        string? email,
        string? telefono,
        string? giro,
        string? rut,
        bool esEmisorElectronico) =>
        new()
        {
            Id = id,
            Aka = aka,
            Nombre = nombre,
            Descripcion = descripcion,
            MonedaId = monedaId,
            IdiomaId = idiomaId,
            CiudadId = ciudad.Id,
            Ciudad = ciudad,
            Direccion = direccion,
            Email = email,
            Telefono = telefono,
            Giro = giro,
            Rut = rut,
            EsEmisorElectronico = esEmisorElectronico,
            EsActivo = true,
        };
}
