using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Dia;

/// <summary>
/// El manejador de <see cref="ReabrirDia"/>.
/// </summary>
public sealed class ReabrirDiaHandler : MediatorRequestHandler<ReabrirDia, ResultOf<AperturaCajaInfo>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CajaManager _cajaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReabrirDiaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cajaManager">El <see cref="CajaManager"/>.</param>
    public ReabrirDiaHandler(IOzyParkAdminContext context, CajaManager cajaManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cajaManager);
        _context = context;
        _cajaManager = cajaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<AperturaCajaInfo>> Handle(ReabrirDia request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ResultOf<AperturaDia> result = await _cajaManager.ReabrirDiaAsync(request.DiaId, cancellationToken);
        return await result.MatchResultOfAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure);
    }

    private async Task<ResultOf<AperturaCajaInfo>> SaveAsync(AperturaDia aperturaDia, CancellationToken cancellationToken)
    {
        _context.Update(aperturaDia);
        await _context.SaveChangesAsync(cancellationToken);
        return aperturaDia.ToInfo();
    }
}
