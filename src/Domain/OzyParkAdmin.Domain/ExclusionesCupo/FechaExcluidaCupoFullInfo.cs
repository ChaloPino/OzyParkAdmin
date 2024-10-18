using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// Contiene toda la información de una fecha excluída para los cupos.
/// </summary>
public sealed record FechaExcluidaCupoFullInfo
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public required CentroCostoInfo CentroCosto { get; set; }

    /// <summary>
    /// El canal de venta.
    /// </summary>
    public required CanalVenta CanalVenta { get; set; }

    /// <summary>
    /// La fecha excluída.
    /// </summary>
    public DateOnly Fecha { get; set; }
}
