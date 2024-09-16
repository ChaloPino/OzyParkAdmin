using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.CentrosCosto;

/// <summary>
/// El repositorio de <see cref="CentroCosto"/>.
/// </summary>
/// <param name="context"></param>
public sealed class CentroCostoRepository(OzyParkAdminContext context) : Repository<CentroCosto>(context), ICentroCostoRepository
{
    /// <inheritdoc/>
    public async Task<List<CentroCosto>> ListCentrosCostoAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        return centroCostoIds is null
            ? await EntitySet.AsNoTracking().Where(x => x.EsActivo).OrderBy(x => x.Descripcion).ToListAsync(cancellationToken)
            : await EntitySet.AsNoTracking().Where(x => centroCostoIds.Contains(x.Id) && x.EsActivo).OrderBy(x => x.Descripcion).ToListAsync(cancellationToken);
    }
}
