using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignCentrosCostoToServicio"/>.
/// </summary>
public sealed class AssignCentrosCostoToServicioHandler : ServicioStateChangeableHandler<AssignCentrosCostoToServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignCentrosCostoToServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignCentrosCostoToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<AssignCentrosCostoToServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(AssignCentrosCostoToServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.AssignCentrosCostoAsync(command.ServicioId, command.CentrosCosto, cancellationToken);
}
