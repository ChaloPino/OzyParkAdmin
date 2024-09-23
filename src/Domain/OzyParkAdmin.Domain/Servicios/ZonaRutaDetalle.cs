using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El detalle de una zona ruta.
/// </summary>
public class ZonaRutaDetalle
{
    /// <summary>
    /// La zona del detalle de zona ruta.
    /// </summary>
    public Zona Zona { get; private set; } = default!;
    /// <summary>
    /// El sentido de control del detalle.
    /// </summary>
    public Sentido SentidoControl { get; private set; } = default!;

    /// <summary>
    /// Si el detalle es de retorno.
    /// </summary>
    public bool EsRetorno { get; private set; }

    /// <summary>
    /// si el detalle es de combinación.
    /// </summary>
    public bool EsCombinacion { get; private set; }

    /// <summary>
    /// El orden en el que pasa por el detalle.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// La cantidad de pasadas que se puede hacer en el detalle.
    /// </summary>
    public int CantidadPasadas { get; private set; }

    /// <summary>
    /// Si el detalle está activo.
    /// </summary>
    public bool EsActivo { get; set; }
}