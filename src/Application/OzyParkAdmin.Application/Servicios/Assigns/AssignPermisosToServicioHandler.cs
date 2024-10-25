using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignPermisosToServicio"/>.
/// </summary>
public sealed class AssignPermisosToServicioHandler : ServicioStateChangeableHandler<AssignPermisosToServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignPermisosToServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignPermisosToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<AssignPermisosToServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(AssignPermisosToServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.AssignPermisosAsync(command.ServicioId, command.Permisos, cancellationToken);
}
