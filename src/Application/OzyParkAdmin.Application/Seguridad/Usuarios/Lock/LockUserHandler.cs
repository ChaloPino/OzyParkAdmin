using MassTransit.Mediator;
using Microsoft.AspNetCore.Identity;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Lock;

/// <summary>
/// El manejador de <see cref="LockUser"/>.
/// </summary>
public sealed class LockUserHandler : MediatorRequestHandler<LockUser, ResultOf<UsuarioInfo>>
{
    private readonly UserManager<Usuario> _userManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="LockUserHandler"/>.
    /// </summary>
    /// <param name="userManager">El <see cref="UserManager{TUser}"/>.</param>
    public LockUserHandler(UserManager<Usuario> userManager)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        _userManager = userManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioInfo>> Handle(LockUser request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Usuario? usuario = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (usuario is null)
        {
            return new NotFound();
        }

        IdentityResult result = await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.UtcNow);
        return result.Succeeded ? usuario.ToInfo() : result.ToFailure();
    }
}
