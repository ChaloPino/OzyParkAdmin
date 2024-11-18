using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CategoriasProducto.Assign;

/// <summary>
/// El manejador de <see cref="AssignCanalesToCategoriaProducto"/>
/// </summary>
public sealed class AssignCanalesToCategoriaProductoHandler : CommandHandler<AssignCanalesToCategoriaProducto, CategoriaProductoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CategoriaProductoManager _categoriaProductoManager;


    /// <summary>
    /// Crea una instancia de <see cref="AssignCanalesToCategoriaProductoHandler" />
    /// </summary>
    /// <param name="categoriaProductoManager">El <see cref="CategoriaProductoManager"/></param>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/></param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignCanalesToCategoriaProductoHandler(CategoriaProductoManager categoriaProductoManager, IOzyParkAdminContext context, ILogger<AssignCanalesToCategoriaProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(categoriaProductoManager);

        _context = context;
        _categoriaProductoManager = categoriaProductoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CategoriaProductoFullInfo>> ExecuteAsync(AssignCanalesToCategoriaProducto command, CancellationToken cancellationToken)
    {
        var result = await _categoriaProductoManager.AssignCanalesVentaAsync(command.categoriaProductoId, command.CanalesVenta, cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure);
    }
    private async Task<ResultOf<CategoriaProductoFullInfo>> SaveAsync(CategoriaProducto categoriaProducto, CancellationToken cancellationToken)
    {
        _context.Update(categoriaProducto);
        await _context.SaveChangesAsync(cancellationToken);

        return categoriaProducto.ToFullInfo();
    }
}
