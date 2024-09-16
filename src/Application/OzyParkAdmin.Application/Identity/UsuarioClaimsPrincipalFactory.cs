using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Identity;

/// <summary>
/// Proporciona métodos para crear un claims principal para el usuario dado.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="UserClaimsPrincipalFactory{TUser}"/>.
/// </remarks>
/// <param name="userManager">El <see cref="UserManager{TUser}"/> para recuperar información del usuario.</param>
/// <param name="roleManager">El <see cref="RoleManager{TRole}"/> para recuperar información del rol.</param>
/// <param name="optionsAccessor">El <see cref="IdentityOptions"/> configurado.</param>
public sealed class UsuarioClaimsPrincipalFactory(UserManager<Usuario> userManager, RoleManager<Rol> roleManager, IOptions<IdentityOptions> optionsAccessor) : UserClaimsPrincipalFactory<Usuario, Rol>(userManager, roleManager, optionsAccessor)
{
    /// <inheritdoc/>
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

        foreach (var franquicia in user.Franquicias)
        {
            identity.AddClaim(new Claim(ClaimTypesExtended.Franquicia, franquicia.ToString()!));
        }

        foreach (var centroCosto in user.CentrosCosto)
        {
            identity.AddClaim(new Claim(ClaimTypesExtended.CentroCosto, centroCosto.ToString()!));
        }

        identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FriendlyName));

        return identity;
    }
}
