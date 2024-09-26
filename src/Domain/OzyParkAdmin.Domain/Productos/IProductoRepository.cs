using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// El repositorio de <see cref="Producto"/>.
/// </summary>
public interface IProductoRepository
{
    /// <summary>
    /// Revisa si es que existe un producto con el mismo aka.
    /// </summary>
    /// <param name="productoId">El id del producto que se está revisado.</param>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="aka">El aka a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns><c>true</c> si existe un producto con el mismo aka; en caso contrario, <c>false</c>.</returns>
    Task<bool> ExistAkaAsync(int productoId, int franquiciaId, string? aka, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un producto por su aka.
    /// </summary>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="aka">El aka a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El producto si existe.</returns>
    Task<Producto?> FindByAkaAsync(int franquiciaId, string? aka, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un producto por su id.
    /// </summary>
    /// <param name="id">El id del producto a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El producto si existe.</returns>
    Task<Producto?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un producto por su id indicando el detalle que se quiere incluir.
    /// </summary>
    /// <param name="id">El id del producto a buscar.</param>
    /// <param name="incluirDetalle">El detalle del producto que se quiere incluir en la consulta.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El producto si existe.</returns>
    Task<Producto?> FindByIdAsync(int id, ProductoDetail incluirDetalle, CancellationToken cancellationToken);


    /// <summary>
    /// Busca los productos que coincidan con <paramref name="productoIds"/>.
    /// </summary>
    /// <param name="productoIds">Los ids de producto.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <see cref="Producto"/>.</returns>
    Task<IEnumerable<Producto>> FindByIdsAsync(int[] productoIds, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los productos que son complementos que pertenecen a una categoría de producto.
    /// </summary>
    /// <param name="categoriaId">El id de la categoría de producto.</param>
    /// <param name="exceptoProductoId">El id del producto que no se quiere que se muestre en la lista.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <see cref="ProductoInfo"/> que son complementos.</returns>
    /// <remarks>
    /// El <paramref name="exceptoProductoId"/> tiene que ir con un valor distinto a <c>0</c> cuando se está consultando específicamente para editar ese producto.
    /// </remarks>
    Task<List<ProductoInfo>> ListComplementosByCategoriaAsync(int categoriaId, int exceptoProductoId, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los productos que pueden ser partes de otro producto.
    /// </summary>
    /// <param name="franquiciaId">El id de la franquicia a la que pertenecen los productos.</param>
    /// <param name="exceptoProductoId">El id del producto que no se quiere que se muestre en la lista.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <see cref="ProductoInfo"/> que son complementos.</returns>
    /// <remarks>
    /// El <paramref name="exceptoProductoId"/> tiene que ir con un valor distinto a <c>0</c> cuando se está consultando específicamente para editar ese producto.
    /// </remarks>
    Task<List<ProductoInfo>> ListProductosParaPartesAsync(int franquiciaId, int exceptoProductoId, CancellationToken cancellationToken);

    /// <summary>
    /// Devuelve el id máximo.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El id máximo.</returns>
    Task<int> MaxIdAsync(CancellationToken cancellationToken);

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
