using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Search;

/// <summary>
/// El manejador de <see cref="SearchProductosHandler"/>.
/// </summary>
public sealed class SearchProductosHandler : MediatorRequestHandler<SearchProductos, PagedList<ProductoFullInfo>>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchProductosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    public SearchProductosHandler(IProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<ProductoFullInfo>> Handle(SearchProductos request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.SearchProductosAsync(request.User.GetCentrosCosto(), request.SearchText, request.FilterExpressions, request.SortExpressions, request.Page, request.PageSize, cancellationToken);
    }
}
