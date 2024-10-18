using MassTransit.Mediator;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Delete;

/// <summary>
/// El manejador de <see cref="DeleteCupoFecha"/>
/// </summary>
public sealed class DeleteCupoFechaHandler : MediatorRequestHandler<DeleteCupoFecha, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly ICupoFechaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteCupoFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="ICupoFechaRepository"/>.</param>
    public DeleteCupoFechaHandler(IOzyParkAdminContext context, ICupoFechaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(DeleteCupoFecha request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        CupoFecha? cupoFecha = await _repository.FindByIdAsync(request.Id, cancellationToken);

        if (cupoFecha is null)
        {
            return new NotFound();
        }

        _context.Remove(cupoFecha);
        await _context.SaveChangesAsync(cancellationToken);
        return new Success();
    }
}
