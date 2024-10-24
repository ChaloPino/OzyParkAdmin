using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Create;

/// <summary>
/// El manejador de <see cref="CreateFechasExcluidasCupo"/>.
/// </summary>
public sealed class CreateFechasExcluidasCupoHandler : CommandHandler<CreateFechasExcluidasCupo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly FechaExcluidaCupoManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateFechasExcluidasCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="FechaExcluidaCupoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateFechasExcluidasCupoHandler(IOzyParkAdminContext context, FechaExcluidaCupoManager manager, ILogger<CreateFechasExcluidasCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateFechasExcluidasCupo command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        
        _context.AttachRange(command.CanalesVenta);

        var result = await _manager.CreateFechasExcluidasAsync(
            command.CentroCosto,
            command.CanalesVenta,
            command.FechaDesde,
            command.FechaHasta,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<FechaExcluidaCupo> fechaExcluidaCupos, CancellationToken cancellationToken)
    {
        if (fechaExcluidaCupos.Count() < 30)
        {
            await _context.AddRangeAsync(fechaExcluidaCupos, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkInsertAsync(fechaExcluidaCupos, cancellationToken);
        }

        return new Success();
    }
}
