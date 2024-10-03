using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.EscenariosCupo;

/// <summary>
/// El repositorio de <see cref="EscenarioCupo"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/></param>.
public sealed class EscenarioCupoRepository(OzyParkAdminContext context) : Repository<EscenarioCupo>(context), IEscenarioCupoRepository
{
    /// <inheritdoc/>
    public async Task<EscenarioCupo?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await EntitySet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<List<EscenarioCupoInfo>> ListAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        IQueryable<EscenarioCupo> query = EntitySet;

        if (centroCostoIds is not null)
        {
            query = query.Where(x => centroCostoIds.Contains(x.CentroCosto.Id));
        }

        return await query.Select(x => new EscenarioCupoInfo { Id = x.Id, Nombre = x.Nombre, EsActivo = x.EsActivo }).ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
