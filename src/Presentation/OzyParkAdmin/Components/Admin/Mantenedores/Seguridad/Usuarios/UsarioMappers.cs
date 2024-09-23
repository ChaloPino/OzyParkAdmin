using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Seguridad.Usuarios.Create;
using OzyParkAdmin.Application.Seguridad.Usuarios.Search;
using OzyParkAdmin.Application.Seguridad.Usuarios.Update;
using OzyParkAdmin.Components.Admin.Shared;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios;

internal static class UsarioMappers
{
    public static SearchUsers ToSearchUsers(this GridState<UsuarioViewModel> state, ClaimsPrincipal user, string? searchText = null)
    {
        return new SearchUsers(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);
    }

    private static FilterExpressionCollection<Usuario> ToFilterExpressions(this GridState<UsuarioViewModel> state)
    {
        IEnumerable<IFilterExpression<Usuario>> filterExpressions = state.FilterDefinitions.Select(ToFilterExpression);
        return new(filterExpressions);
    }

    private static IFilterExpression<Usuario> ToFilterExpression(IFilterDefinition<UsuarioViewModel> filterDefinition)
    {
        return filterDefinition.Column!.PropertyName switch
        {
            nameof(UsuarioViewModel.UserName) => new StringFilterExpression<Usuario>(x => x.UserName, filterDefinition.Operator!, (string?)filterDefinition.Value),
            nameof(UsuarioViewModel.FriendlyName) => new StringFilterExpression<Usuario>(x => x.FriendlyName, filterDefinition.Operator!, (string?)filterDefinition.Value),
            nameof(UsuarioViewModel.Rut) => new StringFilterExpression<Usuario>(x => x.Rut, filterDefinition.Operator!, (string?)filterDefinition.Value),
            nameof(UsuarioViewModel.Email) => new StringFilterExpression<Usuario>(x => x.Email, filterDefinition.Operator!, (string?)filterDefinition.Value),
            nameof(UsuarioViewModel.IsLockedout) => CreateFilterExpressionForLockout(filterDefinition),
            _ => throw new UnreachableException(),
        };
    }

    private static DateAndTimeFilterExpression<Usuario, DateTime> CreateFilterExpressionForLockout(IFilterDefinition<UsuarioViewModel> filterDefinition)
    {
        bool value = (bool)filterDefinition.Value!;
        return value
            ? new DateAndTimeFilterExpression<Usuario, DateTime>(x => x.LockoutEndDateUtc, FilterOperator.DateTime.NotEmpty)
            : new DateAndTimeFilterExpression<Usuario, DateTime>(x => x.LockoutEndDateUtc, FilterOperator.DateTime.Empty);
    }

    private static SortExpressionCollection<Usuario> ToSortExpressions(this GridState<UsuarioViewModel> state)
    {
        IEnumerable<ISortExpression<Usuario>> sortExpressions = state.SortDefinitions.Select(ToSortExpression);
        return new(sortExpressions);
    }

    private static ISortExpression<Usuario> ToSortExpression(SortDefinition<UsuarioViewModel> sortDefinition)
    {
        return sortDefinition.SortBy switch
        {
            nameof(UsuarioViewModel.UserName) => new SortExpression<Usuario, string>(x => x.UserName, sortDefinition.Descending),
            nameof(UsuarioViewModel.FriendlyName) => new SortExpression<Usuario, string>(x => x.FriendlyName, sortDefinition.Descending),
            nameof(UsuarioViewModel.Rut) => new SortExpression<Usuario, string>(x => x.Rut, sortDefinition.Descending),
            nameof(UsuarioViewModel.Email) => new SortExpression<Usuario, string>(x => x.Email, sortDefinition.Descending),
            nameof(UsuarioViewModel.IsLockedout) => new SortExpression<Usuario, DateTime?>(x => x.LockoutEndDateUtc, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<UsuarioViewModel> ToGridData(this PagedList<UsuarioInfo> pagedList, IMudStateHasChanged stateHasChanged)
    {
        return new ObservableGridData<UsuarioViewModel, Guid>(pagedList.Items.Select(ToViewModel), pagedList.TotalCount, stateHasChanged, (user) => user.Id);
    }

    private static UsuarioViewModel ToViewModel(UsuarioInfo usuario) =>
        new()
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            FriendlyName = usuario.FriendlyName,
            Rut = usuario.Rut,
            Email = usuario.Email,
            IsLockedout = usuario.IsLockedout,
            Roles = usuario.Roles.ToModel(),
            CentrosCosto = usuario.CentrosCosto.ToModel(),
            Franquicias = usuario.Franquicias.ToModel(),
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
