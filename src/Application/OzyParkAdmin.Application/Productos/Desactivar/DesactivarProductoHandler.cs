using Microsoft.Extensions.Logging;
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
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DesactivarProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager, ILogger<DesactivarProductoHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteChangeStateAsync(DesactivarProducto command, CancellationToken cancellationToken) =>
        await _productoManager.DesactivarProductoAsync(command.ProductoId, cancellationToken);
}
