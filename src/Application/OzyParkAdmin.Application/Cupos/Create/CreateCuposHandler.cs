using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cupos.Create;

/// <summary>
/// El manejador de <see cref="CreateCupos"/>.
/// </summary>
public sealed class CreateCuposHandler : CommandHandler<CreateCupos, List<CupoFullInfo>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoManager _cupoManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateCuposHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoManager">El <see cref="CupoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateCuposHandler(IOzyParkAdminContext context, CupoManager cupoManager, ILogger<CreateCuposHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoManager);
        _context = context;
        _cupoManager = cupoManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<List<CupoFullInfo>>> ExecuteAsync(CreateCupos command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        _context.AttachRange(command.CanalesVenta);
        _context.AttachRange(command.DiasSemana);

        ResultOf<IEnumerable<Cupo>> result = await _cupoManager.CreateCuposAsync(
            command.FechaEfectiva,
            command.EscenarioCupo,
            command.CanalesVenta,
            command.DiasSemana,
            command.HoraInicio,
            command.HoraTermino,
            command.IntervaloMinutos,
            command.Total,
            command.SobreCupo,
            command.TopeEnCupo,
            cancellationToken);

        return await result.BindAsync(
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
