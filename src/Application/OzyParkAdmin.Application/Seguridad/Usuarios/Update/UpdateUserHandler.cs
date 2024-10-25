using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Update;

/// <summary>
/// Manejador de <see cref="UpdateUser"/>,
/// </summary>
public sealed class UpdateUserHandler : CommandHandler<UpdateUser, UsuarioFullInfo>
{
    private readonly UsuarioService _usuarioService;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UsuarioService"/>.
    /// </summary>
    /// <param name="usuarioService">El <see cref="UsuarioService"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateUserHandler(UsuarioService usuarioService, ILogger<UpdateUserHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(usuarioService);
        _usuarioService = usuarioService;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioFullInfo>> ExecuteAsync(UpdateUser command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _usuarioService.UpdateUserAsync(
            command.Id,
            command.UserName,
            command.FriendlyName,
            command.Rut,
            command.Email,
            command.Roles,
            command.CentroCostos,
            command.Franquicias,
            cancellationToken);
    }
}
