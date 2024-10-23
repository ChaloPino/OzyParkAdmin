using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Domain.OmisionesCupo;

/// <summary>
/// Contiene la información de una omisión de exclusión de un escenario de cupo.
/// </summary>
public sealed record IgnoraEscenarioCupoExclusionFullInfo
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
