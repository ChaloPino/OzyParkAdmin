using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.TarifasProducto;

/// <summary>
/// El repositorio de <see cref="TarifaProducto"/>.
/// </summary>
public interface ITarifaProductoRepository
{
    /// <summary>
    /// Consigue una tarifa de Producto por su clave primaria.
    /// </summary>
    /// <param name="inicioVigencia">El inicio de vigencia del tramo.</param>
    /// <param name="monedaId">El id de la moneda.</param>
    /// <param name="ProductoId">El id del Producto.</param>
    /// <param name="canalVentaId">El id del canal de venta.</param>
    /// <param name="tipoDiaId">El id del tipo de día.</param>
    /// <param name="tipoHorarioId">El id del tipo de horario.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La tarifa de Producto si existe.</returns>
    Task<TarifaProducto?> FindByPrimaryKeyAsync(DateTime inicioVigencia, int monedaId, int ProductoId, int canalVentaId, int tipoDiaId, int tipoHorarioId, CancellationToken cancellationToken);

    /// <summary>
    /// Consigue todas las tarifas que coincidan con las claves primarias.
    /// </summary>
    /// <param name="primaryKeys">Las claves primarias a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de tarifas que coinciden con las claves primarias.</returns>
    Task<IEnumerable<(DateTime InicioVigencia, Moneda Moneda, Producto Producto, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario)>> FindByPrimaryKeysAsync(List<(DateTime InicioVigencia, Moneda Moneda, Producto Producto, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario)> primaryKeys, CancellationToken cancellationToken);

    /// <summary>
    /// Busca tarifas de Producto que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centroCostoId">El id del centro de costo al que pertenecen los Productos.</param>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de tarifas de Producto.</returns>
    Task<PagedList<TarifaProducto>> SearchTarifasProductosAsync(int centroCostoId, string? searchText, FilterExpressionCollection<TarifaProducto> filterExpressions, SortExpressionCollection<TarifaProducto> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
