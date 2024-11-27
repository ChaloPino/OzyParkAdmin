using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasServicio;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.TarifasServicio;

/// <summary>
/// El repositorio de <see cref="TarifaServicio"/>.
/// </summary>
public sealed class TarifaServicioRepository(OzyParkAdminContext context) : Repository<TarifaServicio>(context), ITarifaServicioRepository
{
    /// <inheritdoc/>
    public async Task<TarifaServicio?> FindByPrimaryKeyAsync(DateTime inicioVigencia, int monedaId, int servicioId, int tramoId, int grupoEtarioId, int canalVentaId, int tipoDiaId, int tipoHorarioId, int tipoSegmentacionId, CancellationToken cancellationToken)
    {
        return await EntitySet.FirstOrDefaultAsync(x =>
            x.InicioVigencia == inicioVigencia &&
            x.Moneda.Id == monedaId &&
            x.Servicio.Id == servicioId &&
            x.Tramo.Id == tramoId &&
            x.GrupoEtario.Id == grupoEtarioId &&
            x.CanalVenta.Id == canalVentaId &&
            x.TipoDia.Id == tipoDiaId &&
            x.TipoHorario.Id == tipoHorarioId &&
            x.TipoSegmentacion.Id == tipoSegmentacionId)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<(DateTime InicioVigencia, Moneda Moneda, Servicio Servicio, Tramo Tramo, GrupoEtario GrupoEtario, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario, TipoSegmentacion TipoSegmentacion)>> FindByPrimaryKeysAsync(List<(DateTime InicioVigencia, Moneda Moneda, Servicio Servicio, Tramo Tramo, GrupoEtario GrupoEtario, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario, TipoSegmentacion TipoSegmentacion)> primaryKeys, CancellationToken cancellationToken)
    {
        DateTime[] iniciosVigencia = primaryKeys.Select(x => x.InicioVigencia).ToArray();
        int[] monedasId = primaryKeys.Select(x => x.Moneda.Id).ToArray();
        int[] serviciosId = primaryKeys.Select(x => x.Servicio.Id).ToArray();
        int[] tramosId = primaryKeys.Select(x => x.Tramo.Id).ToArray();
        int[] gruposEtariosId = primaryKeys.Select(x => x.GrupoEtario.Id).ToArray();
        int[] canalesVentaId = primaryKeys.Select(x => x.CanalVenta.Id).ToArray();
        int[] tiposDiaId = primaryKeys.Select(x => x.TipoDia.Id).ToArray();
        int[] tiposHorarioId = primaryKeys.Select(x => x.TipoHorario.Id).ToArray();
        int[] tiposSegmentacionId = primaryKeys.Select(x => x.TipoSegmentacion.Id).ToArray();
        var tarifas = await EntitySet.Where(x =>
            Enumerable.Contains(iniciosVigencia, x.InicioVigencia) &&
            Enumerable.Contains(monedasId, x.Moneda.Id) &&
            Enumerable.Contains(serviciosId, x.Servicio.Id) &&
            Enumerable.Contains(tramosId, x.Tramo.Id) &&
            Enumerable.Contains(gruposEtariosId, x.GrupoEtario.Id) &&
            Enumerable.Contains(canalesVentaId, x.CanalVenta.Id) &&
            Enumerable.Contains(tiposDiaId, x.TipoDia.Id) &&
            Enumerable.Contains(tiposHorarioId, x.TipoHorario.Id) &&
            Enumerable.Contains(tiposSegmentacionId, x.TipoSegmentacion.Id))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return tarifas.ConvertAll(x => (x.InicioVigencia, x.Moneda, x.Servicio, x.Tramo, x.GrupoEtario, x.CanalVenta, x.TipoDia, x.TipoHorario, x.TipoSegmentacion));
    }

    /// <inheritdoc/>
    public async Task<PagedList<TarifaServicio>> SearchTarifasServiciosAsync(int centroCostoId, string? searchText, FilterExpressionCollection<TarifaServicio> filterExpressions, SortExpressionCollection<TarifaServicio> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        DateTime now = DateTime.Now;

        IQueryable<TarifaServicio> query = EntitySet.IgnoreAutoIncludes().AsNoTracking().AsSplitQuery().Where(x => x.Servicio.CentroCostoId == centroCostoId);

        if (searchText is not null)
        {
            query = query.Where(x =>
                x.Moneda.Nombre.Contains(searchText) ||
                x.Servicio.Nombre.Contains(searchText) ||
                x.Tramo.Descripcion.Contains(searchText) ||
                x.GrupoEtario.Descripcion.Contains(searchText) ||
                x.TipoDia.Descripcion.Contains(searchText) ||
                x.TipoHorario.Descripcion.Contains(searchText) ||
                x.CanalVenta.Aka.Contains(searchText) ||
                x.TipoSegmentacion.Descripcion.Contains(searchText));
        }

        query = query.Include(x => x.Moneda)
            .Include(x => x.Servicio)
            .Include(x => x.Tramo)
            .Include(x => x.GrupoEtario)
            .Include(x => x.TipoDia)
            .Include(x => x.TipoHorario)
            .Include(x => x.CanalVenta)
            .Include(x => x.TipoSegmentacion);

        query = filterExpressions.Where(query);

        var tarifasFuturas = query
            .Where(x => x.InicioVigencia >= now)
            .Select(t => new
            {
                MonedaId = t.Moneda.Id,
                ServicioId = t.Servicio.Id,
                TramoId = t.Tramo.Id,
                GrupoEtarioId = t.GrupoEtario.Id,
                CanalVentaId = t.CanalVenta.Id,
                TipoDiaId = t.TipoDia.Id,
                TipoHorarioId = t.TipoHorario.Id,
                TipoSegmentacionId = t.TipoSegmentacion.Id
            })
            .Distinct();

        var tarifasPasadas = from t in query
                             join m in from t in query.Where(x => x.InicioVigencia < now)
                                       where t.InicioVigencia < now &&
                                        !tarifasFuturas.Any(f =>
                                          f.MonedaId == t.Moneda.Id &&
                                          f.ServicioId == t.Servicio.Id &&
                                          f.TramoId == t.Tramo.Id &&
                                          f.GrupoEtarioId == t.GrupoEtario.Id &&
                                          f.CanalVentaId == t.CanalVenta.Id &&
                                          f.TipoDiaId == t.TipoDia.Id &&
                                          f.TipoHorarioId == t.TipoHorario.Id &&
                                          f.TipoSegmentacionId == t.TipoSegmentacion.Id)
                                       group t by new
                                       {
                                           MonedaId = t.Moneda.Id,
                                           ServicioId = t.Servicio.Id,
                                           TramoId = t.Tramo.Id,
                                           GrupoEtarioId = t.GrupoEtario.Id,
                                           CanalVentaId = t.CanalVenta.Id,
                                           TipoDiaId = t.TipoDia.Id,
                                           TipoHorarioId = t.TipoHorario.Id,
                                           TipoSegmentacionId = t.TipoSegmentacion.Id
                                       } into g
                                       select new
                                       {
                                           g.Key.MonedaId,
                                           g.Key.ServicioId,
                                           g.Key.TramoId,
                                           g.Key.GrupoEtarioId,
                                           g.Key.CanalVentaId,
                                           g.Key.TipoDiaId,
                                           g.Key.TipoHorarioId,
                                           g.Key.TipoSegmentacionId,
                                           InicioVigencia = g.Max(x => x.InicioVigencia)
                                       }
                             on new
                             {
                                 t.InicioVigencia,
                                 MonedaId = t.Moneda.Id,
                                 ServicioId = t.Servicio.Id,
                                 TramoId = t.Tramo.Id,
                                 GrupoEtarioId = t.GrupoEtario.Id,
                                 CanalVentaId = t.CanalVenta.Id,
                                 TipoDiaId = t.TipoDia.Id,
                                 TipoHorarioId = t.TipoHorario.Id,
                                 TipoSegmentacionId = t.TipoSegmentacion.Id,
                             }
                             equals new
                             {
                                 m.InicioVigencia,
                                 m.MonedaId,
                                 m.ServicioId,
                                 m.TramoId,
                                 m.GrupoEtarioId,
                                 m.CanalVentaId,
                                 m.TipoDiaId,
                                 m.TipoHorarioId,
                                 m.TipoSegmentacionId,
                             }
                             select t;

        query = query.Where(x => x.InicioVigencia >= now).Union(tarifasPasadas);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        List<TarifaServicio> tarifas = await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<TarifaServicio>()
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = tarifas,
        };
    }
}
