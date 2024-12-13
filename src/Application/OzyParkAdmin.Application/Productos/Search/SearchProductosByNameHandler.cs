using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.Search;

/// <summary>
/// El manejador de <see cref="SearchProductosByName"/>.
/// </summary>
public sealed class SearchProductosByNameHandler : QueryListOfHandler<SearchProductosByName, ProductoInfo>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchProductosByNameHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchProductosByNameHandler(IProductoRepository repository, ILogger<SearchProductosByNameHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ProductoInfo>> ExecuteListAsync(SearchProductosByName query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _repository.SearchProductosAsync(query.CentroCostoId, query.SearchText, cancellationToken);
    }
}
