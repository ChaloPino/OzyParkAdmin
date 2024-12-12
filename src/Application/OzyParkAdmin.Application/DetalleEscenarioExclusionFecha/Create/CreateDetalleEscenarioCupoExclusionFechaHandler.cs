using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.Create;

/// <summary>
/// Handler encargado de la creación de exclusiones por fecha de un escenario de cupo.
/// </summary>
public sealed class CreateDetalleEscenarioCupoExclusionFechaHandler : CommandHandler<CreateDetalleEscenarioCupoExclusionFecha>
{
    private readonly IOzyParkAdminContext _context;
    private readonly DetalleEscenarioCupoExclusionFechaManager _exclusionFechaManager;

    /// <summary>
    /// Constructor de la clase <see cref="CreateDetalleEscenarioCupoExclusionFechaHandler"/>.
    /// </summary>
    /// <param name="context">Contexto de datos de la aplicación.</param>
    /// <param name="logger">Logger de la clase.</param>
    /// <param name="exclusionFechaManager">Manager de exclusiones por fecha.</param>
    public CreateDetalleEscenarioCupoExclusionFechaHandler(
        IOzyParkAdminContext context,
        ILogger<CreateDetalleEscenarioCupoExclusionFechaHandler> logger,
        DetalleEscenarioCupoExclusionFechaManager exclusionFechaManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exclusionFechaManager);

        _context = context;
        _exclusionFechaManager = exclusionFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateDetalleEscenarioCupoExclusionFecha command, CancellationToken cancellationToken)
    {
        var result = await _exclusionFechaManager.SyncExclusionesAsync(command.EscenarioCupoId, command.Exclusiones, cancellationToken);

        var (nuevas, actualizar, eliminar) = result;

        if (eliminar.Any())
        {
            if (eliminar.Count() > 30)
            {
                await _context.BulkDeleteAsync(eliminar, cancellationToken);
            }
            else
            {
                _context.RemoveRange(eliminar);
            }
        }

        if (actualizar.Any())
        {
            _context.UpdateRange(actualizar);
        }

        if (nuevas.Any())
        {
            if (nuevas.Count() > 30)
            {
                await _context.BulkInsertAsync(nuevas, cancellationToken);
            }
            else
            {
                await _context.AddRangeAsync(nuevas, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
