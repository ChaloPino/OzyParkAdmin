using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.Franquicias.List;
using OzyParkAdmin.Application.Seguridad.Roles.List;
using OzyParkAdmin.Application.Seguridad.Usuarios.Lock;
using OzyParkAdmin.Application.Seguridad.Usuarios.Search;
using OzyParkAdmin.Application.Seguridad.Usuarios.Unlock;
using OzyParkAdmin.Application.Seguridad.Usuarios;
using OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios;

/// <summary>
/// Página del mantenedor de usuarios.
/// </summary>
public partial class Index
{
    private ClaimsPrincipal? user;
    private MudDataGrid<UsuarioViewModel> dataGrid = default!;
    private string? searchText;
    private List<FranquiciaInfo> franquicias = [];
    private List<CentroCostoInfo> centrosCosto = [];
    private List<UsuarioRolModel> roles = [];
    private ObservableGridData<UsuarioViewModel> currentUsers = new();

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;
        await LoadReferencesAsync();
    }

    private async Task LoadReferencesAsync()
    {
        Task[] loadingTasks = [LoadRolesAsync(), LoadCentrosCostoAsync(), LoadFranquiciasAsync()];
        await Task.WhenAll(loadingTasks);
    }

    private async Task LoadRolesAsync()
    {
        var result = await Mediator.SendRequest(new ListRoles(user!));
        result.Switch(
            onSuccess: list => roles = list.ToModel(),
            onFailure: failure => Snackbar.AddFailure(failure, "cargar roles"));
    }

    private async Task LoadCentrosCostoAsync()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user!));
        result.Switch(
             onSuccess: list => centrosCosto = list,
             onFailure: failure => Snackbar.AddFailure(failure, "cargar centros de costo"));
    }

    private async Task LoadFranquiciasAsync()
    {
        var result = await Mediator.SendRequest(new ListFranquicias(user!));
        result.Switch(
             onSuccess: list => franquicias = list,
             onFailure: failure => Snackbar.AddFailure(failure, "cargar franquicias"));
    }

    private async Task<GridData<UsuarioViewModel>> SearchUsuariosAsync(GridState<UsuarioViewModel> state)
    {
        SearchUsers searchUsers = state.ToSearchUsers(user!, searchText);
        var result = await Mediator.SendRequest(searchUsers);
        result.Switch(
            onSuccess: usuarios => currentUsers = usuarios.ToGridData(dataGrid),
            onFailure: failure => Snackbar.AddFailure(failure, "buscar usuarios"));
        return currentUsers;
    }

    private async Task AddUsuarioAsync()
    {
        UsuarioViewModel newUsuario = new() { IsNew = true };
        currentUsers?.Add(newUsuario);
        await new CellContext<UsuarioViewModel>(dataGrid, newUsuario).Actions.StartEditingItemAsync();
    }

    private void CancelEditing(UsuarioViewModel usuario)
    {
        if (usuario.IsNew)
        {
            currentUsers?.Remove(usuario);
        }
    }

    private async Task SaveUsuarioAsync(UsuarioViewModel usuario)
    {
        IUserChangeable changeStatus = usuario.IsNew
            ? usuario.ToCreate()
            : usuario.ToUpdate();

        var result = await Mediator.SendRequest(changeStatus);
        UpdateUsuario(usuario, result, usuario.IsNew ? "crear usuario" : "actualizar usuario");
    }

    private async Task ChangeStatusAsync(CellContext<UsuarioViewModel> context, bool lockout)
    {
        var usuario = context.Item;
        IUserChangeable changeStatus = lockout
            ? new LockUser(usuario.Id)
            : new UnlockUser(usuario.Id);

        var result = await Mediator.SendRequest(changeStatus);
        UpdateUsuario(usuario, result, lockout ? "bloquear usuario" : "desbloquear usuario");
    }

    private void UpdateUsuario(UsuarioViewModel currentUser, ResultOf<UsuarioFullInfo> result, string action)
    {
        result.Switch(
            onSuccess: (usuario) => UpdateUsuario(currentUser, usuario),
            onFailure: (failure) => Snackbar.AddFailure(failure, action)
        );
    }

    private static void UpdateUsuario(UsuarioViewModel currentUser, UsuarioFullInfo usuario)
    {
        if (currentUser.IsNew)
        {
            currentUser.IsNew = false;
            currentUser.Id = usuario.Id;
            currentUser.UserName = usuario.UserName;
        }

        currentUser.FriendlyName = usuario.FriendlyName;
        currentUser.Email = usuario.Email;
        currentUser.IsLockedout = usuario.IsLockedout;
        currentUser.Roles = usuario.Roles.ToModel();
        currentUser.CentrosCosto = usuario.CentrosCosto;
        currentUser.Franquicias = usuario.Franquicias;
    }
}
