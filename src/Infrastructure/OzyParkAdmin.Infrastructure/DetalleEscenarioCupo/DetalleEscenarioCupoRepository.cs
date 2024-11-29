using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.DetallesEscenariosCupos;

/// <summary>
/// El repositorio para manejar los detalles del escenario de cupo.
/// </summary>
public sealed class DetalleEscenarioCupoRepository(OzyParkAdminContext context) : Repository<DetalleEscenarioCupo>(context), IDetalleEscenarioCupoRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<DetalleEscenarioCupo>> GetDetallesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
    public async Task<IEnumerable<DetalleEscenarioCupo>> FindByIdsAsync(int escenarioCupoId, CancellationToken cancellationToken) =>
       await EntitySet.AsSplitQuery().Where(x => x.EscenarioCupoId == escenarioCupoId).ToListAsync(cancellationToken);
    /// <inheritdoc/>
    public async Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken)
    {
        await EntitySet.AddRangeAsync(detalles, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    /// <inheritdoc/>
    public async Task UpdateDetalleAsync(DetalleEscenarioCupo detalle, CancellationToken cancellationToken)
    {
        EntitySet.Update(detalle);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    /// <inheritdoc/>
    public async Task RemoveDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken)
    {
        EntitySet.RemoveRange(detalles);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
