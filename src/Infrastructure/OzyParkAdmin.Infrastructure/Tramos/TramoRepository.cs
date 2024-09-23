using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Tramos;

/// <summary>
/// El repositorio de <see cref="Tramo"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="OzyParkAdminContext"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class TramoRepository(OzyParkAdminContext context) : Repository<Tramo>(context), ITramoRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Tramo>> FindByIdsAsync(int[] tramoIds, CancellationToken cancellationToken) =>
        await EntitySet.Where(x => tramoIds.Contains(x.Id)).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<List<TramoInfo>> ListTramosAsync(CancellationToken cancellationToken)
    {
        return await EntitySet.AsNoTracking().Select(x => new TramoInfo {  Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion}).ToListAsync(cancellationToken);
    }
}
