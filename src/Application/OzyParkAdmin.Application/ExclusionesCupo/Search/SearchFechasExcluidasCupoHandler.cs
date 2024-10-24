using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Search;

/// <summary>
/// El manejador de <see cref="SearchFechasExcluidasCupo"/>
/// </summary>
public sealed class SearchFechasExcluidasCupoHandler : QueryPagedOfHandler<SearchFechasExcluidasCupo, FechaExcluidaCupoFullInfo>
{
    private readonly IFechaExcluidaCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchFechasExcluidasCupoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IFechaExcluidaCupoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchFechasExcluidasCupoHandler(IFechaExcluidaCupoRepository repository, ILogger<SearchFechasExcluidasCupoHandler> logger)
        : base(logger)  
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<FechaExcluidaCupoFullInfo>> ExecutePagedListAsync(SearchFechasExcluidasCupo query, CancellationToken cancellationToken)
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
