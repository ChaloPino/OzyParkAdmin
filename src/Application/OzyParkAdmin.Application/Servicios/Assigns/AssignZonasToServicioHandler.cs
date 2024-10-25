using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignZonasToServicio"/>.
/// </summary>
public sealed class AssignZonasToServicioHandler : ServicioStateChangeableHandler<AssignZonasToServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignZonasToServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignZonasToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<AssignZonasToServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(AssignZonasToServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.AssignZonasAsync(command.ServicioId, command.Zonas, cancellationToken);
}
