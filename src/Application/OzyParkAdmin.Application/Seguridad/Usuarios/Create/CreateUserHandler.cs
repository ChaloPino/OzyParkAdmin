using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Create;

/// <summary>
/// El handler de <see cref="CreateUser"/>.
/// </summary>
public sealed class CreateUserHandler : CommandHandler<CreateUser, UsuarioFullInfo>
{
    private readonly UsuarioService _usuarioCreator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateUserHandler"/>.
    /// </summary>
    /// <param name="usuarioService">El <see cref="UsuarioService"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateUserHandler(UsuarioService usuarioService, ILogger<CreateUserHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(usuarioService);
        _usuarioCreator = usuarioService;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioFullInfo>> ExecuteAsync(CreateUser command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Aplicar lógica de negocio.
        ResultOf<UsuarioFullInfo> result = await _usuarioCreator.CreateUserAsync(
            command.UserName,
            command.FriendlyName,
            command.Rut,
            command.Email,
            command.Roles,
            command.CentroCostos,
            command.Franquicias,
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
