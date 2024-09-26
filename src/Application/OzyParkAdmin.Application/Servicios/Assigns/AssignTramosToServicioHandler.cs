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
    public AssignTramosToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager) 
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(AssignTramosToServicio request, CancellationToken cancellationToken) =>
        await _servicioManager.AssignTramosAsync(request.ServicioId, request.Tramos, cancellationToken);
}
