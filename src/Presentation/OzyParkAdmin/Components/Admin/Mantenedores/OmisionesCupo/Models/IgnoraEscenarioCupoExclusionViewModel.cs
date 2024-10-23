using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Models;

/// <summary>
/// El modelo de omisión de exclusión.
/// </summary>
public sealed class IgnoraEscenarioCupoExclusionViewModel
{
    /// <summary>
    /// El escenario de cupo.
    /// </summary>
    public EscenarioCupoInfo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// El canal de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// La fecha ignorada.
    /// </summary>
    public DateOnly FechaIgnorada { get; set; }
}
