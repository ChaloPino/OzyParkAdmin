using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignGruposEtariosToServicio"/>.
/// </summary>
public sealed class AssignGruposEtariosToServicioHandler : ServicioStateChangeableHandler<AssignGruposEtariosToServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignGruposEtariosToServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public AssignGruposEtariosToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<AssignGruposEtariosToServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(AssignGruposEtariosToServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.AssignGruposEtariosAsync(command.ServicioId, command.GruposEtarios, cancellationToken);
}
