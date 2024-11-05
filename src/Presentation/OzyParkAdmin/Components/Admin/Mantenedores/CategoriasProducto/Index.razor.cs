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
using OzyParkAdmin.Application.Productos.Activar;
using OzyParkAdmin.Application.Productos.Desactivar;
using OzyParkAdmin.Application.Productos;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

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

    private bool openEditing;
    private CategoriaProductoViewModel? currentCategoriaProducto;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    private async Task<GridData<CategoriaProductoViewModel>> SearchCategoriaServiciosAsync(GridState<CategoriaProductoViewModel> state)
    {
        SearchCategoriaProducto searchServicios = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchServicios);
        result.Switch(
            onSuccess: categorias => currentCategoriasProducto = categorias.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar Categoria de Productos"));

        if (state.SortDefinitions.Any() && dataGrid is not null)
        {
            //Si aplicaron algún ordenamiento ya no tiene sentido que se agrupe, ya que practicamente quedan agrupados con un solo item.
            dataGrid.Groupable = false;
        }

        return currentCategoriasProducto;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }
    private async Task AddCategoriaProductoAsync()
    {
        currentCategoriaProducto = new CategoriaProductoViewModel() { IsNew = true };
        CellContext<CategoriaProductoViewModel> context = new(dataGrid, currentCategoriaProducto);
        await ShowEditingAsync(context);
    }

    private async Task ShowEditingAsync(CellContext<CategoriaProductoViewModel> context)
    {
        currentCategoriaProducto = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentCategoriaProducto is not null)
        {
            //await LoadCategoriasAsync(currentCategoriaProducto.FranquiciaId);
            openEditing = true;
        }
    }

    private async Task<bool> SaveCategoriaProductoAsync(CategoriaProductoViewModel categoriaProducto)
    {
        throw new NotImplementedException();
    }
    private async Task SaveEsActivoAsync(CategoriaProductoViewModel producto, bool esActivo)
    {
        throw new NotImplementedException();

        //>IProductoStateChangeable changeStatus = esActivo
        //>    ? new ActivarProducto(producto.Id)
        //>    : new DesactivarProducto(producto.Id);
        //>
        //>var result = await Mediator.SendRequest(changeStatus);
        //>UpdateProducto(producto, result, esActivo ? "activar producto" : "desactivar producto");
    }
}
