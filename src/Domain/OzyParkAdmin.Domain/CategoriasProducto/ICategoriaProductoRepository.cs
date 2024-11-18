
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.CategoriasProducto;

/// <summary>
/// El repositorio de <see cref="CategoriaProducto"/>.
/// </summary>
public interface ICategoriaProductoRepository
{
    /// <summary>
    /// Busca una categoría de producto por su id.
    /// </summary>
    /// <param name="id">El id de la categoría de producto a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La categoría de producto si existe.</returns>
    Task<CategoriaProducto?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todas las categorías de producto que pertenecen a la franquicia.
    /// </summary>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="tipoCategoria">El tipo de categoría a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <see cref="CategoriaProductoInfo"/>.</returns>
    Task<List<CategoriaProductoInfo>> ListByFranquiciaIdAsync(int franquiciaId, TipoCategoria tipoCategoria, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene el Valor de ID mayor en Categoria de Productos
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> MaxIdAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Busqueda de la Categorias de Productos
    /// </summary>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de <see cref="CategoriaProductoFullInfo"/> que coinciden con los criterios de búsqueda.</returns>
    Task<PagedList<CategoriaProductoFullInfo>> SearchCategoriaProductoAsync(string? searchText, FilterExpressionCollection<CategoriaProducto> filterExpressions, SortExpressionCollection<CategoriaProducto> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Revisa si es que existe un producto con el mismo aka.
    /// </summary>
    /// <param name="categoriaProductoId">El id de la categoria de producto que se está revisado.</param>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="aka">El aka a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns><c>true</c> si existe un producto con el mismo aka; en caso contrario, <c>false</c>.</returns>
    Task<bool> ExistAkaAsync(int categoriaProductoId, int franquiciaId, string? aka, CancellationToken cancellationToken);
}
