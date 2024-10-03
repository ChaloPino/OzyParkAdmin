using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cupos.Create;

/// <summary>
/// El manejador de <see cref="CreateCupos"/>.
/// </summary>
public sealed class CreateCuposHandler : MediatorRequestHandler<CreateCupos, ResultOf<List<CupoFullInfo>>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoManager _cupoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateCuposHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoManager">El <see cref="CupoManager"/>.</param>
    public CreateCuposHandler(IOzyParkAdminContext context, CupoManager cupoManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoManager);
        _context = context;
        _cupoManager = cupoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<List<CupoFullInfo>>> Handle(CreateCupos request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _context.AttachRange(request.CanalesVenta);
        _context.AttachRange(request.DiasSemana);

        ResultOf<IEnumerable<Cupo>> result = await _cupoManager.CreateCuposAsync(
            request.FechaEfectiva,
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

        return await result.MatchResultOfAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<List<CupoFullInfo>>> SaveAsync(IEnumerable<Cupo> cupos, CancellationToken cancellationToken)
    {
        await _context.AddRangeAsync(cupos, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cupos.ToFullInfo();
    }
}
