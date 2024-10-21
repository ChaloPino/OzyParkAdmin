using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Seguridad.Usuarios.Create;
using OzyParkAdmin.Application.Seguridad.Usuarios.Search;
using OzyParkAdmin.Application.Seguridad.Usuarios.Update;
using OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios.Models;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios.Models;

internal static class UsarioMappers
{
    public static SearchUsers ToSearchUsers(this GridState<UsuarioViewModel> state, ClaimsPrincipal user, string? searchText = null)
    {
        return new SearchUsers(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);
    }

    private static FilterExpressionCollection<Usuario> ToFilterExpressions(this GridState<UsuarioViewModel> state)
    {
        var filterExpressions = new FilterExpressionCollection<Usuario>();

        foreach (IFilterDefinition<UsuarioViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<UsuarioViewModel> filterDefinition, FilterExpressionCollection<Usuario> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(UsuarioViewModel.UserName) => filterExpressions.Add(x => x.UserName, filterDefinition.Operator!, filterDefinition.Value),
            nameof(UsuarioViewModel.FriendlyName) => filterExpressions.Add(x => x.FriendlyName, filterDefinition.Operator!, filterDefinition.Value),
            nameof(UsuarioViewModel.Rut) => filterExpressions.Add(x => x.Rut, filterDefinition.Operator!, filterDefinition.Value),
            nameof(UsuarioViewModel.Email) => filterExpressions.Add(x => x.Email, filterDefinition.Operator!, filterDefinition.Value),
            nameof(UsuarioViewModel.IsLockedout) => filterDefinition.CreateFilterExpressionForLockout(filterExpressions),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterExpressionCollection<Usuario> CreateFilterExpressionForLockout(this IFilterDefinition<UsuarioViewModel> filterDefinition, FilterExpressionCollection<Usuario> filterExpressions)
    {
        bool value = (bool)filterDefinition.Value!;

        return value
            ? filterExpressions.Add(x => x.LockoutEndDateUtc, FilterOperator.DateTime.NotEmpty, createIfNull: true)
            : filterExpressions.Add(x => x.LockoutEndDateUtc, FilterOperator.DateTime.Empty, createIfNull: true);
    }

    private static SortExpressionCollection<Usuario> ToSortExpressions(this GridState<UsuarioViewModel> state)
    {
        SortExpressionCollection<Usuario> sortExpressions = new();

        foreach (var sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<UsuarioViewModel> sortDefinition, SortExpressionCollection<Usuario> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(UsuarioViewModel.UserName) => sortExpressions.Add(x => x.UserName, sortDefinition.Descending),
            nameof(UsuarioViewModel.FriendlyName) => sortExpressions.Add(x => x.FriendlyName, sortDefinition.Descending),
            nameof(UsuarioViewModel.Rut) => sortExpressions.Add(x => x.Rut, sortDefinition.Descending),
            nameof(UsuarioViewModel.Email) => sortExpressions.Add(x => x.Email, sortDefinition.Descending),
            nameof(UsuarioViewModel.IsLockedout) => sortExpressions.Add(x => x.LockoutEndDateUtc, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<UsuarioViewModel> ToGridData(this PagedList<UsuarioFullInfo> pagedList, IMudStateHasChanged stateHasChanged)
    {
        return new ObservableGridData<UsuarioViewModel, Guid>(pagedList.Items.Select(ToViewModel), pagedList.TotalCount, stateHasChanged, (user) => user.Id);
    }

    private static UsuarioViewModel ToViewModel(UsuarioFullInfo usuario) =>
        new()
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            FriendlyName = usuario.FriendlyName,
            Rut = usuario.Rut,
            Email = usuario.Email,
            IsLockedout = usuario.IsLockedout,
            Roles = usuario.Roles.ToModel(),
            CentrosCosto = usuario.CentrosCosto,
            Franquicias = usuario.Franquicias,
        };

    public static List<UsuarioRolModel> ToModel(this IEnumerable<Rol> roles) =>
        roles.Select(ToModel).ToList();

    private static UsuarioRolModel ToModel(Rol rol) =>
        new() { Id = rol.Id, Nombre = rol.Name };


    public static CreateUser ToCreate(this UsuarioViewModel model) =>
        new(model.UserName, model.FriendlyName, model.Rut, model.Email, [.. model.Roles.Select(x => x.Nombre)], [.. model.CentrosCosto.Select(x => x.Id)], [.. model.Franquicias.Select(x => x.Id)]);

    public static UpdateUser ToUpdate(this UsuarioViewModel model) =>
        new(model.Id, model.UserName, model.FriendlyName, model.Rut, model.Email, [.. model.Roles.Select(x => x.Nombre)], [.. model.CentrosCosto.Select(x => x.Id)], [.. model.Franquicias.Select(x => x.Id)]);
}
