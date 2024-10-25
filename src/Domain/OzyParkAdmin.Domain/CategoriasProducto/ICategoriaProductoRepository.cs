
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
}
