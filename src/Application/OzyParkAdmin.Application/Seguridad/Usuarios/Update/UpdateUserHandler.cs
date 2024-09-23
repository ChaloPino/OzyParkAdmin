using MassTransit.Mediator;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Update;

/// <summary>
/// Manejador de <see cref="UpdateUser"/>,
/// </summary>
public sealed class UpdateUserHandler : MediatorRequestHandler<UpdateUser, ResultOf<UsuarioInfo>>
{
    private readonly UsuarioService _usuarioService;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UsuarioService"/>.
    /// </summary>
    /// <param name="usuarioService">El <see cref="UsuarioService"/>.</param>
    public UpdateUserHandler(UsuarioService usuarioService)
    {
        ArgumentNullException.ThrowIfNull(usuarioService);
        _usuarioService = usuarioService;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioInfo>> Handle(UpdateUser request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _usuarioService.UpdateUserAsync(
            request.Id,
            request.UserName,
            request.FriendlyName,
            request.Rut,
            request.Email,
            request.Roles,
            request.CentroCostos,
            request.Franquicias,
            cancellationToken);
    }
}
