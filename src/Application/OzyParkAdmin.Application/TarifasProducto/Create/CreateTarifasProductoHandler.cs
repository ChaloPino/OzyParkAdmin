using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Application.TarfiasProducto.Create;

/// <summary>
/// El manejador de <see cref="CreateTarifasProducto"/>.
/// </summary>
public sealed class CreateTarifasProductoHandler : CommandHandler<CreateTarifasProducto>
{
    private readonly IOzyParkAdminContext _context;
    private readonly TarifaProductoManager _tarifaProductoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateTarifasProductoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="tarifaProductoManager">El <see cref="TarifaProductoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateTarifasProductoHandler(IOzyParkAdminContext context, TarifaProductoManager tarifaProductoManager, ILogger<CreateTarifasProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tarifaProductoManager);
        _context = context;
        _tarifaProductoManager = tarifaProductoManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateTarifasProducto command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        _context.Attach(command.Moneda);
        _context.AttachRange(command.CanalesVenta);
        _context.AttachRange(command.TiposDia);
        _context.AttachRange(command.TiposHorario);

        var result = await _tarifaProductoManager.CreateAsync(
            command.InicioVigencia,
            command.Moneda,
            command.Producto,
            command.CanalesVenta,
            command.TiposDia,
            command.TiposHorario,
            command.ValorAfecto,
            command.ValorExento,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<TarifaProducto> tarifas, CancellationToken cancellationToken)
    {
        if (tarifas.Count() < 30)
        {
            await _context.AddRangeAsync(tarifas, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new Success();
        }

        await _context.BulkInsertAsync(tarifas, cancellationToken);
        return new Success();
    }
}
