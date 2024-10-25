using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Create;

/// <summary>
/// El manejador de <see cref="CreateProducto"/>.
/// </summary>
public sealed class CreateProductoHandler : ProductoStateChangeableHandler<CreateProducto>
{
    private readonly ProductoManager _productoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="productoManager">El <see cref="ProductoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager, ILogger<CreateProductoHandler> logger)
        :base(context, logger, StateAction.Create)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteChangeStateAsync(CreateProducto command, CancellationToken cancellationToken)
    {
        Context.Attach(command.TipoProducto);

        if (command.Familia is not null)
        {
            Context.Attach(command.Familia);
        }

        return await _productoManager.CreateProductoAsync(
            command.Aka,
            command.Sku,
            command.Nombre,
            command.FranquiciaId,
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
