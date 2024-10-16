using MassTransit.Mediator;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Create;

/// <summary>
/// El handler de <see cref="CreateUser"/>.
/// </summary>
public sealed class CreateUserHandler : MediatorRequestHandler<CreateUser, ResultOf<UsuarioFullInfo>>
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
    protected override async Task<ResultOf<UsuarioFullInfo>> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Aplicar lógica de negocio.
        ResultOf<UsuarioFullInfo> result = await _usuarioCreator.CreateUserAsync(
            request.UserName,
            request.FriendlyName,
            request.Rut,
            request.Email,
            request.Roles,
            request.CentroCostos,
            request.Franquicias,
            cancellationToken);

        // Aplica otra lógica de aplicación
        // En este caso enviar correo electr
        return await result.MatchAsync(
            onSuccess: SendEmailUsuarioAsync,
            onFailure: _ => _,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<UsuarioFullInfo>> SendEmailUsuarioAsync(UsuarioFullInfo usuario, CancellationToken cancellationToken)
    {
        return await Task.FromResult(usuario);
    }
}
