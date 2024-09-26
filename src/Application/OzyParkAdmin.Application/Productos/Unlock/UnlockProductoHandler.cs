using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Unlock;

/// <summary>
/// El manejador de <see cref="UnlockProducto"/>.
/// </summary>
public sealed class UnlockProductoHandler : ProductoStateChangeableHandler<UnlockProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UnlockProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    public UnlockProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(UnlockProducto request, CancellationToken cancellationToken) =>
        await _productoManager.DesbloquearProductoAsync(request.ProductoId, cancellationToken);
}
