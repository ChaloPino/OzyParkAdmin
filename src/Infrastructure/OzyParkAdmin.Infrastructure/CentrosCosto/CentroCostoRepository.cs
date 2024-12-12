using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Infrastructure.Shared;
using System.Reflection.Metadata.Ecma335;

namespace OzyParkAdmin.Infrastructure.CentrosCosto;

/// <summary>
/// El repositorio de <see cref="CentroCosto"/>.
/// </summary>
/// <param name="context"></param>
public sealed class CentroCostoRepository(OzyParkAdminContext context) : Repository<CentroCosto>(context), ICentroCostoRepository
{
    /// <inheritdoc/>
    public Task<CentroCosto?> FindByIdAsync(int centroCostoId, CancellationToken cancellationToken) =>
        EntitySet.AsNoTracking().FirstOrDefaultAsync(x => x.Id ==  centroCostoId, cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<CentroCosto>> FindByIdsAsync(int[] centroCostoIds, CancellationToken cancellationToken) =>
        await EntitySet.Where(x => centroCostoIds.Contains(x.Id)).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<List<CentroCostoInfo>> ListCentrosCostoAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        return centroCostoIds is null
            ? await EntitySet.AsNoTracking().Where(x => x.EsActivo).OrderBy(x => x.Descripcion).Select(x => new CentroCostoInfo {  Id = x.Id, Descripcion = x.Descripcion}).ToListAsync(cancellationToken)
            : await EntitySet.AsNoTracking().Where(x => centroCostoIds.Contains(x.Id) && x.EsActivo).OrderBy(x => x.Descripcion).Select(x => new CentroCostoInfo { Id = x.Id, Descripcion = x.Descripcion }).ToListAsync(cancellationToken);
    }
}
