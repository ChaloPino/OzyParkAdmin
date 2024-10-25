using MassTransit.Mediator;
using OzyParkAdmin.Application.Productos.Create;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;


namespace OzyParkAdmin.Application.CategoriasProducto.Create;

/// <summary>
/// El manejador de <see cref="CreateCategoriaProducto"/>
/// </summary>
//public sealed class CategoriaProductoHandle: MediatorRequestHandler<CategoriaProducto, SuccessOrFailure>
public class CreateCategoriaProductoHandle : MediatorRequestHandler<CreateCategoriaProducto, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CategoriaProductoManager _categoriaProductoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateCategoriaProductoHandle"/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="categoriaProductoManager"></param>
    public CreateCategoriaProductoHandle(IOzyParkAdminContext context, CategoriaProductoManager categoriaProductoManager)
    {
        _context = context;
        _categoriaProductoManager = categoriaProductoManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(CreateCategoriaProducto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await _categoriaProductoManager.CreateAsync(
            request.FranquiciaId,
            request.Aka,
            request.Nombre,
            request.PadreInfo,
            request.EsFinal,
            request.ImagenInfo,
            request.Orden,
            request.EsTop,
            request.Nivel,
            request.PrimeroProductos,
            request.UsuarioCreacionInfo,
            request.FechaCreacion,
            request.UsuarioModificacionInfo,
            request.UltimaModificacion, 
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure);
    }


    private async Task<SuccessOrFailure> SaveAsync(CategoriaProducto categoriaProducto, CancellationToken cancellationToken)
    {
        await _context.AddAsync(categoriaProducto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
