using DocumentFormat.OpenXml.Drawing.Diagrams;
using MudBlazor;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Models;

/// <summary>
/// El modelo para crear varias omisiones de fechas.
/// </summary>
public class OmisionesCupoExclusionModel
{
    /// <summary>
    /// El rango de fechas.
    /// </summary>
    public DateRange RangoFechas { get; set; } = new DateRange(DateTime.Now, DateTime.Now);

    /// <summary>
    /// Los escenario de cupo seleccionados.
    /// </summary>
    public IEnumerable<EscenarioCupoInfo> EscenariosCupo { get; set; } = [];

    /// <summary>
    /// Los canales de venta seleccionados.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Marca para indicar que se está procesando la creación de omisiones.
    /// </summary>
    internal bool Loading { get; set; }

    internal EscenarioCupoInfo? ValidationEscenariosCupo => EscenariosCupo.FirstOrDefault();

    internal CanalVenta? ValidationCanalesVenta => CanalesVenta.FirstOrDefault();
}
