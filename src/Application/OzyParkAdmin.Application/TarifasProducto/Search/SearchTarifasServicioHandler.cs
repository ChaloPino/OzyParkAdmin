using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Application.TarfiasProducto.Search;

/// <summary>
/// El manejador de <see cref="SearchTarifasProducto"/>.
/// </summary>
public sealed class SearchTarifasProductoHandler : QueryPagedOfHandler<SearchTarifasProducto, TarifaProductoFullInfo>
{
    private readonly ITarifaProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchTarifasProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ITarifaProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchTarifasProductoHandler(
        ITarifaProductoRepository repository,
        ILogger<SearchTarifasProductoHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<TarifaProductoFullInfo>> ExecutePagedListAsync(SearchTarifasProducto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        var tarifas = await _repository.SearchTarifasProductosAsync(
            query.CentroCostoId,
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);

        return new PagedList<TarifaProductoFullInfo>
        {
            CurrentPage = tarifas.CurrentPage,
            PageSize = tarifas.PageSize,
            TotalCount = tarifas.TotalCount,
            Items = tarifas.Items.ToFullInfo(),
        };
    }
}
