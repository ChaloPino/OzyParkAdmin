using Microsoft.Extensions.Logging;
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
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UnlockProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager, ILogger<UnlockProductoHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteChangeStateAsync(UnlockProducto command, CancellationToken cancellationToken) =>
        await _productoManager.DesbloquearProductoAsync(command.ProductoId, cancellationToken);
}
