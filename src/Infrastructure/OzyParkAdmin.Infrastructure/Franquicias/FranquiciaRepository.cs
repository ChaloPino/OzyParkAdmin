using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Franquicias;

/// <summary>
/// El repositorio de <see cref="Franquicia"/>.
/// </summary>
public sealed class FranquiciaRepository(OzyParkAdminContext context) : Repository<Franquicia>(context), IFranquiciaRepository
{
    /// <inheritdoc/>
    public async Task<List<Franquicia>> ListFranquiciasAsync(int[]? franquiciaIds, CancellationToken cancellationToken)
    {
        return franquiciaIds is null
            ? await EntitySet.AsNoTracking().Where(x => x.EsActivo).OrderBy(x => x.Nombre).ToListAsync(cancellationToken)
            : await EntitySet.AsNoTracking().Where(x => franquiciaIds.Contains(x.Id) && x.EsActivo).OrderBy(x => x.Nombre).ToListAsync(cancellationToken);
    }
}
