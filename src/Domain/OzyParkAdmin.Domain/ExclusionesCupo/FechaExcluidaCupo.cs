using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// La entidad de una fecha excluida para un cupo.
/// </summary>
public sealed class FechaExcluidaCupo
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCosto CentroCosto { get; private init; } = default!;

    /// <summary>
    /// El id del canal de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; private init; } = default!;

    /// <summary>
    /// La fecha excluída para todos los cupos.
    /// </summary>
    public DateOnly Fecha { get; private init; }

    internal static ResultOf<FechaExcluidaCupo> Create(
        CentroCosto centroCosto,
        CanalVenta canalVenta,
        DateOnly fecha)
    {
        if (fecha < DateOnly.FromDateTime(DateTime.Today))
        {
            return new ValidationError(nameof(Fecha), "La fecha no debe ser menor al día de hoy.");
        }

        return new FechaExcluidaCupo()
        {
            CentroCosto = centroCosto,
            CanalVenta = canalVenta,
            Fecha = fecha
        };
    }
        
}
