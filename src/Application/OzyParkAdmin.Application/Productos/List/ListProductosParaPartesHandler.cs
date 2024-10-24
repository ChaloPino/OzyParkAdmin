using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// El manejador de <see cref="ListProductosParaPartes"/>.
/// </summary>
public sealed class ListProductosParaPartesHandler : QueryListOfHandler<ListProductosParaPartes, ProductoInfo>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListProductosParaPartesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListProductosParaPartesHandler(IProductoRepository repository, ILogger<ListProductosParaPartesHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ProductoInfo>> ExecuteListAsync(ListProductosParaPartes query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListProductosParaPartesAsync(query.FranquiciaId, query.ExceptoProductoId, cancellationToken);
    }
}
