using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignCajasToServicio"/>.
/// </summary>
public sealed class AssignCajasToServicioHandler : ServicioStateChangeableHandler<AssignCajasToServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignCajasToServicio"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignCajasToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<AssignCajasToServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(AssignCajasToServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.AssignCajasAsync(command.ServicioId, command.Cajas, cancellationToken);
}
