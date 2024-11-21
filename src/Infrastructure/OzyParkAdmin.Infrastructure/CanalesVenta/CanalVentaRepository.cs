using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.CanalesVenta;

/// <summary>
/// El repositorio de <see cref="CanalVenta"/>
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="CanalVentaRepository"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class CanalVentaRepository(OzyParkAdminContext context) : Repository<CanalVenta>(context), ICanalVentaRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<CanalVenta>> FindByIdsAsync(int[] canalesVentaIds, CancellationToken cancellationToken)
    {
        return await EntitySet.Where(x => canalesVentaIds.Contains(x.Id)).ToListAsync(cancellationToken);
    }
}
