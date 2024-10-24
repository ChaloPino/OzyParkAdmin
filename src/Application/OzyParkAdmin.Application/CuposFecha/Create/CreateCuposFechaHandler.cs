using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Create;

/// <summary>
/// El manejador de <see cref="CreateCuposFecha"/>
/// </summary>
public sealed class CreateCuposFechaHandler : CommandHandler<CreateCuposFecha>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoFechaManager _cupoFechaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateCuposFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoFechaManager">El <see cref="CupoFechaManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateCuposFechaHandler(IOzyParkAdminContext context, CupoFechaManager cupoFechaManager, ILogger<CreateCuposFechaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoFechaManager);
        _context = context;
        _cupoFechaManager = cupoFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateCuposFecha command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        _context.AttachRange(command.CanalesVenta);
        _context.AttachRange(command.DiasSemana);

        ResultOf<IEnumerable<CupoFecha>> result = await _cupoFechaManager.CreateCuposFechaAsync(
            command.FechaDesde,
            command.FechaHasta,
            command.EscenarioCupo,
            command.CanalesVenta,
            command.DiasSemana,
            command.HoraInicio,
            command.HoraTermino,
            command.IntervaloMinutos,
            command.Total,
            command.SobreCupo,
            command.TopeEnCupo,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<CupoFecha> cuposFecha, CancellationToken cancellationToken)
    {
        if (cuposFecha.Count() < 30)
        {
            await _context.AddRangeAsync(cuposFecha, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkInsertAsync(cuposFecha, cancellationToken);
        }

        return new Success();
    }
}
