using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Search;

/// <summary>
/// El manejador de <see cref="SearchCuposFecha"/>.
/// </summary>
public sealed class SearchCuposFechaHandler : QueryPagedOfHandler<SearchCuposFecha, CupoFechaFullInfo>
{
    private readonly ICupoFechaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCuposFechaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoFechaRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchCuposFechaHandler(ICupoFechaRepository repository, ILogger<SearchCuposFechaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<CupoFechaFullInfo>> ExecutePagedListAsync(SearchCuposFecha query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchAsync(
            query.User.GetCentrosCosto(),
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
