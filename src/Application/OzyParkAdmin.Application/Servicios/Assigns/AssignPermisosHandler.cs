using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignPermisos"/>.
/// </summary>
public sealed class AssignPermisosHandler : ServicioStateChangeableHandler<AssignPermisos>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignPermisosHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    public AssignPermisosHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(AssignPermisos request, CancellationToken cancellationToken) =>
        await _servicioManager.AssignPermisosAsync(request.ServicioId, request.Permisos, cancellationToken);
}
