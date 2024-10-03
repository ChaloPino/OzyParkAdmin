using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.Cajas.Turno;

/// <summary>
/// El manejador de <see cref="ReabrirTurno"/>.
/// </summary>
public sealed class ReabrirTurnoHandler : MediatorRequestHandler<ReabrirTurno, ResultOf<TurnoCajaInfo>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CajaManager _cajaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReabrirTurnoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cajaManager">El <see cref="CajaManager"/>.</param>
    public ReabrirTurnoHandler(IOzyParkAdminContext context, CajaManager cajaManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cajaManager);
        _context = context;
        _cajaManager = cajaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<TurnoCajaInfo>> Handle(ReabrirTurno request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ResultOf<AperturaDia> result = await _cajaManager.ReabrirTurnoAsync(request.DiaId, request.TurnoId, cancellationToken);
        return await result.MatchResultOfAsync(
            onSuccess: (apertura, token) => SaveAsync(apertura, request, token),
            onFailure: failure => failure);
    }

    private async Task<ResultOf<TurnoCajaInfo>> SaveAsync(AperturaDia aperturaDia, ReabrirTurno request, CancellationToken cancellationToken)
    {
        _context.Update(aperturaDia);
        await _context.SaveChangesAsync(cancellationToken);
        return aperturaDia.ToTurnoInfo(request.TurnoId, request.Movimientos);
    }
}
