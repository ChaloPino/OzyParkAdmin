using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.Find;

/// <summary>
/// El manejador de <see cref="FindProducto"/>.
/// </summary>
public sealed class FindProductoHandler : QueryHandler<FindProducto, ProductoFullInfo>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public FindProductoHandler(IProductoRepository repository, ILogger<FindProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ProductoFullInfo?> ExecuteQueryAsync(FindProducto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        Producto? producto = await _repository.FindByIdAsync(query.ProductoId, ProductoDetail.Cajas | ProductoDetail.Partes, cancellationToken);
        return producto?.ToFullInfo();
    }
}
