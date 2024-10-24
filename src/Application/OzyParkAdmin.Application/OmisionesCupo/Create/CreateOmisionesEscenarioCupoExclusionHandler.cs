using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.OmisionesCupo.Create;

/// <summary>
/// El manejador de <see cref="CreateOmisionesEscenarioCupoExclusion"/>.
/// </summary>
public sealed class CreateOmisionesEscenarioCupoExclusionHandler : CommandHandler<CreateOmisionesEscenarioCupoExclusion>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IgnoraEscenarioCupoExclusionManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateOmisionesEscenarioCupoExclusionHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="IgnoraEscenarioCupoExclusionManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateOmisionesEscenarioCupoExclusionHandler(
        IOzyParkAdminContext context,
        IgnoraEscenarioCupoExclusionManager manager,
        ILogger<CreateOmisionesEscenarioCupoExclusionHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateOmisionesEscenarioCupoExclusion command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        _context.AttachRange(command.CanalesVenta);
        var result = await _manager.CreateAsync(command.EscenariosCupo, command.CanalesVenta, command.FechaDesde, command.FechaHasta, cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
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
