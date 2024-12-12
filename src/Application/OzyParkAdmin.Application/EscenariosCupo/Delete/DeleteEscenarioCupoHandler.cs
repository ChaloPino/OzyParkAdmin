using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Repositories;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Delete;

/// <summary>
/// El manejador de <see cref="DeleteEscenarioCupo"/>.
/// </summary>
public sealed class DeleteEscenarioCupoHandler : CommandHandler<DeleteEscenarioCupo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IEscenarioCupoRepository _repository;
    private readonly IDetalleEscenarioCupoRepository _detalleRepository;
    private readonly IDetalleEscenarioCupoExclusionRepository _exclusionRepository;
    private readonly IDetalleEscenarioCupoExclusionFechaRepository _exclusionFechaRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteEscenarioCupoHandler"/>.
    /// </summary>
    public DeleteEscenarioCupoHandler(
        IOzyParkAdminContext context,
        IEscenarioCupoRepository repository,
        IDetalleEscenarioCupoRepository detalleRepository,
        IDetalleEscenarioCupoExclusionRepository exclusionRepository,
        IDetalleEscenarioCupoExclusionFechaRepository exclusionFechaRepository,
        ILogger<DeleteEscenarioCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(detalleRepository, nameof(detalleRepository));
        ArgumentNullException.ThrowIfNull(exclusionRepository, nameof(exclusionRepository));
        ArgumentNullException.ThrowIfNull(exclusionFechaRepository, nameof(exclusionFechaRepository));

        _context = context;
        _repository = repository;
        _detalleRepository = detalleRepository;
        _exclusionRepository = exclusionRepository;
        _exclusionFechaRepository = exclusionFechaRepository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(DeleteEscenarioCupo command, CancellationToken cancellationToken)
    {
        // Validar el comando
        if (!command.ids.Any())
        {
            return new ValidationError("EscenariosCupos", "Debe especificar al menos un escenario de cupo para eliminar.");
        }

        // Buscar escenarios de cupo por IDs
        IEnumerable<EscenarioCupo> escenariosCupos = await _repository.FindByIdsAsync(command.ids, cancellationToken);

        if (!escenariosCupos.Any())
        {
            Logger.LogWarning("No se encontraron los escenarios de cupo especificados.");
            return new ValidationError("EscenariosCupos", "No se encontraron los escenarios de cupo especificados.");
        }

        // Validar y eliminar relaciones
        foreach (var escenarioCupo in escenariosCupos)
        {
            await DeleteRelationsAsync(escenarioCupo, cancellationToken);
        }

        // Eliminar los escenarios de cupo
        return await DeleteEscenariosCuposAsync(escenariosCupos, cancellationToken);
    }

    /// <summary>
    /// Elimina relaciones asociadas al escenario de cupo.
    /// </summary>
    private async Task DeleteRelationsAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        // Eliminar detalles
        var detalles = await _detalleRepository.FindByIdsAsync(escenarioCupo.Id, cancellationToken);
        if (detalles.Any())
        {
            if (detalles.Count() > 30)
            {
                await _context.BulkDeleteAsync(detalles, cancellationToken);
            }
            else
            {
                await _detalleRepository.RemoveDetallesAsync(detalles, cancellationToken);
            }
        }

        // Eliminar exclusiones
        var exclusiones = await _exclusionRepository.GetExclusionesByEscenarioCupoIdAsync(escenarioCupo.Id, cancellationToken);
        if (exclusiones.Any())
        {
            if (exclusiones.Count() > 30)
            {
                await _context.BulkDeleteAsync(exclusiones, cancellationToken);
            }
            else
            {
                _context.RemoveRange(exclusiones);
            }
        }

        // Eliminar exclusiones por fecha
        var exclusionesFecha = await _exclusionFechaRepository.GetExclusionesByEscenarioCupoIdAsync(escenarioCupo.Id, cancellationToken);
        if (exclusionesFecha.Any())
        {
            if (exclusionesFecha.Count() > 30)
            {
                await _context.BulkDeleteAsync(exclusionesFecha, cancellationToken);
            }
            else
            {
                _context.RemoveRange(exclusionesFecha);
            }
        }
    }


    /// <summary>
    /// Elimina los escenarios de cupo de la base de datos.
    /// </summary>
    private async Task<SuccessOrFailure> DeleteEscenariosCuposAsync(IEnumerable<EscenarioCupo> escenariosCupos, CancellationToken cancellationToken)
    {
        if (escenariosCupos.Count() < 30)
        {
            _context.RemoveRange(escenariosCupos);
        }
        else
        {
            await _context.BulkDeleteAsync(escenariosCupos, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
