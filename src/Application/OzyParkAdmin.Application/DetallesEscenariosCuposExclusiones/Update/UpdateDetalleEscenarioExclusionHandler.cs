using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetallesEscenariosExclusiones.Update;

/// <summary>
/// Handler encargado de la actualización de las exclusiones de un escenario de cupo.
/// </summary>
public sealed class UpdateDetalleEscenarioExclusionHandler : CommandHandler<UpdateDetalleEscenarioExclusion>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IDetalleEscenarioCupoExclusionRepository _detalleExclusionRepository;
    private readonly DetalleEscenarioCupoExclusionManager _detalleExclusionManager;

    public UpdateDetalleEscenarioExclusionHandler(
        IOzyParkAdminContext context,
        ILogger<UpdateDetalleEscenarioExclusionHandler> logger,
        IDetalleEscenarioCupoExclusionRepository detalleExclusionRepository,
        DetalleEscenarioCupoExclusionManager detalleExclusionManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(detalleExclusionRepository);
        ArgumentNullException.ThrowIfNull(detalleExclusionManager);

        _context = context;
        _detalleExclusionRepository = detalleExclusionRepository;
        _detalleExclusionManager = detalleExclusionManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(UpdateDetalleEscenarioExclusion command, CancellationToken cancellationToken)
    {
        // Obtener las exclusiones actuales del escenario
        var exclusionesExistentes = await _detalleExclusionRepository.GetExclusionesByEscenarioCupoIdAsync(command.escenarioCupoId, cancellationToken);

        // Verificar si la lista de exclusiones está vacía (se eliminarán todas las exclusiones)
        if (!command.exclusiones.Any())
        {
            _context.RemoveRange(exclusionesExistentes);
            await _context.SaveChangesAsync(cancellationToken);
            return new Success();
        }

        // Actualizar las exclusiones utilizando el DetalleEscenarioCupoExclusionManager
        var updateExclusionesResult = await _detalleExclusionManager.UpdateExclusionesAsync(
            command.escenarioCupoId,
            command.exclusiones,
            cancellationToken);

        if (updateExclusionesResult.IsFailure(out var failure))
        {
            return failure;
        }

        // Guardar los cambios (usando bulk insert si es necesario)
        return await updateExclusionesResult.MatchAsync(
            onSuccess: SaveExclusionesAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Guarda las exclusiones actualizadas.
    /// </summary>
    private async Task<SuccessOrFailure> SaveExclusionesAsync(IEnumerable<DetalleEscenarioCupoExclusion> exclusiones, CancellationToken cancellationToken)
    {
        if (exclusiones.Count() < 30)
        {
            _context.UpdateRange(exclusiones);
        }
        else
        {
            await _context.BulkInsertAsync(exclusiones, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
