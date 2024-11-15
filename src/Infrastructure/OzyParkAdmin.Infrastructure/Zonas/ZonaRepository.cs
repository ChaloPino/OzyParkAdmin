using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Zonas;

/// <summary>
/// El repositorio de <see cref="Zona"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="ZonaRepository"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class ZonaRepository(OzyParkAdminContext context) : Repository<Zona>(context), IZonaRepository
{
    /// <inheritdoc/>
    public async Task<Zona?> FindByIdAsync(int zonaId, CancellationToken cancellationToken)
   => await EntitySet.Where(x => x.Id == zonaId).FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<Zona>> FindByIdsAsync(int[] zonaIds, CancellationToken cancellationToken) =>
        await EntitySet.Where(x => zonaIds.Contains(x.Id)).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<List<ZonaInfo>> ListZonasAsync(CancellationToken cancellationToken)
    {
        return EntitySet.AsNoTracking().Select(x => new ZonaInfo { Id = x.Id, Descripcion = x.Descripcion }).ToListAsync(cancellationToken);
    }
}
