using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Search;

/// <summary>
/// El manejador de <see cref="SearchServicios"/>.
/// </summary>
public sealed class SearchServiciosHandler : QueryPagedOfHandler<SearchServicios, ServicioFullInfo>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchServiciosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchServiciosHandler(IServicioRepository repository, ILogger<SearchServiciosHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<ServicioFullInfo>> ExecutePagedListAsync(SearchServicios query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchServiciosAsync(
            query.User.GetCentrosCosto(),
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
