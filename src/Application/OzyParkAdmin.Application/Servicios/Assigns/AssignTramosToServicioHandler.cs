﻿using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignTramosToServicio"/>.
/// </summary>
public sealed class AssignTramosToServicioHandler : ServicioStateChangeableHandler<AssignTramosToServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignTramosToServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignTramosToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<AssignTramosToServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(AssignTramosToServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.AssignTramosAsync(command.ServicioId, command.Tramos, cancellationToken);
}
