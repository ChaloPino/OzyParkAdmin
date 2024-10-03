using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Turno;

/// <summary>
/// El manejador de <see cref="CerrarTurno"/>.
/// </summary>
public sealed class CerrarTurnoHandler : MediatorRequestHandler<CerrarTurno, ResultOf<TurnoCajaInfo>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CajaManager _cajaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CerrarTurnoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cajaManager">El <see cref="CajaManager"/></param>
    public CerrarTurnoHandler(IOzyParkAdminContext context, CajaManager cajaManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cajaManager);
        _context = context;
        _cajaManager = cajaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<TurnoCajaInfo>> Handle(CerrarTurno request, CancellationToken cancellationToken)
    {
        var result = await _cajaManager.CerrarTurnoAsync(
            request.DiaId, request.Id, request.User.ToInfo(), request.RegularizacionEfectivo, request.RegularizacionMontoTransbank, request.Comentario, request.Movimientos, cancellationToken);

        return await result.MatchResultOfAsync(
            onSuccess: (apertura, token) => SaveAsync(apertura, request, token),
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
