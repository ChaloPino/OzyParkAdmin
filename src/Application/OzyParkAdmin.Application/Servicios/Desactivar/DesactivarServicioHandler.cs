using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Desactivar;

/// <summary>
/// El manejador de <see cref="DesactivarServicio"/>.
/// </summary>
public sealed class DesactivarServicioHandler : ServicioStateChangeableHandler<DesactivarServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DesactivarServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DesactivarServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<DesactivarServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(DesactivarServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.DesactivarServicioAsync(command.ServicioId, cancellationToken);
}
