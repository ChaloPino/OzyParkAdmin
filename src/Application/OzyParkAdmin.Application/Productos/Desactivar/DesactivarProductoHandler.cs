using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Desactivar;

/// <summary>
/// El manejador de <see cref="DesactivarProducto"/>.
/// </summary>
public sealed class DesactivarProductoHandler : ProductoStateChangeableHandler<DesactivarProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DesactivarProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    public DesactivarProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(DesactivarProducto request, CancellationToken cancellationToken) =>
        await _productoManager.DesactivarProductoAsync(request.ProductoId, cancellationToken);
}
