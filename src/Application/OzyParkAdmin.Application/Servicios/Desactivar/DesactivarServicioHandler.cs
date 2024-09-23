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
    public DesactivarServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(DesactivarServicio request, CancellationToken cancellationToken) =>
        await _servicioManager.DesactivarServicioAsync(request.ServicioId, cancellationToken);
}
