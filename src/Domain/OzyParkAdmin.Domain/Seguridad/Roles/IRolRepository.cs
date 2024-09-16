namespace OzyParkAdmin.Domain.Seguridad.Roles;

/// <summary>
/// El repositorio de rol.
/// </summary>
public interface IRolRepository
{
    /// <summary>
    /// Consigue todos los roles de us usuario.
    /// </summary>
    /// <param name="userId">El id del usuario.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de roles asociados al usuario.</returns>
    Task<IEnumerable<Rol>> FinRolesByUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca todos los roles que coincidan con los nombres de rol y devuelve solo los roles hijo..
    /// </summary>
    /// <param name="roleNames">Los nombres de rol.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El listado de roles hijo.</returns>
    Task<List<Rol>> ListChildRolesAsync(string[] roleNames, CancellationToken cancellationToken);
}
