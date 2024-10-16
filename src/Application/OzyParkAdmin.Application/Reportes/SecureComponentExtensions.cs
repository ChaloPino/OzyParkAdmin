using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes;

/// <summary>
/// Contiene métodos de extensión para <see cref="ISecureComponent"/>
/// </summary>
public static class SecureComponentExtensions
{
    /// <summary>
    /// Revisa si el componente tiene permiso para <paramref name="roles"/>.
    /// </summary>
    /// <param name="component">El componente que se evaluará.</param>
    /// <param name="roles">La lista de roles a evaluar.</param>
    /// <returns>
    /// <c>true</c> si <paramref name="roles"/> es super admin,
    /// o si <paramref name="component"/> tiene permiso para todos los roles,
    /// o si alguno de los roles de <paramref name="roles"/> es parte de los roles permitidos de <paramref name="component"/>;
    /// en caso contrario, <c>false</c>.
    /// </returns>
    public static bool IsInRole(this ISecureComponent component, string[] roles)
    {
        if (Array.Exists(roles, x => string.Equals(RolesConstant.SuperAdmin, x, StringComparison.OrdinalIgnoreCase)))
        {
            roles = [RolesConstant.AllRoles];
        }

        bool allRoles = Array.Exists(roles, x => string.Equals(RolesConstant.AllRoles, x, StringComparison.OrdinalIgnoreCase));

        // Si se solicita por todos los roles (SuperAdmin), entonces si tiene permiso sobre el componente.
        if (allRoles)
        {
            return true;
        }

        // Si el componente está permitido para todos los roles,
        // entonces no es necesario evaluar la coincidencia de roles y se tiene permiso sobre el componente.
        if (component.Roles.Any(x => string.Equals(RolesConstant.AllRoles, x, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return component.Roles.Any(x => roles.Contains(x, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Revisa si el componente puede ser accedido por el <paramref name="user"/>.
    /// </summary>
    /// <param name="component">El componente que será evaluado.</param>
    /// <param name="user">El <see cref="ClaimsPrincipal"/> que se evaluará si tiene permiso sobre el <paramref name="component"/>.</param>
    /// <returns><c>true</c> si tiene acceso; en caso contrario, <c>false</c>.</returns>
    public static bool IsAccessibleByUser(this ISecureComponent component, ClaimsPrincipal user)
    {
        string[] roles = user.GetRoles();
        return component.IsInRole(roles);
    }
}
