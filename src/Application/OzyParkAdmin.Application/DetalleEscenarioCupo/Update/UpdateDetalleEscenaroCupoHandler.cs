using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.Update;

/// <summary>
/// Handler encargado de la actualización de los detalles de un escenario de cupo.
/// </summary>
public sealed class UpdateDetalleEscenarioCupoHandler : CommandHandler<UpdateDetalleEscenarioCupo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly DetalleEscenarioCupoManager _detalleManager;

    public UpdateDetalleEscenarioCupoHandler(
        IOzyParkAdminContext context,
        ILogger<UpdateDetalleEscenarioCupoHandler> logger,
        DetalleEscenarioCupoManager detalleManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(detalleManager);

        _context = context;
        _detalleManager = detalleManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(UpdateDetalleEscenarioCupo command, CancellationToken cancellationToken)
    {
        // Validar que el comando contenga detalles
        if (!command.Detalles.Any())
        {
            return new ValidationError("Detalles", "No se proporcionaron detalles para actualizar.");
        }

        // Sincronizar detalles usando el manager
        var result = await _detalleManager.SyncDetallesAsync(command.EscenarioCupoId, command.Detalles, cancellationToken);

        var (nuevos, actualizar, eliminar) = result;

        // Eliminar detalles no incluidos en la nueva lista
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

        // Actualizar detalles existentes
        if (actualizar.Any())
        {
            _context.UpdateRange(actualizar);
        }

        // Insertar nuevos detalles
        if (nuevos.Any())
        {
            if (nuevos.Count() > 30)
            {
                await _context.BulkInsertAsync(nuevos, cancellationToken);
            }
            else
            {
                await _context.AddRangeAsync(nuevos, cancellationToken);
            }
        }

        // Guardar los cambios
        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
