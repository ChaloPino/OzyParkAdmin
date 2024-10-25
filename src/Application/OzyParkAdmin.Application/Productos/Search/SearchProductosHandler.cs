using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Search;

/// <summary>
/// El manejador de <see cref="SearchProductosHandler"/>.
/// </summary>
public sealed class SearchProductosHandler : QueryPagedOfHandler<SearchProductos, ProductoFullInfo>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchProductosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchProductosHandler(IProductoRepository repository, ILogger<SearchProductosHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<ProductoFullInfo>> ExecutePagedListAsync(SearchProductos query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchProductosAsync(
            query.User.GetCentrosCosto(),
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
