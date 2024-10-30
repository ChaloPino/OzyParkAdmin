using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using System.Security.Claims;
using OzyParkAdmin.Application.CategoriasProducto.Search;
using OzyParkAdmin.Shared;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto;

/// <summary>
/// Pagina del Mantenedor de Categoria Producto
/// </summary>
public partial class Index
{
    private ClaimsPrincipal? user;
    private MudDataGrid<CategoriaProductoViewModel> dataGrid = default!;
    private ObservableGridData<CategoriaProductoViewModel> currentCategoriasProducto = new();
    private string? searchText;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    private async Task<GridData<CategoriaProductoViewModel>> SearchCategoriaServiciosAsync(GridState<CategoriaProductoViewModel> state)
    {
        SearchCategoriaProducto searchServicios = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchServicios);
        result.Switch(
            onSuccess: categorias => currentCategoriasProducto = categorias.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar Categoria de Productos"));

        return currentCategoriasProducto;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }
    private async Task AddCategoriaProductoAsync()
    {
        throw new NotImplementedException();
    }
}
