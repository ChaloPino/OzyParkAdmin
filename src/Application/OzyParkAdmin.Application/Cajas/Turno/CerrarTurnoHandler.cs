using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Turno;

/// <summary>
/// El manejador de <see cref="CerrarTurno"/>.
/// </summary>
public sealed class CerrarTurnoHandler : CommandHandler<CerrarTurno, TurnoCajaInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CajaManager _cajaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CerrarTurnoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cajaManager">El <see cref="CajaManager"/></param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CerrarTurnoHandler(IOzyParkAdminContext context, CajaManager cajaManager, ILogger<CerrarTurnoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cajaManager);
        _context = context;
        _cajaManager = cajaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<TurnoCajaInfo>> ExecuteAsync(CerrarTurno command, CancellationToken cancellationToken)
    {
        var result = await _cajaManager.CerrarTurnoAsync(
            command.DiaId,
            command.Id,
            command.User.ToInfo(),
            command.RegularizacionEfectivo, command.RegularizacionMontoTransbank,
            command.Comentario,
            command.Movimientos,
            cancellationToken);

        return await result.BindAsync(
            onSuccess: (apertura, token) => SaveAsync(apertura, command, token),
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<TurnoCajaInfo>> SaveAsync(AperturaDia aperturaDia, CerrarTurno request, CancellationToken cancellationToken)
    {
        _context.Update(aperturaDia);
        await _context.SaveChangesAsync(cancellationToken);
        return aperturaDia.ToTurnoInfo(request.Id, request.Movimientos);
    }
}
