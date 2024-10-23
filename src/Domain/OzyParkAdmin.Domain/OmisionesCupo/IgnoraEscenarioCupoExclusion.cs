using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.OmisionesCupo;

/// <summary>
/// Entidad que define la fecha de omisión de exclusiones de un escenario de cupo.
/// </summary>
public class IgnoraEscenarioCupoExclusion
{
    /// <summary>
    /// El escenario de cupo.
    /// </summary>
    public EscenarioCupo EscenarioCupo { get; private init; } = default!;

    /// <summary>
    /// El canal de venta asociado.
    /// </summary>
    public CanalVenta CanalVenta { get; private init; } = default!;

    /// <summary>
    /// La fecha que se ignora.
    /// </summary>
    public DateOnly FechaIgnorada { get; private init; }

    internal static ResultOf<IgnoraEscenarioCupoExclusion> Create(
        EscenarioCupo escenarioCupo,
        CanalVenta canalVenta,
        DateOnly fechaIgnorada)
    {
        return new IgnoraEscenarioCupoExclusion
        {
            EscenarioCupo = escenarioCupo,
            FechaIgnorada = fechaIgnorada,
            CanalVenta = canalVenta,
        };
    }
}
