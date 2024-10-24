using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Delete;

/// <summary>
/// El manejador de <see cref="DeleteCuposFecha"/>.
/// </summary>
public sealed class DeleteCuposFechaHandler : CommandHandler<DeleteCuposFecha>
{
    private readonly IOzyParkAdminContext _context;
    private readonly ICupoFechaRepository _repository;
    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteCuposFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="CupoFechaManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DeleteCuposFechaHandler(IOzyParkAdminContext context, ICupoFechaRepository repository, ILogger<DeleteCuposFechaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(DeleteCuposFecha command, CancellationToken cancellationToken)
    {
        IEnumerable<CupoFecha> cuposFecha = await _repository.FindByUniqueKeysAsync(
            command.FechaDesde,
            command.FechaHasta,
            command.EscenarioCupo,
            command.CanalesVenta,
            command.DiasSemana,
            command.HoraInicio,
            command.HoraTermino,
            command.IntervaloMinutos,
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
