using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.Update;

/// <summary>
/// Handler encargado de la actualización de exclusiones por fecha de un escenario de cupo.
/// </summary>
public sealed class UpdateDetalleEscenarioCupoExclusionFechaHandler : CommandHandler<UpdateDetalleEscenarioCupoExclusionFecha>
{
    private readonly IOzyParkAdminContext _context;
    private readonly DetalleEscenarioCupoExclusionFechaManager _exclusionFechaManager;

    /// <summary>
    /// Constructor de la clase <see cref="UpdateDetalleEscenarioCupoExclusionFechaHandler"/>.
    /// </summary>
    /// <param name="context">Contexto de datos de la aplicación.</param>
    /// <param name="logger">Logger de la clase.</param>
    /// <param name="exclusionFechaManager">Manager de exclusiones por fecha.</param>
    public UpdateDetalleEscenarioCupoExclusionFechaHandler(
        IOzyParkAdminContext context,
        ILogger<UpdateDetalleEscenarioCupoExclusionFechaHandler> logger,
        DetalleEscenarioCupoExclusionFechaManager exclusionFechaManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exclusionFechaManager);

        _context = context;
        _exclusionFechaManager = exclusionFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(UpdateDetalleEscenarioCupoExclusionFecha command, CancellationToken cancellationToken)
    {
       

        // Sincronizar exclusiones usando el manager
        var result = await _exclusionFechaManager.SyncExclusionesAsync(command.EscenarioCupoId, command.ExclusionesFecha, cancellationToken);

        var (nuevas, actualizar, eliminar) = result;

        // Eliminar exclusiones no incluidas en la nueva lista
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

        // Actualizar exclusiones existentes
        if (actualizar.Any())
        {
            _context.UpdateRange(actualizar);
        }

        // Insertar nuevas exclusiones
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

        // Guardar los cambios
        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
