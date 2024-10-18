using MassTransit.Mediator;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Create;

/// <summary>
/// El manejador de <see cref="CreateFechasExcluidasCupo"/>.
/// </summary>
public sealed class CreateFechasExcluidasCupoHandler : MediatorRequestHandler<CreateFechasExcluidasCupo, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly FechaExcluidaCupoManager _fechaExcluidaCupoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateFechasExcluidasCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="fechaExcluidaCupoManager">El <see cref="FechaExcluidaCupoManager"/>.</param>
    public CreateFechasExcluidasCupoHandler(IOzyParkAdminContext context, FechaExcluidaCupoManager fechaExcluidaCupoManager)
    {
        _context = context;
        _fechaExcluidaCupoManager = fechaExcluidaCupoManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(CreateFechasExcluidasCupo request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _context.AttachRange(request.CanalesVenta);

        var result = await _fechaExcluidaCupoManager.CreateFechasExcluidasAsync(
            request.CentroCosto,
            request.CanalesVenta,
            request.FechaDesde,
            request.FechaHasta,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<FechaExcluidaCupo> fechaExcluidaCupos, CancellationToken cancellationToken)
    {
        if (fechaExcluidaCupos.Count() < 30)
        {
            await _context.AddRangeAsync(fechaExcluidaCupos, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkInsertAsync(fechaExcluidaCupos, cancellationToken);
        }

        return new Success();
    }

}
