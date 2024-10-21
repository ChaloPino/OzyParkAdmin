using MassTransit.Mediator;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Delete;

/// <summary>
/// El manejador de <see cref="DeleteFechasExcluidasCupo"/>.
/// </summary>
public sealed class DeleteFechasExcluidasCupoHandler : MediatorRequestHandler<DeleteFechasExcluidasCupo, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IFechaExcluidaCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteFechasExcluidasCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="IFechaExcluidaCupoRepository"/>.</param>
    public DeleteFechasExcluidasCupoHandler(IOzyParkAdminContext context, IFechaExcluidaCupoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(DeleteFechasExcluidasCupo request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        IEnumerable<FechaExcluidaCupo> fechasExcluidasToDelete = await _repository.FindFechasExcluidasAsync(request.FechasExcluidas, cancellationToken);

        return await SaveAsync(fechasExcluidasToDelete, cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<FechaExcluidaCupo> fechasExcluidasToDelete, CancellationToken cancellationToken)
    {
        if (fechasExcluidasToDelete.Count() < 30)
        {
            _context.RemoveRange(fechasExcluidasToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkDeleteAsync(fechasExcluidasToDelete, cancellationToken);
        }

        return new Success();
    }
}
