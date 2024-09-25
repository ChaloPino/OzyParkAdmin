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
    public UpdateProductoHandler(IOzyParkAdminContext context, ProductoManager productoManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(productoManager);
        _productoManager = productoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Producto>> ExecuteAsync(UpdateProducto request, CancellationToken cancellationToken)
    {
        Context.Attach(request.TipoProducto);

        if (request.Familia is not null)
        {
            Context.Attach(request.Familia);
        }

        return await _productoManager.UpdateProductoAsync(
            request.Id,
            request.Aka,
            request.Sku,
            request.Nombre,
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
