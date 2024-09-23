using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignCajas"/>.
/// </summary>
public sealed class AssignCajasHandler : ServicioStateChangeableHandler<AssignCajas>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignCajas"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    public AssignCajasHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(AssignCajas request, CancellationToken cancellationToken) =>
        await _servicioManager.AssignCajasAsync(request.ServicioId, request.Cajas, cancellationToken);
}
