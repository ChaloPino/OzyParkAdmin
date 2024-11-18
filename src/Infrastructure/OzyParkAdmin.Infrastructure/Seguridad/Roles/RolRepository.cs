using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Seguridad.Roles;

/// <summary>
/// El repositorio de <see cref="Rol"/>.
/// </summary>
public class RolRepository(OzyParkAdminContext context) : Repository<Rol>(context), IRolRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Rol>> FinRolesByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var query = from userRole in Context.Set<UsuarioRol>()
                    join role in Context.Set<Rol>() on userRole.RoleId equals role.Id
                    where userRole.UserId == userId
                    select role;
        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Rol>> ListChildRolesAsync(string[] roleNames, CancellationToken cancellationToken)
    {
        IEnumerable<Rol> roles = await EntitySet.AsNoTracking().AsSplitQuery().Include(x => x.ChildRoles).Where(x => Enumerable.Contains(roleNames, x.Name)).ToListAsync(cancellationToken);
        return [.. roles.SelectMany(x => x.ChildRoles).DistinctBy(x => x.Name, StringComparer.OrdinalIgnoreCase).OrderBy(x => x.Name)];
    }
}
