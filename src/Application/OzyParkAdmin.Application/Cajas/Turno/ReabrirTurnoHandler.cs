using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Turno;

/// <summary>
/// El manejador de <see cref="ReabrirTurno"/>.
/// </summary>
public sealed class ReabrirTurnoHandler : CommandHandler<ReabrirTurno, TurnoCajaInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CajaManager _cajaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReabrirTurnoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cajaManager">El <see cref="CajaManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ReabrirTurnoHandler(IOzyParkAdminContext context, CajaManager cajaManager, ILogger<ReabrirTurnoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cajaManager);
        _context = context;
        _cajaManager = cajaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<TurnoCajaInfo>> ExecuteAsync(ReabrirTurno command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ResultOf<AperturaDia> result = await _cajaManager.ReabrirTurnoAsync(command.DiaId, command.TurnoId, cancellationToken);
        return await result.BindAsync(
            onSuccess: (apertura, token) => SaveAsync(apertura, command, token),
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<TurnoCajaInfo>> SaveAsync(AperturaDia aperturaDia, ReabrirTurno command, CancellationToken cancellationToken)
    {
        _context.Update(aperturaDia);
        await _context.SaveChangesAsync(cancellationToken);
        return aperturaDia.ToTurnoInfo(command.TurnoId, command.Movimientos);
    }
}
