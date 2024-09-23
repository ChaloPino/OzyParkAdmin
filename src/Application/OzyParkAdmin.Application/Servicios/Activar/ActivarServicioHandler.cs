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
    public ActivarServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager) 
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(ActivarServicio request, CancellationToken cancellationToken) =>
        await _servicioManager.ActivarServicioAsync(request.ServicioId, cancellationToken);
}
