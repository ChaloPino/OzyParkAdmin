using Dapper;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Cupos;

/// <summary>
/// El repositorio de <see cref="Cupo"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class CupoRepository(OzyParkAdminContext context) : Repository<Cupo>(context), ICupoRepository
{
    /// <inheritdoc/>
    public async Task<Cupo?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await EntitySet.AsSingleQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Cupo?> FindByUniqueKey(DateOnly fechaEfectiva, EscenarioCupo escenarioCupo, CanalVenta canalVenta, DiaSemana diaSemana, TimeSpan horaInicio, CancellationToken cancellationToken)
    {
        return await EntitySet.AsNoTracking().AsSingleQuery().FirstOrDefaultAsync(x =>
            x.FechaEfectiva == fechaEfectiva &&
            x.EscenarioCupo.Id == escenarioCupo.Id &&
            x.CanalVenta.Id == canalVenta.Id &&
            x.DiaSemana.Id == diaSemana.Id &&
            x.HoraInicio == horaInicio,
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> MaxIdAsync(CancellationToken cancellationToken)
    {
        int? id = await EntitySet.MaxAsync(x => (int?)x.Id, cancellationToken).ConfigureAwait(false);
        return id ?? 0;
    }

    /// <inheritdoc/>
    public async Task<PagedList<CupoFullInfo>> SearchAsync(int[]? centroCostoIds, string? searchText, FilterExpressionCollection<Cupo> filterExpressions, SortExpressionCollection<Cupo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        var query = EntitySet.AsNoTracking().Select(x => new
        {
            x.Id,
            CentroCostoId = x.EscenarioCupo.CentroCosto.Id,
            EscenarioCupoId = x.EscenarioCupo.Id,
            EscenarioCupoNombre = x.EscenarioCupo.Nombre,
            CanalVentaId = x.CanalVenta.Id,
            CanalVentaNombre = x.CanalVenta.Nombre,
            DiaSemanaId = x.DiaSemana.Id,
            DiaSemanaAka = x.DiaSemana.Aka,
            x.HoraInicio,
            x.FechaEfectiva,
        });

        if (centroCostoIds is not null)
        {
            query = query.Where(x => centroCostoIds.Contains(x.CentroCostoId));
        }

        var maxQuery = from cupo in query
                       where cupo.FechaEfectiva <= today
                       group cupo by new { cupo.EscenarioCupoId, cupo.CanalVentaId, cupo.DiaSemanaId, cupo.HoraInicio } into groupCupo
                       select new
                       {
                           groupCupo.Key.EscenarioCupoId,
                           groupCupo.Key.CanalVentaId,
                           groupCupo.Key.DiaSemanaId,
                           groupCupo.Key.HoraInicio,
                           FechaEfectiva = groupCupo.Max(g => g.FechaEfectiva),
                       };

        var joinQuery = from cupo in query
                        join maxCupo in maxQuery
                        on new { cupo.EscenarioCupoId, cupo.CanalVentaId, cupo.DiaSemanaId, cupo.HoraInicio } equals new { maxCupo.EscenarioCupoId, maxCupo.CanalVentaId, maxCupo.DiaSemanaId, maxCupo.HoraInicio }
                        select cupo;


        query = joinQuery.Union(query.Where(x => x.FechaEfectiva > today));

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x =>
                x.EscenarioCupoNombre.Contains(searchText) ||
                x.CanalVentaNombre.Contains(searchText) ||
                x.DiaSemanaAka.Contains(searchText));
        }

        var finalQuery = from cupo in EntitySet
                         join unionCupo in query on cupo.Id equals unionCupo.Id
                         select cupo;

        finalQuery = filterExpressions.Where(finalQuery);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        IEnumerable<CupoFullInfo> items = await sortExpressions.Sort(finalQuery).Skip(page * pageSize).Take(pageSize).Select(x => new CupoFullInfo
        {
            Id = x.Id,
            EscenarioCupo = new EscenarioCupoInfo { Id = x.EscenarioCupo.Id, Nombre = x.EscenarioCupo.Nombre, EsActivo = x.EscenarioCupo.EsActivo },
            CanalVenta = x.CanalVenta,
            DiaSemana = x.DiaSemana,
            HoraInicio = x.HoraInicio,
            HoraFin = x.HoraFin,
            Total = x.Total,
            SobreCupo = x.SobreCupo,
            TopeEnCupo = x.TopeEnCupo,
            FechaEfectiva = x.FechaEfectiva,
            UltimaModificacion = x.UltimaModificacion,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<CupoFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = items,
        };
    }

    /// <inheritdoc/>
    public async Task<List<CupoFechaInfo>> SearchCuposParaCalendarioAsync(int canalVentaId, string alcance, int servicioId, int? zonaOrigenId, DateTime inicio, int dias, CancellationToken cancellationToken)
    {
        CommandDefinition commandDefinition = new(
            commandText: "qsp_getCupos_prc",
            parameters: new Dictionary<string, object?>
            {
                ["@CanalVentaId"] = canalVentaId,
                ["@Alcance"] = alcance,
                ["@ServicioId"] = servicioId,
                ["@ZonaOrigenId"] = zonaOrigenId,
                ["@FechaInicio"] = inicio,
                ["@Dias"] = dias
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        IEnumerable<CupoPorDiaInfo> cupos = await Context.Database.GetDbConnection().QueryAsync<CupoPorDiaInfo>(commandDefinition);

        return cupos
            .GroupBy(x => x.Fecha)
            .Select(x => new CupoFechaInfo(x.Key, [.. x.Select(h => new CupoHoraInfo(h.HoraInicio, h.HoraFin, h.CupoTotal, h.CupoDisponible))]))
            .ToList();
    }
}
