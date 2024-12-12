using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.DetallesEscenariosExclusiones.Search;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenariosExclusiones.Search;
public sealed class SearchEscenarioCupoExclusionHandler : QueryPagedOfHandler<SearchEscenarioCupoExclusion, DetalleEscenarioCupoExclusionFullInfo>
{
    private readonly IDetalleEscenarioCupoExclusionRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchEscenarioCupoExclusionHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IDetalleEscenarioCupoExclusionRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchEscenarioCupoExclusionHandler(IDetalleEscenarioCupoExclusionRepository repository, ILogger<SearchEscenarioCupoExclusionHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }
    /// <inheritdoc/>
    protected override async Task<PagedList<DetalleEscenarioCupoExclusionFullInfo>> ExecutePagedListAsync(SearchEscenarioCupoExclusion query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchAsync(
               query.ServiciosIds,
               query.CanalesDeVentaIds,
               query.DiasDeSemanaIds,
               query.EscenarioId,
               query.SearchText,
               query.FilterExpressions,
               query.SortExpressions,
               query.Page,
               query.PageSize,
               cancellationToken);
    }
}
