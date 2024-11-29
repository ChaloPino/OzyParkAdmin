using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Repositories;
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
            .AsSplitQuery()
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

}
