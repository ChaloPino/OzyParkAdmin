using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Delete;

/// <summary>
/// El manejador de <see cref="DeleteCupoFecha"/>
/// </summary>
public sealed class DeleteCupoFechaHandler : CommandHandler<DeleteCupoFecha>
{
    private readonly IOzyParkAdminContext _context;
    private readonly ICupoFechaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteCupoFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="ICupoFechaRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DeleteCupoFechaHandler(IOzyParkAdminContext context, ICupoFechaRepository repository, ILogger<DeleteCupoFechaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(DeleteCupoFecha command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        CupoFecha? cupoFecha = await _repository.FindByIdAsync(command.Id, cancellationToken);

        if (cupoFecha is null)
        {
            return new NotFound();
        }

        _context.Remove(cupoFecha);
        await _context.SaveChangesAsync(cancellationToken);
        return new Success();
    }
}
