using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// El repositorio de <see cref="Producto"/>.
/// </summary>
public interface IProductoRepository
{
    /// <summary>
    /// Busca productos que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de <see cref="ProductoFullInfo"/> que coinciden con los criterios de búsqueda.</returns>
    Task<PagedList<ProductoFullInfo>> SearchProductosAsync(int[]? centroCostoIds, string? searchText, FilterExpressionCollection<Producto> filterExpressions, SortExpressionCollection<Producto> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
    
}
