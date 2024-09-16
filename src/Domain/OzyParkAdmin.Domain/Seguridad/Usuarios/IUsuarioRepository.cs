using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Representa el repositorio de <see cref="Usuario"/>.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Busca usuarios que coincidan con el criterio de búsqueda,
    /// y devuelve una lista paginada.
    /// </summary>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="centrosCosto">Una lista de centros de costos.</param>
    /// <param name="roles">Una lista de roles.</param>
    /// <param name="filterExpressions"´>Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">La expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El listado de usuarios paginado que coinciden con el criterio de búsqueda.</returns>
    Task<PagedList<UsuarioInfo>> BuscarUsuariosAsync(string? searchText, int[]? centrosCosto, string[]? roles, FilterExpressionCollection<Usuario> filterExpressions, SortExpressionCollection<Usuario> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
