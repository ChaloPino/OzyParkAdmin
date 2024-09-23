using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// La zona de ruta del servicio.
/// </summary>
public sealed class ZonaRuta
{
    private readonly List<ZonaRutaDestino> _destinos = [];
    private readonly List<ZonaRutaDetalle> _detalle = [];
    /// <summary>
    /// El tramo asociado a la zona ruta.
    /// </summary>
    public Tramo Tramo { get; private set; } = default!;

    /// <summary>
    /// La zona de origen de la zona ruta.
    /// </summary>
    public Zona ZonaOrigen { get; private set; } = default!;

    /// <summary>
    /// El sentido que tiene esta zona ruta.
    /// </summary>
    public Sentido Sentido { get; private set; } = default!;


    /// <summary>
    /// El sentido de control que tiene esta zona ruta.
    /// </summary>
    public Sentido SentidoControl { get; private set; } = default!;

    /// <summary>
    /// Los posibles destinos de esta zona ruta.
    /// </summary>
    public IEnumerable<ZonaRutaDestino> Destinos => _destinos;

    /// <summary>
    /// Los detalles de esta zona ruta.
    /// </summary>
    public IEnumerable<ZonaRutaDetalle> Detalle => _detalle;
}