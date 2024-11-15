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
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(DeleteEscenarioCupo command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        IEnumerable<EscenarioCupo> escenariosCuposToDelete = await _repository.FindEscenariosAsync(command.EscenariosCupos, cancellationToken);

        return await SaveAsync(escenariosCuposToDelete, cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<EscenarioCupo> escenariosCuposToDelete, CancellationToken cancellationToken)
    {
        if (escenariosCuposToDelete.Count() < 30)
        {
            _context.RemoveRange(escenariosCuposToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkDeleteAsync(escenariosCuposToDelete, cancellationToken);
        }

        return new Success();
    }
}
