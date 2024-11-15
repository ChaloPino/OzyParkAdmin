using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;


namespace OzyParkAdmin.Application.CategoriasProducto.Create;

/// <summary>
/// El manejador de <see cref="CreateCategoriaProducto"/>
/// </summary>
public sealed class CreateCategoriaProductoHandler : CommandHandler<CreateCategoriaProducto, CategoriaProductoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CategoriaProductoManager _categoriaProductoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateCategoriaProducto"/>
    /// </summary>
    /// <param name="categoriaProductoManager">El <see cref="CategoriaProductoManager"/></param>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/></param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateCategoriaProductoHandler(CategoriaProductoManager categoriaProductoManager, IOzyParkAdminContext context, ILogger<CreateCategoriaProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(categoriaProductoManager);

        _context = context;
        _categoriaProductoManager = categoriaProductoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CategoriaProductoFullInfo>> ExecuteAsync(CreateCategoriaProducto command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = await _categoriaProductoManager.CreateAsync(
            command.FranquiciaId,
            command.Aka,
            command.Nombre,
            command.PadreInfo,
            command.EsFinal,
            command.ImagenInfo,
            command.Orden,
            command.EsTop,
            command.Nivel,
            command.PrimeroProductos,
            command.UsuarioCreacion.ToInfo(),
            command.FechaCreacion,
            command.UsuarioModificacion.ToInfo(),
            command.UltimaModificacion, 
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure);
    }


    private async Task<ResultOf<CategoriaProductoFullInfo>> SaveAsync(CategoriaProducto categoriaProducto, CancellationToken cancellationToken)
    {
        await _context.AddAsync(categoriaProducto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return categoriaProducto.ToFullInfo();
    }
}
