using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Lock;

/// <summary>
/// El manejador de <see cref="LockUser"/>.
/// </summary>
public sealed class LockUserHandler : CommandHandler<LockUser, UsuarioFullInfo>
{
    private readonly UserManager<Usuario> _userManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="LockUserHandler"/>.
    /// </summary>
    /// <param name="userManager">El <see cref="UserManager{TUser}"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public LockUserHandler(UserManager<Usuario> userManager, ILogger<LockUserHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        _userManager = userManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioFullInfo>> ExecuteAsync(LockUser command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        Usuario? usuario = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (usuario is null)
        {
            return new NotFound();
        }

        IdentityResult result = await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.UtcNow);
        return result.Succeeded ? usuario.ToFullInfo() : result.ToFailure();
    }
}
