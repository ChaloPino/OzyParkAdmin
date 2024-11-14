using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.CategoriasProducto.Create;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.CategoriasProducto.Update;

/// <summary>
/// El manejador de <see cref="UpdateCategoriaProductoHandler"/>
/// </summary>
public sealed class UpdateCategoriaProductoHandler : CommandHandler<UpdateCategoriaProducto, CategoriaProductoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CategoriaProductoManager _categoriaProductoManager;

    /// <summary>
    /// Modifica una instancia de <see cref="CreateCategoriaProducto"/>
    /// </summary>
    /// <param name="categoriaProductoManager">El <see cref="CategoriaProductoManager"/></param>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/></param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateCategoriaProductoHandler(CategoriaProductoManager categoriaProductoManager, IOzyParkAdminContext context, ILogger<CreateCategoriaProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(categoriaProductoManager);

        _context = context;
        _categoriaProductoManager = categoriaProductoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CategoriaProductoFullInfo>> ExecuteAsync(UpdateCategoriaProducto command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = await _categoriaProductoManager.UpdateAsync(
            command.Id,
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
            command.UsuarioModificacion.ToInfo(),
            command.UltimaModificacion,
            cancellationToken);

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
