using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Cajas;

/// <summary>
/// El repositorio de <see cref="Caja"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="CajaRepository"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class CajaRepository(OzyParkAdminContext context) : Repository<Caja>(context), ICajaRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Caja>> FindByIdsAsync(int[] cajaIds, CancellationToken cancellationToken)
    {
        return await EntitySet.Where(x => cajaIds.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<CajaInfo>> ListCajasAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        return centroCostoIds is null
            ? await EntitySet.AsNoTracking().Select(x => new CajaInfo { Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion }).ToListAsync(cancellationToken)
            : await EntitySet.AsNoTracking().Where(x => centroCostoIds.Contains(x.CentroCosto.Id)).Select(x => new CajaInfo { Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion }).ToListAsync(cancellationToken);
    }
}
