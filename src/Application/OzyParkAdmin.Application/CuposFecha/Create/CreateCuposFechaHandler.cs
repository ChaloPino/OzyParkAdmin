using MassTransit.Mediator;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Create;

/// <summary>
/// El manejador de <see cref="CreateCuposFecha"/>
/// </summary>
public sealed class CreateCuposFechaHandler : MediatorRequestHandler<CreateCuposFecha, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoFechaManager _cupoFechaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateCuposFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoFechaManager">El <see cref="CupoFechaManager"/>.</param>
    public CreateCuposFechaHandler(IOzyParkAdminContext context, CupoFechaManager cupoFechaManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoFechaManager);
        _context = context;
        _cupoFechaManager = cupoFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(CreateCuposFecha request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _context.AttachRange(request.CanalesVenta);
        _context.AttachRange(request.DiasSemana);

        ResultOf<IEnumerable<CupoFecha>> result = await _cupoFechaManager.CreateCuposFechaAsync(
            request.FechaDesde,
            request.FechaHasta,
            request.EscenarioCupo,
            request.CanalesVenta,
            request.DiasSemana,
            request.HoraInicio,
            request.HoraTermino,
            request.IntervaloMinutos,
            request.Total,
            request.SobreCupo,
            request.TopeEnCupo,
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
