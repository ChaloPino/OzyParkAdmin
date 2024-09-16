using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Franquicias;

namespace OzyParkAdmin.Domain.PuntosVenta;

/// <summary>
/// Entidad punto de venta.
/// </summary>
public sealed class PuntoVenta
{
    /// <summary>
    /// Identificador único del punto de venta.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Franquicia a la que pertenece el punto de venta.
    /// </summary>
    public Franquicia Franquicia { get; private set; } = default!;

    /// <summary>
    /// Aka del punto de venta.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Descripción del punto de venta.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Dirección del punto de venta.
    /// </summary>
    public string Direccion { get; private set; } = string.Empty;

    /// <summary>
    /// Fecha en que se creó el punto de venta.
    /// </summary>
    public DateTime FechaCreacion { get; private set; }

    /// <summary>
    /// Hora de inicio de la venta en las cajas.
    /// </summary>
    public TimeSpan HoraInicio { get; private set; }

    /// <summary>
    /// Hora de término de la venta en las cajas.
    /// </summary>
    public TimeSpan HoraTermino { get; private set; }

    /// <summary>
    /// Canal de venta asociado al punto de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; private set; } = default!;

    /// <summary>
    /// Si el punto de venta es de la red interna.
    /// </summary>
    public bool EsRedInterna { get; private set; }

    /// <summary>
    /// Si el punto de venta está activo o no.
    /// </summary>
    public bool EsActivo { get; private set; }

    /// <summary>
    /// Bodega asociada al punto de venta.
    /// </summary>
    public int? BodegaId { get; private set; }

    /// <summary>
    /// Crea un nuevo punto de venta.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="franquicia">Franquicia.</param>
    /// <param name="aka">Aka.</param>
    /// <param name="nombre">Nombre.</param>
    /// <param name="direccion">Dirección.</param>
    /// <param name="horaInicio">Hora inicio.</param>
    /// <param name="horaTermino">Hora término.</param>
    /// <param name="canalVenta">Canal de venta.</param>
    /// <param name="esRedInterna">Si es de la red interna.</param>
    /// <param name="bodegaId">Bodega asociada.</param>
    /// <returns>Un nuevo <see cref="PuntoVenta"/>.</returns>
    public static PuntoVenta Crear(
        int id,
        Franquicia franquicia,
        string aka,
        string nombre,
        string direccion,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        CanalVenta canalVenta,
        bool esRedInterna,
        int? bodegaId) =>
        new()
        {
            Id = id,
            Franquicia = franquicia,
            Aka = aka,
            Nombre = nombre,
            Direccion = direccion,
            HoraInicio = horaInicio,
            HoraTermino = horaTermino,
            CanalVenta = canalVenta,
            EsRedInterna = esRedInterna,
            BodegaId = bodegaId,
            FechaCreacion = DateTime.Now,
            EsActivo = true,
        };
}
