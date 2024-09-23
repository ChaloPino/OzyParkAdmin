using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;

/// <summary>
/// El repositorio para <see cref="Usuario"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/></param>
public sealed class UsuarioRepository(OzyParkAdminContext context) : Repository<Usuario>(context), IUsuarioRepository
{
    /// <inheritdoc/>
    public async Task<PagedList<UsuarioInfo>> BuscarUsuariosAsync(string? searchText, int[]? centrosCosto, string[]? roles, FilterExpressionCollection<Usuario> filterExpressions, SortExpressionCollection<Usuario> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        IQueryable<Usuario> query = EntitySet;

        if (centrosCosto?.Length > 0)
        {
            query = query.Where(usuario => usuario.CentrosCosto.Any(cc => centrosCosto.Contains(cc.CentroCostoId)));
        }

        if (roles?.Length > 0)
        {
            query = (from user in query
                     from userRole in user.Roles
                     join rol in Context.Set<Rol>() on userRole.RoleId equals rol.Id
                     where roles.Contains(rol.Name)
                     select user).Distinct();
        }

        query = filterExpressions.Where(query);

        if (searchText is not null)
        {
            query = query.Where(usuario => usuario.UserName.Contains(searchText) ||
                usuario.FriendlyName.Contains(searchText) ||
                usuario.Email!.Contains(searchText));
        }
        
        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        IEnumerable<Usuario> lista = await sortExpressions.Sort(query).Skip(page *  pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

        IEnumerable<UsuarioInfo> usuarios = await LoadUsuariosAsync(lista, cancellationToken);

        return new PagedList<UsuarioInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = usuarios,
        };
    }

    private async Task<IEnumerable<UsuarioInfo>> LoadUsuariosAsync(IEnumerable<Usuario> usuarios, CancellationToken cancellationToken)
    {
        Guid[] rolesId = usuarios.SelectMany(x => x.Roles.Select(x => x.RoleId)).ToArray();
        int[] centrosCostoId = usuarios.SelectMany(x => x.CentrosCosto.Select(x => x.CentroCostoId)).ToArray();
        int[] franquiciasId = usuarios.SelectMany(x => x.Franquicias.Select(x => x.FranquiciaId)).ToArray();

        List<Rol> roles = await Context.Set<Rol>().Where(x => rolesId.Contains(x.Id)).ToListAsync(cancellationToken);
        List<CentroCosto> centrosCosto = await Context.Set<CentroCosto>().Where(x => centrosCostoId.Contains(x.Id)).ToListAsync(cancellationToken);
        List<Franquicia> franquicias = await Context.Set<Franquicia>().Where(x => franquiciasId.Contains(x.Id)).ToListAsync(cancellationToken);

        return usuarios.Select(x => ToUsuarioInfo(x, roles, centrosCosto, franquicias));
    }

    private static UsuarioInfo ToUsuarioInfo(Usuario usuario, List<Rol> roles, List<CentroCosto> centrosCosto, List<Franquicia> franquicias) =>
        usuario.ToInfo(
            roles.Where(x => ContieneRol(usuario, x)).ToList(),
            centrosCosto.Where(x => ContieneCentroCosto(usuario, x)).ToList(),
            franquicias.Where(x => ContieneFranquicia(usuario, x)).ToList());

    private static bool ContieneRol(Usuario usuario, Rol rol) =>
        usuario.Roles.Any(x => x.RoleId == rol.Id);

    private static bool ContieneCentroCosto(Usuario usuario, CentroCosto centroCosto) =>
        usuario.CentrosCosto.Any(x => x.CentroCostoId == centroCosto.Id);

    private static bool ContieneFranquicia(Usuario usuario, Franquicia franquicia) =>
        usuario.Franquicias.Any(x => x.FranquiciaId == franquicia.Id);
}
