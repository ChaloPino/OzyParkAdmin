using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using RazorEngineCore;
using System.Collections.Immutable;

namespace OzyParkAdmin.Infrastructure.CuposFecha;

/// <summary>
/// El repositorio de <see cref="CupoFecha"/>.
/// </summary>
public sealed class CupoFechaRepository(OzyParkAdminContext context) : Repository<CupoFecha>(context), ICupoFechaRepository
{
    /// <inheritdoc/>
    public void Delete(CupoFecha cupoFecha)
    {
        EntitySet.Remove(cupoFecha);
    }

    /// <inheritdoc/>
    public async Task<CupoFecha?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await EntitySet.AsSingleQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CupoFecha>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken)
    {
        return await EntitySet.AsSingleQuery().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<CupoFecha?> FindByUniqueKeyAsync(DateOnly fecha, EscenarioCupo escenarioCupo, CanalVenta canalVenta, DiaSemana diaSemana, TimeSpan horaInicio, CancellationToken cancellationToken)
    {
        return await EntitySet.AsNoTracking().AsSingleQuery().FirstOrDefaultAsync(x =>
            x.Fecha == fecha &&
            x.EscenarioCupo.Id == escenarioCupo.Id &&
            x.CanalVenta.Id == canalVenta.Id &&
            x.DiaSemana.Id == diaSemana.Id &&
            x.HoraInicio == horaInicio,
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CupoFecha>> FindByUniqueKeysAsync((DateOnly Fecha, EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio)[] uniqueKey, CancellationToken cancellationToken)
    {
        DateOnly[] fechas = uniqueKey.Select(x => x.Fecha).ToArray();
        int[] escenariosCuposId = uniqueKey.Select(x => x.EscenarioCupo.Id).ToArray();
        int[] canalesVentaId = uniqueKey.Select(x => x.CanalVenta.Id).ToArray();
        int[] diasSemanaId = uniqueKey.Select(x => x.DiaSemana.Id).ToArray();
        TimeSpan[] horasInicio = uniqueKey.Select(x => x.HoraInicio).ToArray();

        return await EntitySet.AsNoTracking().AsSingleQuery().Where(x =>
            fechas.Contains(x.Fecha) &&
            escenariosCuposId.Contains(x.EscenarioCupo.Id) &&
            canalesVentaId.Contains(x.CanalVenta.Id) &&
            diasSemanaId.Contains(x.DiaSemana.Id) &&
            horasInicio.Contains(x.HoraInicio))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CupoFecha>> FindByUniqueKeysAsync(
        DateOnly fechaDesde,
        DateOnly fechaHasta,
        EscenarioCupoInfo escenarioCupo,
        ImmutableArray<CanalVenta> canalesVenta,
        ImmutableArray<DiaSemana> diasSemana,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        int intervaloMinutos,
        CancellationToken cancellationToken)
    {
        DateOnly[] fechas = CreateFechas(fechaDesde, fechaHasta).ToArray();
        TimeSpan[] horas = CreateHoras(horaInicio, horaTermino, intervaloMinutos).ToArray();
        int escenarioCupoId = escenarioCupo.Id;
        int[] canalesVentaId = [.. canalesVenta.Select(x => x.Id)];
        int[] diasSemanaId = [.. diasSemana.Select(x => x.Id)];

        return await EntitySet.AsSingleQuery().Where(x =>
            fechas.Contains(x.Fecha) &&
            x.EscenarioCupo.Id == escenarioCupoId &&
            canalesVentaId.Contains(x.CanalVenta.Id) &&
            diasSemanaId.Contains(x.DiaSemana.Id) &&
            horas.Contains(x.HoraInicio))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IEnumerable<TimeSpan> CreateHoras(TimeSpan horaInicio, TimeSpan horaTermino, int intervaloMinutos)
    {
        TimeSpan time = horaInicio;

        do
        {
            yield return time;
            time = time.Add(TimeSpan.FromSeconds(intervaloMinutos * 60));
        } while (time <= horaTermino);
    }

    private static IEnumerable<DateOnly> CreateFechas(DateOnly fechaDesde, DateOnly fechaHasta)
    {
        DateOnly date = fechaDesde;

        do
        {
            yield return date;
            date = date.AddDays(1);
        } while (date <= fechaHasta);
    }

    /// <inheritdoc/>
    public async Task<int> MaxIdAsync(CancellationToken cancellationToken)
    {
        int? id = await EntitySet.MaxAsync(x => (int?)x.Id, cancellationToken).ConfigureAwait(false);
        return id ?? 0;
    }

    /// <inheritdoc/>
    public async Task<PagedList<CupoFechaFullInfo>> SearchAsync(int[]? centroCostoIds, string? searchText, FilterExpressionCollection<CupoFecha> filterExpressions, SortExpressionCollection<CupoFecha> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        var query = EntitySet.AsNoTracking().Select(x => new
        {
            x.Id,
            x.Fecha,
            CentroCostoId = x.EscenarioCupo.CentroCosto.Id,
            EscenarioCupoId = x.EscenarioCupo.Id,
            EscenarioCupoNombre = x.EscenarioCupo.Nombre,
            CanalVentaId = x.CanalVenta.Id,
            CanalVentaNombre = x.CanalVenta.Nombre,
            DiaSemanaId = x.DiaSemana.Id,
            DiaSemanaAka = x.DiaSemana.Aka,
            x.HoraInicio,
        });

        if (centroCostoIds is not null)
        {
            query = query.Where(x => centroCostoIds.Contains(x.CentroCostoId));
        }

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

        IEnumerable<CupoFechaFullInfo> items = await sortExpressions.Sort(finalQuery).Skip(page * pageSize).Take(pageSize).Select(x => new CupoFechaFullInfo
        {
            Id = x.Id,
            Fecha = x.Fecha,
            EscenarioCupo = new EscenarioCupoInfo { Id = x.EscenarioCupo.Id, Nombre = x.EscenarioCupo.Nombre, EsActivo = x.EscenarioCupo.EsActivo },
            CanalVenta = x.CanalVenta,
            DiaSemana = x.DiaSemana,
            HoraInicio = x.HoraInicio,
            HoraFin = x.HoraFin,
            Total = x.Total,
            SobreCupo = x.SobreCupo,
            TopeEnCupo = x.TopeEnCupo,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<CupoFechaFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = items,
        };
    }
}
