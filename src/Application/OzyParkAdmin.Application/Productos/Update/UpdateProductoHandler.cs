using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Update;

/// <summary>
/// El manejador de <see cref="UpdateProducto"/>.
/// </summary>
public sealed class UpdateProductoHandler : ProductoStateChangeableHandler<UpdateProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager, ILogger<UpdateProductoHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteChangeStateAsync(UpdateProducto command, CancellationToken cancellationToken)
    {
        Context.Attach(command.TipoProducto);
        Context.Attach(command.Familia);

        return await _productoManager.UpdateProductoAsync(
            command.Id,
            command.Aka,
            command.Sku,
            command.Nombre,
            command.CentroCosto,
            command.Categoria,
            command.CategoriaDespliegue,
            command.Imagen,
            command.TipoProducto,
            command.Familia,
            command.Orden,
            command.EsComplemento,
            command.FechaAlta,
            command.UsuarioModificacion.ToInfo(),
            command.Complementos,
            cancellationToken);
    }
}
