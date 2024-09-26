using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Activar;

/// <summary>
/// El manejador de <see cref="ActivarProducto"/>
/// </summary>
public sealed class ActivarProductoHandler : ProductoStateChangeableHandler<ActivarProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ActivarProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    public ActivarProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(ActivarProducto request, CancellationToken cancellationToken) =>
        await _productoManager.ActivarProductoAsync(request.ProductoId, cancellationToken);
}
