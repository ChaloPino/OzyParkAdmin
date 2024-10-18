using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Infrastructure.ExclusionesCupo;

/// <summary>
/// El repositorio de <see cref="FechaExcluidaCupo"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class FechaExcluidaCupoRepository(OzyParkAdminContext context) : Repository<FechaExcluidaCupo>(context), IFechaExcluidaCupoRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<FechaExcluidaCupo>> FindFechasExcluidasAsync(int centroCostoId, int[] canalesVentaId, DateOnly[] fechas, CancellationToken cancellationToken)
    {
        return await EntitySet
            .Where(x =>
                centroCostoId == x.CentroCosto.Id &&
                canalesVentaId.Contains(x.CanalVenta.Id) &&
                fechas.Contains(x.Fecha))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FechaExcluidaCupo>> FindFechasExcluidasAsync(IEnumerable<FechaExcluidaCupoFullInfo> fechasExcluidas, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(fechasExcluidas);

        int[] centrosCostoId = [.. fechasExcluidas.Select(x => x.CentroCosto.Id)];
        int[] canalesVentaId = [.. fechasExcluidas.Select(x => x.CanalVenta.Id)];
        DateOnly[] fechas = [.. fechasExcluidas.Select(x => x.Fecha)];

        return await EntitySet
            .Where(x =>
                centrosCostoId.Contains(x.CentroCosto.Id) &&
                canalesVentaId.Contains(x.CanalVenta.Id) && 
                fechas.Contains(x.Fecha))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<PagedList<FechaExcluidaCupoFullInfo>> SearchAsync(int[]? centrosCostoId, string? searchText, FilterExpressionCollection<FechaExcluidaCupo> filterExpressions, SortExpressionCollection<FechaExcluidaCupo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        IQueryable<FechaExcluidaCupo> query = EntitySet.AsNoTracking().AsSplitQuery();

        if (centrosCostoId is not null)
        {
            query = query.Where(x => centrosCostoId.Contains(x.CentroCosto.Id));
        }

        if (searchText is not null)
        {
            query = query.Where(x =>
                x.CentroCosto.Descripcion.Contains(searchText) ||
                x.CanalVenta.Nombre.Contains(searchText));
        }

        query = filterExpressions.Where(query);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        List<FechaExcluidaCupoFullInfo> servicios = await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).Select(x => new FechaExcluidaCupoFullInfo
        {
            CentroCosto = new CentroCostoInfo { Id = x.CentroCosto.Id, Descripcion = x.CentroCosto.Descripcion },
            CanalVenta = x.CanalVenta,
            Fecha = x.Fecha,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<FechaExcluidaCupoFullInfo>()
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = servicios,
        };
    }
}
