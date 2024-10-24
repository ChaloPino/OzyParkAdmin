using Microsoft.Extensions.Logging;
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
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ActivarProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager, ILogger<ActivarProductoHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteChangeStateAsync(ActivarProducto command, CancellationToken cancellationToken) =>
        await _productoManager.ActivarProductoAsync(command.ProductoId, cancellationToken);
}
