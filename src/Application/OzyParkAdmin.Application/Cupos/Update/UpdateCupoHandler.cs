using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cupos.Update;

/// <summary>
/// El manejador de <see cref="UpdateCupo"/>.
/// </summary>
public sealed class UpdateCupoHandler : CommandHandler<UpdateCupo, CupoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoManager _cupoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoManager">El <see cref="CupoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateCupoHandler(IOzyParkAdminContext context, CupoManager cupoManager, ILogger<UpdateCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoManager);
        _context = context;
        _cupoManager = cupoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CupoFullInfo>> ExecuteAsync(UpdateCupo command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        _context.AttachRange(command.CanalVenta, command.DiaSemana);

        ResultOf<Cupo> result = await _cupoManager.UpdateCupoAsync(
            command.Id,
            command.FechaEfectiva,
            command.EscenarioCupo,
            command.CanalVenta,
            command.DiaSemana,
            command.HoraInicio,
            command.HoraFin,
            command.Total,
            command.SobreCupo,
            command.TopeEnCupo,
            cancellationToken);

        return await result.BindAsync(
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
