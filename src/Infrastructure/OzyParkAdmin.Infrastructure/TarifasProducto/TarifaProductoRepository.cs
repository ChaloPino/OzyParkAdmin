using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.TarifasProducto;

/// <summary>
/// El repositorio de <see cref="TarifaProducto"/>.
/// </summary>
public sealed class TarifaProductoRepository(OzyParkAdminContext context) : Repository<TarifaProducto>(context), ITarifaProductoRepository
{
    /// <inheritdoc/>
    public async Task<TarifaProducto?> FindByPrimaryKeyAsync(DateTime inicioVigencia, int monedaId, int ProductoId, int canalVentaId, int tipoDiaId, int tipoHorarioId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .IgnoreAutoIncludes()
            .Include(x => x.Moneda)
            .Include(x => x.Producto)
            .Include(x => x.CanalVenta)
            .Include(x => x.TipoDia)
            .Include(x => x.TipoHorario)
            .Where(x => x.InicioVigencia == inicioVigencia &&
                        x.MonedaId == monedaId &&
                        x.ProductoId == ProductoId &&
                        x.CanalVentaId == canalVentaId &&
                        x.TipoDiaId == tipoDiaId &&
                        x.TipoHorarioId == tipoHorarioId)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<(DateTime InicioVigencia, Moneda Moneda, Producto Producto, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario)>> FindByPrimaryKeysAsync(List<(DateTime InicioVigencia, Moneda Moneda, Producto Producto, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario)> primaryKeys, CancellationToken cancellationToken)
    {
        DateTime[] iniciosVigencia = primaryKeys.Select(x => x.InicioVigencia).ToArray();
        int[] monedasId = primaryKeys.Select(x => x.Moneda.Id).ToArray();
        int[] ProductosId = primaryKeys.Select(x => x.Producto.Id).ToArray();
        int[] canalesVentaId = primaryKeys.Select(x => x.CanalVenta.Id).ToArray();
        int[] tiposDiaId = primaryKeys.Select(x => x.TipoDia.Id).ToArray();
        int[] tiposHorarioId = primaryKeys.Select(x => x.TipoHorario.Id).ToArray();
        var tarifas = await EntitySet.Where(x =>
            Enumerable.Contains(iniciosVigencia, x.InicioVigencia) &&
            Enumerable.Contains(monedasId, x.Moneda.Id) &&
            Enumerable.Contains(ProductosId, x.Producto.Id) &&
            Enumerable.Contains(canalesVentaId, x.CanalVenta.Id) &&
            Enumerable.Contains(tiposDiaId, x.TipoDia.Id) &&
            Enumerable.Contains(tiposHorarioId, x.TipoHorario.Id))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return tarifas.ConvertAll(x => (x.InicioVigencia, x.Moneda, x.Producto, x.CanalVenta, x.TipoDia, x.TipoHorario));
    }

    /// <inheritdoc/>
    public async Task<PagedList<TarifaProducto>> SearchTarifasProductosAsync(int centroCostoId, string? searchText, FilterExpressionCollection<TarifaProducto> filterExpressions, SortExpressionCollection<TarifaProducto> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        DateTime now = DateTime.Now;

        IQueryable<TarifaProducto> query = EntitySet.IgnoreAutoIncludes().AsNoTracking().AsSplitQuery().Where(x => x.Producto.CentroCosto.Id == centroCostoId);

        if (searchText is not null)
        {
            query = query.Where(x =>
                x.Moneda.Nombre.Contains(searchText) ||
                x.Producto.Nombre.Contains(searchText) ||
                x.TipoDia.Descripcion.Contains(searchText) ||
                x.TipoHorario.Descripcion.Contains(searchText) ||
                x.CanalVenta.Aka.Contains(searchText));
        }

        query = query.Include(x => x.Moneda)
            .Include(x => x.Producto)
            .Include(x => x.TipoDia)
            .Include(x => x.TipoHorario)
            .Include(x => x.CanalVenta);

        query = filterExpressions.Where(query);

        var tarifasFuturas = query
            .Where(x => x.InicioVigencia >= now)
            .Select(t => new
            {
                MonedaId = t.Moneda.Id,
                ProductoId = t.Producto.Id,
                CanalVentaId = t.CanalVenta.Id,
                TipoDiaId = t.TipoDia.Id,
                TipoHorarioId = t.TipoHorario.Id,
            })
            .Distinct();

        var tarifasPasadas = from t in query
                             join m in from t in query.Where(x => x.InicioVigencia < now)
                                       where t.InicioVigencia < now &&
                                        !tarifasFuturas.Any(f =>
                                          f.MonedaId == t.Moneda.Id &&
                                          f.ProductoId == t.Producto.Id &&
                                          f.CanalVentaId == t.CanalVenta.Id &&
                                          f.TipoDiaId == t.TipoDia.Id &&
                                          f.TipoHorarioId == t.TipoHorario.Id)
                                       group t by new
                                       {
                                           MonedaId = t.Moneda.Id,
                                           ProductoId = t.Producto.Id,
                                           CanalVentaId = t.CanalVenta.Id,
                                           TipoDiaId = t.TipoDia.Id,
                                           TipoHorarioId = t.TipoHorario.Id
                                       } into g
                                       select new
                                       {
                                           g.Key.MonedaId,
                                           g.Key.ProductoId,
                                           g.Key.CanalVentaId,
                                           g.Key.TipoDiaId,
                                           g.Key.TipoHorarioId,
                                           InicioVigencia = g.Max(x => x.InicioVigencia)
                                       }
                             on new
                             {
                                 t.InicioVigencia,
                                 MonedaId = t.Moneda.Id,
                                 ProductoId = t.Producto.Id,
                                 CanalVentaId = t.CanalVenta.Id,
                                 TipoDiaId = t.TipoDia.Id,
                                 TipoHorarioId = t.TipoHorario.Id,
                             }
                             equals new
                             {
                                 m.InicioVigencia,
                                 m.MonedaId,
                                 m.ProductoId,
                                 m.CanalVentaId,
                                 m.TipoDiaId,
                                 m.TipoHorarioId,
                             }
                             select t;

        query = query.Where(x => x.InicioVigencia >= now).Union(tarifasPasadas);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        List<TarifaProducto> tarifas = await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<TarifaProducto>()
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = tarifas,
        };
    }
}
