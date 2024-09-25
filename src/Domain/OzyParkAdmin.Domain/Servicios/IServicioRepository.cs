using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El repositorio de <see cref="Servicio"/>.
/// </summary>
public interface IServicioRepository
{
    /// <summary>
    /// Busca un servicio que coincida con la <paramref name="franquiciaId"/> y el <paramref name="aka"/>.
    /// </summary>
    /// <param name="franquiciaId">El id de franquicia.</param>
    /// <param name="aka">El aka del servicio.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El servicio si existe.</returns>
    Task<Servicio?> FindByAkaAsync(int franquiciaId, string? aka, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un servicio dado el <paramref name="servicioId"/>.
    /// </summary>
    /// <param name="servicioId">El id del servicio a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El servicio si existe.</returns>
    Task<Servicio?> FindByIdAsync(int servicioId, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los servicios que petenezcan a la <paramref name="franquiciaId"/>.
    /// </summary>
    /// <param name="franquiciaId">El id de franquicia a la que pertenecen los servicios que se buscan.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de servicios que pertenecen a la franquicia del servicio.</returns>
    Task<List<ServicioInfo>> ListAsync(int franquiciaId, CancellationToken cancellationToken);

    /// <summary>
    /// Devuelve el id máximo.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El id máximo.</returns>
    Task<int> MaxIdAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Busca servicios que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centrosCostoId">Lista de ids de centros de costo que acota el universo de servicios.</param>
    /// <param name="searchText">El texto a buscar en el aka y el nombre del servicio.</param>
    /// <param name="filterExpressions">Las expresiones de filtro.</param>
    /// <param name="sortExpressions">La expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Una lista paginada de servicios.</returns>
    Task<PagedList<ServicioFullInfo>> SearchServiciosAsync(int[]? centrosCostoId, string? searchText, FilterExpressionCollection<Servicio> filterExpressions, SortExpressionCollection<Servicio> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
