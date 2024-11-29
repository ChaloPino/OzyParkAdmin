using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Delete;

/// <summary>
/// El manejador de <see cref="DeleteEscenarioCupo"/>.
/// </summary>
public sealed class DeleteEscenarioCupoHandler : CommandHandler<DeleteEscenarioCupo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IEscenarioCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteEscenarioCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="IEscenarioCupoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DeleteEscenarioCupoHandler(IOzyParkAdminContext context, IEscenarioCupoRepository repository, ILogger<DeleteEscenarioCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(DeleteEscenarioCupo command, CancellationToken cancellationToken)
    {
        // Validar el comando proporcionado
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        if (!command.ids.Any())
        {
            return new ValidationError("EscenariosCupos", "Debe especificar al menos un escenario de cupo para eliminar.");
        }

        // Buscar los escenarios de cupo que se deben eliminar utilizando sus IDs
        IEnumerable<EscenarioCupo> escenariosCuposToDelete = await _repository.FindByIdsAsync(command.ids, cancellationToken);

        if (!escenariosCuposToDelete.Any())
        {
            Logger.LogWarning("No se encontraron los escenarios de cupo especificados.");
            return new ValidationError("EscenariosCupos", "No se encontraron los escenarios de cupo especificados.");
        }

        await DeleteRelations(escenariosCuposToDelete, cancellationToken);

        // Proceder a guardar los cambios (eliminar los escenarios de cupo)
        return await SaveAsync(escenariosCuposToDelete, cancellationToken);
    }

    private async Task DeleteRelations(IEnumerable<EscenarioCupo> escenariosCuposToDelete, CancellationToken cancellationToken)
    {
        foreach (var escenarioCupo in escenariosCuposToDelete)
        {
            // Eliminar exclusiones por fecha: usar eliminación masiva si hay más de 30 exclusiones
            if (escenarioCupo.ExclusionesPorFecha.Count < 30)
            {
                _context.RemoveRange(escenarioCupo.ExclusionesPorFecha);
            }
            else
            {
                await _context.BulkDeleteAsync(escenarioCupo.ExclusionesPorFecha, cancellationToken);
            }

            // Eliminar exclusiones de días: también considerar si usar eliminación masiva
            if (escenarioCupo.Exclusiones.Count < 30)
            {
                _context.RemoveRange(escenarioCupo.Exclusiones);
            }
            else
            {
                await _context.BulkDeleteAsync(escenarioCupo.Exclusiones, cancellationToken);
            }

            // Eliminar los detalles del escenario
            if (escenarioCupo.DetallesEscenarioCupo.Count < 30)
            {
                _context.RemoveRange(escenarioCupo.DetallesEscenarioCupo);
            }
            else
            {
                await _context.BulkDeleteAsync(escenarioCupo.DetallesEscenarioCupo, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Guarda los cambios eliminando los escenarios de cupo especificados.
    /// </summary>
    /// <param name="escenariosCuposToDelete">Lista de escenarios de cupo a eliminar.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Retorna un <see cref="SuccessOrFailure"/> indicando si la operación fue exitosa.</returns>
    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<EscenarioCupo> escenariosCuposToDelete, CancellationToken cancellationToken)
    {
        if (escenariosCuposToDelete.Count() < 30)
        {
            _context.RemoveRange(escenariosCuposToDelete);
        }
        else
        {
            await _context.BulkDeleteAsync(escenariosCuposToDelete, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
