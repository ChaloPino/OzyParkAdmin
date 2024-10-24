using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Unlock;

/// <summary>
/// El manejador de <see cref="UnlockUser"/>.
/// </summary>
public sealed class UnlockUserHandler : CommandHandler<UnlockUser, UsuarioFullInfo>
{
    private readonly UserManager<Usuario> _userManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UnlockUserHandler"/>.
    /// </summary>
    /// <param name="userManager">El <see cref="UserManager{TUser}"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UnlockUserHandler(UserManager<Usuario> userManager, ILogger<UnlockUserHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        _userManager = userManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioFullInfo>> ExecuteAsync(UnlockUser command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        Usuario? usuario = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (usuario is null)
        {
            return new NotFound();
        }

        var result = await _userManager.SetLockoutEndDateAsync(usuario, null);
        return result.Succeeded ? usuario.ToFullInfo() : result.ToFailure();
    }
}
