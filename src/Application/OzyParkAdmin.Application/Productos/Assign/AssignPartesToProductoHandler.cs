using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Assign;

/// <summary>
/// El manejador de <see cref="AssignPartesToProducto"/>
/// </summary>
public sealed class AssignPartesToProductoHandler : ProductoStateChangeableHandler<AssignPartesToProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignPartesToProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    public AssignPartesToProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(AssignPartesToProducto request, CancellationToken cancellationToken) =>
        await _productoManager.AssignPartesAsync(request.ProductoId, request.Partes, cancellationToken);
}
