using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// El manejador de <see cref="AssignCentrosCosto"/>.
/// </summary>
public sealed class AssignCentrosCostoHandler : ServicioStateChangeableHandler<AssignCentrosCosto>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="AssignCentrosCostoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    public AssignCentrosCostoHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(AssignCentrosCosto request, CancellationToken cancellationToken) =>
        await _servicioManager.AssignCentrosCostoAsync(request.ServicioId, request.CentrosCosto, cancellationToken);
}
