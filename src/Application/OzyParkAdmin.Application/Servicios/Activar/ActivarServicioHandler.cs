using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Activar;

/// <summary>
/// El manejador de <see cref="ActivarServicio"/>.
/// </summary>
public sealed class ActivarServicioHandler : ServicioStateChangeableHandler<ActivarServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ActivarServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ActivarServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<ActivarServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(ActivarServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.ActivarServicioAsync(command.ServicioId, cancellationToken);
}
