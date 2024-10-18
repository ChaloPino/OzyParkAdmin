using MudBlazor;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;

/// <summary>
/// Modelo para crear fechas excluidas para cupos.
/// </summary>
public sealed class FechasExcluidasCupoModel
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// Los canales de venta.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// El rango de fechas para la creación de las exclusiones.
    /// </summary>
    public DateRange RangoFechas { get; set; } = new(DateTime.Today, DateTime.Today);

    internal CanalVenta? ValidationCanalVenta => CanalesVenta.FirstOrDefault();

    internal bool Loading { get; set; }
}
