using System.Security.Claims;

namespace OzyParkAdmin.Application.Identity;

/// <summary>
/// Contiene métodos de extensión para <see cref="ClaimsPrincipal"/>.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    private delegate bool WhereSelector<in T, TResult>(T item, out TResult result);

    /// <summary>
    /// Consigue todos los franquicias que tiene el usuario.
    /// </summary>
    /// <param name="user">El usuario de quien se buscarán los franquicias asociados.</param>
    /// <returns>El listado de franquicias asociados a <paramref name="user"/>.</returns>
    public static int[]? GetCentrosCosto(this ClaimsPrincipal user)
    {
        if (user.IsInRole(RolesConstant.SuperAdmin))
        {
            return null;
        }

        IEnumerable<Claim> matchedClaims = user.FindAll(ClaimTypesExtended.CentroCosto);
        return matchedClaims.WhereSelect<Claim, int>(TryConvertInt32).ToArray();
    }

    /// <summary>
    /// Consigue todas las franquicias que tiene el usuario.
    /// </summary>
    /// <param name="user">El usuario de quien se buscarán las franquicias asociadas.</param>
    /// <returns>El listado de franquicias asociadas a <paramref name="user"/>.</returns>
    public static int[]? GetFranquicias(this ClaimsPrincipal user)
    {
        if (user.IsInRole(RolesConstant.SuperAdmin))
        {
            return null;
        }

        IEnumerable<Claim> matchedClaims = user.FindAll(ClaimTypesExtended.Franquicia);
        return matchedClaims.WhereSelect<Claim, int>(TryConvertInt32).ToArray();
    }

    /// <summary>
    /// Consigue todos los roles que tiene el usuario.
    /// </summary>
    /// <param name="user">El usuario de quien se buscarán los roles asociados.</param>
    /// <returns>El listado de roles asociados a <paramref name="user"/>.</returns>
    public static string[] GetRoles(this ClaimsPrincipal user)
    {
        IEnumerable<Claim> matchedClaims = user.FindAll(ClaimTypes.Role);
        return matchedClaims.Select(claim => claim.Value).ToArray();
    }

    /// <summary>
    /// Consigue todos los roles que tiene el usuario.
    /// </summary>
    /// <param name="user">El usuario de quien se buscarán los roles asociados.</param>
    /// <returns>El listado de roles asociados a <paramref name="user"/>.</returns>
    public static string? GetFriendlyName(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.GivenName) ?? user.Identity?.Name;
    }

    private static bool TryConvertInt32(Claim claim, out int result)
    {
        return int.TryParse(claim.Value, out result);
    }

    private static IEnumerable<TResult> WhereSelect<T, TResult>(this IEnumerable<T> source, WhereSelector<T, TResult> whereSelector)
    {
        foreach (var claim in source)
        {
            if (whereSelector(claim, out TResult result))
            {
                yield return result;
            }
        }
    }
}
