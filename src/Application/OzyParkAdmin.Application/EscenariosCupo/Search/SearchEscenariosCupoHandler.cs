using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.EscenariosCupo.Search;

/// <summary>
/// El manejador de <see cref="SearchEscenariosCupo"/>
/// </summary>
public sealed class SearchEscenariosCupoHandler : QueryPagedOfHandler<SearchEscenariosCupo, EscenarioCupoFullInfo>
{
    private readonly IEscenarioCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchEscenariosCupoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IEscenarioCupoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchEscenariosCupoHandler(IEscenarioCupoRepository repository, ILogger<SearchEscenariosCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<EscenarioCupoFullInfo>> ExecutePagedListAsync(SearchEscenariosCupo query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchAsync(
               query.User.GetCentrosCosto(),
               query.ZonasIds,
               query.SearchText,
               query.FilterExpressions,
               query.SortExpressions,
               query.Page,
               query.PageSize,
               cancellationToken);
    }
}
