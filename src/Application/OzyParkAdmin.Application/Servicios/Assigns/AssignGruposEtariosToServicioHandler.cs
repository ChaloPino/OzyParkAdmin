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
    public AssignGruposEtariosToServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(AssignGruposEtariosToServicio request, CancellationToken cancellationToken) =>
        await _servicioManager.AssignGruposEtariosAsync(request.ServicioId, request.GruposEtarios, cancellationToken);
}
