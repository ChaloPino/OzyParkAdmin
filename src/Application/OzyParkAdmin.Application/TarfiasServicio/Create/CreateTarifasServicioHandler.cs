using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Application.TarfiasServicio.Create;

/// <summary>
/// El manejador de <see cref="CreateTarifasServicio"/>.
/// </summary>
public sealed class CreateTarifasServicioHandler : CommandHandler<CreateTarifasServicio>
{
    private readonly IOzyParkAdminContext _context;
    private readonly TarifaServicioManager _tarifaServicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateTarifasServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="tarifaServicioManager">El <see cref="TarifaServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateTarifasServicioHandler(IOzyParkAdminContext context, TarifaServicioManager tarifaServicioManager, ILogger<CreateTarifasServicioHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(tarifaServicioManager);
        _context = context;
        _tarifaServicioManager = tarifaServicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateTarifasServicio command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        _context.Attach(command.Moneda);
        _context.AttachRange(command.CanalesVenta);
        _context.AttachRange(command.TiposDia);
        _context.AttachRange(command.TiposHorario);
        _context.AttachRange(command.TiposSegmentacion);

        var result = await _tarifaServicioManager.CreateAsync(
            command.InicioVigencia,
            command.Moneda,
            command.Servicio,
            command.Tramos,
            command.GruposEtarios,
            command.CanalesVenta,
            command.TiposDia,
            command.TiposHorario,
            command.TiposSegmentacion,
            command.ValorAfecto,
            command.ValorExento,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<TarifaServicio> tarifas, CancellationToken cancellationToken)
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
