using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Repositories;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Repositories;

/// <summary>
/// Repositorio para manejar la entidad <see cref="DetalleEscenarioCupoExclusionFecha"/>.
/// </summary>
public sealed class DetalleEscenarioCupoExclusionFechaRepository(OzyParkAdminContext context) : Repository<DetalleEscenarioCupoExclusionFecha>(context), IDetalleEscenarioCupoExclusionFechaRepository
{
    /// <summary>
    /// Obtiene las exclusiones de fechas para un escenario de cupo específico.
    /// </summary>
    public async Task<IEnumerable<DetalleEscenarioCupoExclusionFecha>> GetExclusionesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .AsNoTracking()
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .Join(
                context.Set<Servicio>().AsNoTracking(),
                exclusion => exclusion.ServicioId,
                servicio => servicio.Id,
                (exclusion, servicio) => new { exclusion, servicio }
                )
            .Join(
                context.Set<CanalVenta>().AsNoTracking(),
                combined => combined.exclusion.CanalVentaId,
                canal => canal.Id,
                (combined, canal) => new DetalleEscenarioCupoExclusionFecha
                {
                    EscenarioCupoId = combined.exclusion.EscenarioCupoId,
                    ServicioId = combined.exclusion.ServicioId,
                    CanalVentaId = combined.exclusion.CanalVentaId,
                    FechaExclusion = combined.exclusion.FechaExclusion,
                    HoraInicio = combined.exclusion.HoraInicio,
                    HoraFin = combined.exclusion.HoraFin,
                    Servicio = combined.servicio, 
                    CanalVenta = canal 
                }
            )
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<DetalleEscenarioCupoExclusionFecha>> GetSimpleExclusionesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .AsNoTracking()
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

}
