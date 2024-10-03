namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Información de detalle de la apertura de caja.
/// </summary>
public sealed record AperturaCajaDetalleInfo
{
    /// <summary>
    /// Los turnos del día.
    /// </summary>
    public List<TurnoCajaInfo> Turnos { get; set; } = [];

    /// <summary>
    /// Los servicios vendidos en el día.
    /// </summary>
    public List<ServicioDiaInfo> Servicios { get; set; } = [];
}
