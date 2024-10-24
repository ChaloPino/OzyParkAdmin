using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cupos.Search;

/// <summary>
/// El manejador de <see cref="SearchCupos"/>.
/// </summary>
public sealed class SearchCuposHandler : QueryPagedOfHandler<SearchCupos, CupoFullInfo>
{
    private readonly ICupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCuposHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchCuposHandler(ICupoRepository repository, ILogger<SearchCuposHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<CupoFullInfo>> ExecutePagedListAsync(SearchCupos query, CancellationToken cancellationToken)
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
