using MassTransit.Mediator;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Create;

/// <summary>
/// El handler de <see cref="CreateUser"/>.
/// </summary>
public sealed class CreateUserHandler : MediatorRequestHandler<CreateUser, ResultOf<UsuarioInfo>>
{
    private readonly UsuarioService _usuarioCreator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateUserHandler"/>.
    /// </summary>
    /// <param name="usuarioService">El <see cref="UsuarioService"/>.</param>
    public CreateUserHandler(UsuarioService usuarioService)
    {
        ArgumentNullException.ThrowIfNull(usuarioService);
        _usuarioCreator = usuarioService;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioInfo>> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ResultOf<UsuarioInfo> result = await _usuarioCreator.CreateUserAsync(
            request.UserName,
            request.FriendlyName,
            request.Rut,
            request.Email,
            request.Roles,
            request.CentroCostos,
            request.Franquicias,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SendUsuarioAsync,
            onFailure: _ => _,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<UsuarioInfo>> SendUsuarioAsync(UsuarioInfo usuario, CancellationToken cancellationToken)
    {
        return await Task.FromResult(usuario);
    }
}
