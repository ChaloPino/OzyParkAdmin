using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.OmisionesCupo;

/// <summary>
/// El repositorio de <see cref="IgnoraEscenarioCupoExclusion"/>.
/// </summary>
public sealed class IgnoraEscenarioCupoExclusionRepository(OzyParkAdminContext context) : Repository<IgnoraEscenarioCupoExclusion>(context), IIgnoraEscenarioCupoExclusionRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<IgnoraEscenarioCupoExclusion>> FindByKeysAsync(IEnumerable<(int EscenarioCupoId, int CanalVentaId, DateOnly Fecha)> keys, CancellationToken cancellationToken)
    {
        int[] escenariosCuposId = [.. keys.Select(x => x.EscenarioCupoId)];
        int[] canalesVentaId = [.. keys.Select(x => x.CanalVentaId)];
        DateOnly[] fechas = [.. keys.Select(x => x.Fecha)];

        return await EntitySet.Where(x =>
            escenariosCuposId.Contains(x.EscenarioCupo.Id) &&
            canalesVentaId.Contains(x.CanalVenta.Id) &&
            fechas.Contains(x.FechaIgnorada))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

    }

    /// <inheritdoc/>
    public async Task<PagedList<IgnoraEscenarioCupoExclusionFullInfo>> SearchAsync(string? searchText, FilterExpressionCollection<IgnoraEscenarioCupoExclusion> filterExpressions, SortExpressionCollection<IgnoraEscenarioCupoExclusion> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        var query = EntitySet
            .AsNoTracking()
            .AsSplitQuery();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x =>
                x.EscenarioCupo.Nombre.Contains(searchText) ||
                x.CanalVenta.Nombre.Contains(searchText));
        }

        query = filterExpressions.Where(query);

        int totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var items = await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).Select(x => new IgnoraEscenarioCupoExclusionFullInfo
        {
            EscenarioCupo = new EscenarioCupoInfo { Id = x.EscenarioCupo.Id, Nombre = x.EscenarioCupo.Nombre, EsActivo = x.EscenarioCupo.EsActivo },
            CanalVenta = x.CanalVenta,
            FechaIgnorada = x.FechaIgnorada,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<IgnoraEscenarioCupoExclusionFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items,
        };
    }
}
