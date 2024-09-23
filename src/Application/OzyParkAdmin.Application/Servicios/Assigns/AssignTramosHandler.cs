using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignTramos"/>.
/// </summary>
public sealed class AssignTramosHandler : ServicioStateChangeableHandler<AssignTramos>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignTramosHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    public AssignTramosHandler(IOzyParkAdminContext context, ServicioManager servicioManager) 
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(AssignTramos request, CancellationToken cancellationToken) =>
        await _servicioManager.AssignTramosAsync(request.ServicioId, request.Tramos, cancellationToken);
}
