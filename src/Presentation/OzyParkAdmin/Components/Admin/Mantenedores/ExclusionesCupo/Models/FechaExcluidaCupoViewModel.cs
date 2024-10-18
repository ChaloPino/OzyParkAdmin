using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;

/// <summary>
/// El modelo de vista para las fechas excluidas para los cupos.
/// </summary>
public sealed record FechaExcluidaCupoViewModel
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// El canal de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// La fecha de la exclusión.
    /// </summary>
    public DateOnly Fecha { get; set; }

    internal bool Loading { get; set; }
}
