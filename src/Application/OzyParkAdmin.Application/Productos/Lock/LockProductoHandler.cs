using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Lock;

/// <summary>
/// El manejador de <see cref="LockProducto"/>.
/// </summary>
public sealed class LockProductoHandler : ProductoStateChangeableHandler<LockProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="LockProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public LockProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager, ILogger<LockProductoHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteChangeStateAsync(LockProducto command, CancellationToken cancellationToken) =>
        await _productoManager.BloquearProductoAsync(command.ProductoId, cancellationToken);
}
