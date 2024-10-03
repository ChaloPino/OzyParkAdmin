using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cupos.Update;

/// <summary>
/// El manejador de <see cref="UpdateCupo"/>.
/// </summary>
public sealed class UpdateCupoHandler : MediatorRequestHandler<UpdateCupo, ResultOf<CupoFullInfo>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoManager _cupoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoManager">El <see cref="CupoManager"/>.</param>
    public UpdateCupoHandler(IOzyParkAdminContext context, CupoManager cupoManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoManager);
        _context = context;
        _cupoManager = cupoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CupoFullInfo>> Handle(UpdateCupo request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _context.AttachRange(request.CanalVenta, request.DiaSemana);

        ResultOf<Cupo> result = await _cupoManager.UpdateCupoAsync(
            request.Id,
            request.FechaEfectiva,
            request.EscenarioCupo,
            request.CanalVenta,
            request.DiaSemana,
            request.HoraInicio,
            request.HorFin,
            request.Total,
            request.SobreCupo,
            request.TopeEnCupo,
            cancellationToken);

        return await result.MatchResultOfAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<CupoFullInfo>> SaveAsync(Cupo cupo, CancellationToken cancellationToken)
    {
        _context.Update(cupo);
        await _context.SaveChangesAsync(cancellationToken);
        return cupo.ToFullInfo();
    }
}
