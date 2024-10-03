using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Application.Identity;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Seguridad.Permisos;

/// <summary>
/// Servicio que consulta los permisos de la caja dependiendo del rol.
/// </summary>
public sealed class PermisoCajaService
{
    private readonly OzyParkAdminContext _context;

    /// <summary>
    /// Crea una nueva instancia de <see cref="PermisoCajaService"/>.
    /// </summary>
    /// <param name="context"></param>
    public PermisoCajaService(OzyParkAdminContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }
    /// <summary>
    /// Consigue las acciones que puede tener el usuario con el control de la caja.
    /// </summary>
    /// <param name="user">El usuario que realiza la consulta.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La acciones que puede tener el usuario.</returns>
    public async Task<CajaAcciones> FindPermisosByUser(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        string[] roles = user.GetRoles();
        List<PermisoRolCaja> permisos = await _context.Set<PermisoRolCaja>()
            .Where(x => roles.Contains(x.Rol.Name))
            .ToListAsync(cancellationToken);

        CajaAcciones acciones = CajaAcciones.None;

        foreach (PermisoRolCaja permiso in permisos)
        {
            acciones |= permiso.CajaAcciones;
        }

        return acciones;
    }

    /// <summary>
    /// Revisa si se tiene el permiso para realizar la acción.
    /// </summary>
    /// <param name="acciones">La acciones completas de un rol.</param>
    /// <param name="acciontAEvaluar">La acción a evaluar.</param>
    /// <returns><c>true</c> si tiene la <paramref name="acciontAEvaluar"/>; en caso contrario, <c>false</c>.</returns>
    public static bool HasAction(CajaAcciones acciones, CajaAcciones acciontAEvaluar)
    {
        if (acciontAEvaluar == CajaAcciones.None)
        {
            return false;
        }

        return (acciones & acciontAEvaluar) == acciontAEvaluar;
    }
}
