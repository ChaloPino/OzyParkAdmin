using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.OmisionesCupo.Delete;

/// <summary>
/// El manejador de <see cref="DeleteOmisionesEscenarioCupoExclusion"/>
/// </summary>
public sealed class DeleteOmisionesEscenarioCupoExclusionHandler : CommandHandler<DeleteOmisionesEscenarioCupoExclusion>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IIgnoraEscenarioCupoExclusionRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteOmisionesEscenarioCupoExclusionHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DeleteOmisionesEscenarioCupoExclusionHandler(
        IOzyParkAdminContext context,
        IIgnoraEscenarioCupoExclusionRepository repository,
        ILogger<DeleteOmisionesEscenarioCupoExclusionHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(DeleteOmisionesEscenarioCupoExclusion command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var keys = command.Omisiones.Select(x => (x.EscenarioCupo.Id, x.CanalVenta.Id, x.FechaIgnorada));
        var omisiones = await _repository.FindByKeysAsync(keys, cancellationToken);

        if (omisiones.Count() < 30)
        {
            _context.RemoveRange(omisiones);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkDeleteAsync(omisiones, cancellationToken);
        }

        return new Success();
    }
}
