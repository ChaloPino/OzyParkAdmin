using Microsoft.AspNetCore.Identity;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Lógica de negocios para crear y actalizar usuarios..
/// </summary>
public class UsuarioService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IRolRepository _rolRepository;
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IFranquiciaRepository _faranquiciaRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UsuarioService"/>.
    /// </summary>
    /// <param name="userManager">El <see cref="UserManager{TUser}"/>.</param>
    /// <param name="rolRepository">El <see cref="IRolRepository"/>.</param>
    /// <param name="centroCostoRepository">El <see cref="ICentroCostoRepository"/>.</param>
    /// <param name="franquiciaRepository">El <see cref="IFranquiciaRepository"/>.</param>
    public UsuarioService(
        UserManager<Usuario> userManager,
        IRolRepository rolRepository,
        ICentroCostoRepository centroCostoRepository,
        IFranquiciaRepository franquiciaRepository)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(rolRepository);
        _userManager = userManager;
        _rolRepository = rolRepository;
        _centroCostoRepository = centroCostoRepository;
        _faranquiciaRepository = franquiciaRepository;
    }

    /// <summary>
    /// Crear un usuario.
    /// </summary>
    /// <param name="username">Nombre de usuario.</param>
    /// <param name="friendlyName">Nombre completo del usuario.</param>
    /// <param name="rut">El rut del usuario.</param>
    /// <param name="email">Dirección de correo electrónico del usuario.</param>
    /// <param name="roles">Roles asociados al usuario.</param>
    /// <param name="centrosCosto">Centros de costo asociados al usuario.</param>
    /// <param name="franquicias">Franquicias asociados al usuario.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Resultado de la creación de un usuario.</returns>
    public async Task<ResultOf<UsuarioFullInfo>> CreateUserAsync(string  username, string friendlyName, string? rut, string? email, IEnumerable<string> roles, IEnumerable<int> centrosCosto, IEnumerable<int> franquicias, CancellationToken cancellationToken)
    {
        Usuario usuario = Usuario.Create(username, friendlyName, rut, email);

        IdentityResult result = await _userManager.CreateAsync(usuario);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        result = await _userManager.AddToRolesAsync(usuario, roles);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        result = await AddToCentrosCostoAsync(usuario, centrosCosto);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        result = await AddToFranquiciasAsync(usuario, franquicias);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        IEnumerable<Rol> rolesPersisted = await _rolRepository.FinRolesByUserAsync(usuario.Id, cancellationToken);
        IEnumerable<CentroCostoInfo> centrosCostoPersisted = await _centroCostoRepository.ListCentrosCostoAsync(usuario.CentrosCosto.Select(x => x.CentroCostoId).ToArray(), cancellationToken);
        IEnumerable<Franquicia> franquiciasPersisted = await _faranquiciaRepository.ListFranquiciasAsync(usuario.CentrosCosto.Select(x => x.CentroCostoId).ToArray(), cancellationToken);

        return usuario.ToInfo(rolesPersisted.ToList(), centrosCostoPersisted.ToList(), franquiciasPersisted.ToList());
    }

    /// <summary>
    /// Actualizar un usuario.
    /// </summary>
    /// <param name="userId">Id del usuario a actualizar.</param>
    /// <param name="username">Nombre de usuario.</param>
    /// <param name="friendlyName">Nombre completo del usuario.</param>
    /// <param name="rut">El rut del usuario.</param>
    /// <param name="email">Dirección de correo electrónico del usuario.</param>
    /// <param name="roles">Roles asociados al usuario.</param>
    /// <param name="centrosCosto">Centros de costo asociados al usuario.</param>
    /// <param name="franquicias">Franquicias asociados al usuario.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Resultado de la creación de un usuario.</returns>
    public async Task<ResultOf<UsuarioFullInfo>> UpdateUserAsync(Guid userId, string username, string friendlyName, string? rut, string? email, IEnumerable<string> roles, IEnumerable<int> centrosCosto, IEnumerable<int> franquicias, CancellationToken cancellationToken)
    {
        Usuario? usuario = await _userManager.FindByIdAsync(userId.ToString());

        if (usuario is null)
        {
            return new NotFound();
        }

        usuario.SetUserName(username);
        usuario.SetEmail(email);
        usuario.SetFriendlyName(friendlyName);
        usuario.SetRut(rut);

        IEnumerable<Rol> rolesPersisted = await _rolRepository.FinRolesByUserAsync(usuario.Id, cancellationToken);

        List<string> rolesToAdd = (from role in roles
                                  join nullable in rolesPersisted on role equals nullable.Name into defRoles
                                  from persisted in defRoles.DefaultIfEmpty()
                                  where persisted is null
                                  select role).ToList();

        List<string> rolesToRemove = (from persisted in rolesPersisted
                                      join nullable in roles on persisted.Name equals nullable into defRoles
                                      from role in defRoles.DefaultIfEmpty()
                                      where role is null
                                      select persisted.Name).ToList();


        IdentityResult result = await _userManager.AddToRolesAsync(usuario, rolesToAdd);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        result = await _userManager.RemoveFromRolesAsync(usuario, rolesToRemove);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        List<int> centrosCostoToAdd = (from centroCosto in centrosCosto
                                       join nullable in usuario.CentrosCosto on centroCosto equals nullable.CentroCostoId into defCentrosCosto
                                       from persited in defCentrosCosto.DefaultIfEmpty()
                                       where persited is null
                                       select centroCosto).ToList();

        List<int> centrosCostoToRemove = (from persisted in usuario.CentrosCosto
                                          join nullable in centrosCosto on persisted.CentroCostoId equals nullable into defCentrosCosto
                                          from centroCosto in defCentrosCosto.DefaultIfEmpty()
                                          where centroCosto == 0
                                          select persisted.CentroCostoId).ToList();

        result = await AddToCentrosCostoAsync(usuario, centrosCostoToAdd);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        result = await RemoveToCentrosCostoAsync(usuario, centrosCostoToRemove);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        List<int> franquiciasToAdd = (from franquicia in franquicias
                                      join nullable in usuario.Franquicias on franquicia equals nullable.FranquiciaId into defFranquicias
                                      from persited in defFranquicias.DefaultIfEmpty()
                                      where persited is null
                                      select franquicia).ToList();

        List<int> franquiciasToRemove = (from persisted in usuario.Franquicias
                                         join nullable in franquicias on persisted.FranquiciaId equals nullable into defFranquicias
                                         from centroCosto in defFranquicias.DefaultIfEmpty()
                                         where centroCosto == 0
                                         select persisted.FranquiciaId).ToList();

        result = await AddToFranquiciasAsync(usuario, franquiciasToAdd);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        result = await RemoveToFranquiciasAsync(usuario, franquiciasToRemove);

        if (!result.Succeeded)
        {
            return result.ToFailure();
        }

        rolesPersisted = await _rolRepository.FinRolesByUserAsync(usuario.Id, cancellationToken);
        IEnumerable<CentroCostoInfo> centrosCostoPersisted = await _centroCostoRepository.ListCentrosCostoAsync(usuario.CentrosCosto.Select(x => x.CentroCostoId).ToArray(), cancellationToken);
        IEnumerable<Franquicia> franquiciasPersisted = await _faranquiciaRepository.ListFranquiciasAsync(usuario.CentrosCosto.Select(x => x.CentroCostoId).ToArray(), cancellationToken);

        return usuario.ToInfo(rolesPersisted.ToList(), centrosCostoPersisted.ToList(), franquiciasPersisted.ToList());
    }

    private async Task<IdentityResult> AddToCentrosCostoAsync(Usuario user,  IEnumerable<int> centrosCosto)
    {
        user.AddCentrosCosto(centrosCosto);
        return await _userManager.UpdateAsync(user);
    }

    private async Task<IdentityResult> RemoveToCentrosCostoAsync(Usuario user, IEnumerable<int> centrosCosto)
    {
        user.RemoveCentrosCosto(centrosCosto);
        return await _userManager.UpdateAsync(user);
    }

    private async Task<IdentityResult> AddToFranquiciasAsync(Usuario user, IEnumerable<int> franquicias)
    {
        user.AddFranquicias(franquicias);
        return await _userManager.UpdateAsync(user);
    }

    private async Task<IdentityResult> RemoveToFranquiciasAsync(Usuario user, IEnumerable<int> franquicias)
    {
        user.RemoveFranquicias(franquicias);
        return await _userManager.UpdateAsync(user);
    }
}
