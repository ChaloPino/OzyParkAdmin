using MassTransit.Mediator;
using Microsoft.AspNetCore.Identity;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Unlock;

/// <summary>
/// El manejador de <see cref="UnlockUser"/>.
/// </summary>
public sealed class UnlockUserHandler : MediatorRequestHandler<UnlockUser, ResultOf<UsuarioFullInfo>>
{
    private readonly UserManager<Usuario> _userManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UnlockUserHandler"/>.
    /// </summary>
    /// <param name="userManager">El <see cref="UserManager{TUser}"/>.</param>
    public UnlockUserHandler(UserManager<Usuario> userManager)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        _userManager = userManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<UsuarioFullInfo>> Handle(UnlockUser request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Usuario? usuario = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (usuario is null)
        {
            return new NotFound();
        }

        var result = await _userManager.SetLockoutEndDateAsync(usuario, null);
        return result.Succeeded ? usuario.ToFullInfo() : result.ToFailure();
    }
}
