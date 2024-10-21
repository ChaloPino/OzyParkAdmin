using MassTransit.Mediator;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Delete;

/// <summary>
/// El manejador de <see cref="DeleteCuposFecha"/>.
/// </summary>
public sealed class DeleteCuposFechaHandler : MediatorRequestHandler<DeleteCuposFecha, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly ICupoFechaRepository _repository;
    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteCuposFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="CupoFechaManager"/>.</param>
    public DeleteCuposFechaHandler(IOzyParkAdminContext context, ICupoFechaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(DeleteCuposFecha request, CancellationToken cancellationToken)
    {
        IEnumerable<CupoFecha> cuposFecha = await _repository.FindByUniqueKeysAsync(
            request.FechaDesde,
            request.FechaHasta,
            request.EscenarioCupo,
            request.CanalesVenta,
            request.DiasSemana,
            request.HoraInicio,
            request.HoraTermino,
            request.IntervaloMinutos,
            cancellationToken);

        if (cuposFecha.Count() < 30)
        {
            _context.RemoveRange(cuposFecha);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkDeleteAsync(cuposFecha, cancellationToken);
        }

        return new Success();
    }
}
