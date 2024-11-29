using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.EscenariosCupo.Create;

/// <summary>
/// El manejador de <see cref="CreateEscenarioCupo"/>.
/// </summary>
public sealed class CreateEscenarioCupoHandler : CommandHandler<CreateEscenarioCupo, EscenarioCupoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly EscenarioCupoManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateEscenarioCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="EscenarioCupoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateEscenarioCupoHandler(IOzyParkAdminContext context, EscenarioCupoManager manager, ILogger<CreateEscenarioCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<EscenarioCupoFullInfo>> ExecuteAsync(CreateEscenarioCupo command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Invocar el manager para crear el escenario de cupo junto con los detalles y exclusiones
        ResultOf<EscenarioCupo> result = await _manager.CreateEscenarioCupoAsync(
            command.CentroCosto,
            command.ZonaInfo,
            command.Nombre,
            command.EsHoraInicio,
            command.MinutosAntes,
            command.EsActivo,
            command.Detalles,
            command.Exclusiones,
            cancellationToken);

        return await result.BindAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Guarda el escenario de cupo y sus detalles asociados en la base de datos.
    /// </summary>
    private async Task<ResultOf<EscenarioCupoFullInfo>> SaveAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        await _context.AddAsync(escenarioCupo, cancellationToken);

        if (escenarioCupo.DetallesEscenarioCupo.Any())
        {
            await _context.AddRangeAsync(escenarioCupo.DetallesEscenarioCupo, cancellationToken);
        }

        if (escenarioCupo.ExclusionesPorFecha.Any())
        {
            await _context.AddRangeAsync(escenarioCupo.ExclusionesPorFecha, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return escenarioCupo.ToFullInfo();
    }
}
