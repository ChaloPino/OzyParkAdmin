using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Assign;

/// <summary>
/// El manejador de <see cref="AssignCajasToProducto"/>.
/// </summary>
public sealed class AssignCajasToProductoHandler : ProductoStateChangeableHandler<AssignCajasToProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignCajasToProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    public AssignCajasToProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(AssignCajasToProducto request, CancellationToken cancellationToken) =>
        await _productoManager.AssignCajasAsync(request.ProductoId, request.Cajas, cancellationToken);
}
