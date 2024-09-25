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
    public CreateProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        :base(context, StateAction.Create)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(CreateProducto request, CancellationToken cancellationToken)
    {
        Context.Attach(request.TipoProducto);

        if (request.Familia is not null)
        {
            Context.Attach(request.Familia);
        }

        return await _productoManager.CreateProductoAsync(
            request.Aka,
            request.Sku,
            request.Nombre,
            request.FranquiciaId,
            request.CentroCosto,
            request.Categoria,
            request.CategoriaDespliegue,
            request.Imagen,
            request.TipoProducto,
            request.Familia,
            request.Orden,
            request.EsComplemento,
            request.FechaAlta,
            request.UsuarioModificacion.ToInfo(),
            request.Complementos,
            cancellationToken);
    }
}
