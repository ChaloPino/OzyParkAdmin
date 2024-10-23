using MassTransit.Mediator;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.OmisionesCupo.Create;

/// <summary>
/// El manejador de <see cref="CreateOmisionesEscenarioCupoExclusion"/>.
/// </summary>
public sealed class CreateOmisionesEscenarioCupoExclusionHandler : MediatorRequestHandler<CreateOmisionesEscenarioCupoExclusion, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IgnoraEscenarioCupoExclusionManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateOmisionesEscenarioCupoExclusionHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="IgnoraEscenarioCupoExclusionManager"/>.</param>
    public CreateOmisionesEscenarioCupoExclusionHandler(IOzyParkAdminContext context, IgnoraEscenarioCupoExclusionManager manager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(CreateOmisionesEscenarioCupoExclusion request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _context.AttachRange(request.CanalesVenta);
        var result = await _manager.CreateAsync(request.EscenariosCupo, request.CanalesVenta, request.FechaDesde, request.FechaHasta, cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<IgnoraEscenarioCupoExclusion> omisiones, CancellationToken cancellationToken)
    {
        if (omisiones.Count() < 30)
        {
            await _context.AddRangeAsync(omisiones, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkInsertAsync(omisiones, cancellationToken);
        }

        return new Success();
    }
}
