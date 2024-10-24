using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.OmisionesCupo.Search;

/// <summary>
/// El manejador de <see cref="SearchOmisionesEscenarioCupoExlusion"/>.
/// </summary>
public sealed class SearchOmisionesEscenarioCupoExlusionHandler : QueryPagedOfHandler<SearchOmisionesEscenarioCupoExlusion, IgnoraEscenarioCupoExclusionFullInfo>
{
    private readonly IIgnoraEscenarioCupoExclusionRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchOmisionesEscenarioCupoExlusionHandler(IIgnoraEscenarioCupoExclusionRepository repository, ILogger<SearchOmisionesEscenarioCupoExlusionHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<IgnoraEscenarioCupoExclusionFullInfo>> ExecutePagedListAsync(SearchOmisionesEscenarioCupoExlusion query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _repository.SearchAsync(
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
