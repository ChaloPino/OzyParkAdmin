using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Find;

/// <summary>
/// El manejador de <see cref="FindProducto"/>.
/// </summary>
public sealed class FindProductoHandler : MediatorRequestHandler<FindProducto, ResultOf<ProductoFullInfo>>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    public FindProductoHandler(IProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ProductoFullInfo>> Handle(FindProducto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        Producto? producto = await _repository.FindByIdAsync(request.ProductoId, ProductoDetail.Cajas | ProductoDetail.Partes, cancellationToken);
        return producto is not null ? producto.ToFullInfo() : new NotFound();
    }
}
