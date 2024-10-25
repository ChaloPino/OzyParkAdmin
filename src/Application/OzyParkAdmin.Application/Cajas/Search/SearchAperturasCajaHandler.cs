using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Search;

/// <summary>
/// El manejador de <see cref="SearchAperturasCaja"/>.
/// </summary>
public sealed class SearchAperturasCajaHandler : QueryPagedOfHandler<SearchAperturasCaja, AperturaCajaInfo>
{
    private readonly ICajaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchAperturasCajaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICajaRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchAperturasCajaHandler(ICajaRepository repository, ILogger<SearchAperturasCajaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<AperturaCajaInfo>> ExecutePagedListAsync(SearchAperturasCaja query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchAperturaCajasAsync(
            query.CentroCostoId,
            query.SearchText,
            query.SearchDate,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
