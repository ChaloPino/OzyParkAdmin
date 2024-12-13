using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Application.TarfiasProducto.Update;

/// <summary>
/// El manejador de <see cref="UpdateTarifaProducto"/>
/// </summary>
public sealed class UpdateTarifaProductoHandler : CommandHandler<UpdateTarifaProducto, TarifaProductoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly TarifaProductoManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="IOzyParkAdminContext"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="TarifaProductoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateTarifaProductoHandler(IOzyParkAdminContext context, TarifaProductoManager manager, ILogger<UpdateTarifaProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<TarifaProductoFullInfo>> ExecuteAsync(UpdateTarifaProducto command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        _context.AttachRange(
            command.Moneda,
            command.CanalVenta,
            command.TipoDia,
            command.TipoHorario);

        var result = await _manager.UpdateAsync(
            command.InicioVigencia,
            command.Moneda,
            command.Producto,
            command.CanalVenta,
            command.TipoDia,
            command.TipoHorario,
            command.ValorAfecto,
            command.ValorExento,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<TarifaProductoFullInfo>> SaveAsync(TarifaProducto tarifaProducto, CancellationToken cancellationToken)
    {
        _context.Update(tarifaProducto);
        await _context.SaveChangesAsync(cancellationToken);
        return tarifaProducto.ToFullInfo();
    }
}
