using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Update;

/// <summary>
/// El manejador de <see cref="UpdateCuposFecha"/>
/// </summary>
public sealed class UpdateCuposFechaHandler : CommandHandler<UpdateCuposFecha>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoFechaManager _cupoFechaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateCupoFechaHandler"/>
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoFechaManager">El <see cref="CupoFechaManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateCuposFechaHandler(IOzyParkAdminContext context, CupoFechaManager cupoFechaManager, ILogger<UpdateCuposFechaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoFechaManager);
        _context = context;
        _cupoFechaManager = cupoFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(UpdateCuposFecha command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        _context.AttachRange(command.CanalVenta, command.DiaSemana);

        ResultOf<IEnumerable<CupoFecha>> result = await _cupoFechaManager.UpdateCuposFechaAsync(
            command.Fecha,
            command.EscenarioCupo,
            command.CanalVenta,
            command.DiaSemana,
            command.Total,
            command.Sobrecupo,
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
            _context.UpdateRange(cuposFecha);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkUpdateAsync(cuposFecha, cancellationToken);
        }

        return new Success();
    }
}
