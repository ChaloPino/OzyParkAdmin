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
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Application.Franquicias.List;
using OzyParkAdmin.Application.CategoriasProducto.List;
using OzyParkAdmin.Domain.CategoriasProducto;
using Azure;
using OzyParkAdmin.Domain.Productos;
using DocumentFormat.OpenXml.EMMA;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto;

/// <summary>
/// Pagina del Mantenedor de Categoria Producto
/// </summary>
public partial class Index
{
    //TODO ver que pueda crear una categoria sin padre, es decir en el ComboBox de las Categprias debería tener un Text "Raiz" con Value: null
    private ClaimsPrincipal? user;
    private MudDataGrid<CategoriaProductoViewModel> dataGrid = default!;
    private ObservableGridData<CategoriaProductoViewModel> currentCategoriasProducto = new();
    private string? searchText;

    private List<FranquiciaInfo> franquicias = []; //Para mudSelect cuando se edite o cree nueva categoria

    private bool openEditing;
    private bool openCanalesVenta;
    private CategoriaProductoViewModel? currentCategoriaProducto;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;


    /// <summary>
    /// Esto es lo primero que se ejecuta cuando llega a esta pagina
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;

        //Carga de listado de Franquicias
        var result = await Mediator.SendRequest(new ListFranquicias(user!));
        result.Switch(
            onSuccess: list => franquicias = list,
            onFailure: failure => AddFailure(failure, "cargar franquicias"));
    }

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

    private Task ShowEditingAsync(CellContext<CategoriaProductoViewModel> context)
    {
        currentCategoriaProducto = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentCategoriaProducto is not null)
        {
            //TODO: por lo visto no es neceario cargar acá await LoadCategoriasAsync(currentCategoriaProducto.FranquiciaId);
            //Revisar esto mismo cuando se edita el producto
            openEditing = true;
        }
        return Task.CompletedTask;
    }

    private Task ShowCanalesVentaAsync(CellContext<CategoriaProductoViewModel> context)
    {
        currentCategoriaProducto = context.Item;

        if (currentCategoriaProducto is not null)
        {
            openCanalesVenta = true;
        }
        return Task.CompletedTask;
    }

    private async Task<bool> SaveCategoriaProductoAsync(CategoriaProductoViewModel categoriaProducto)
    {
        if (categoriaProducto.IsNew)
        {
            var response = await Mediator.SendRequest(categoriaProducto.ToCreate(user!));
            return response.Match(
                onSuccess: fullInfo => UpdateCategoriaProducto(categoriaProducto, fullInfo),
                onFailure: failure => AddFailure(failure, "Crear")
                );
        }
        else
        {
            var response = await Mediator.SendRequest(categoriaProducto.ToUpdate(user!));
            return response.Match(
                onSuccess: fullInfo => UpdateCategoriaProducto(categoriaProducto, fullInfo),
                onFailure: failure => AddFailure(failure, "Crear")
                );
        }
    }

    private bool UpdateCategoriaProducto(CategoriaProductoViewModel categoriaProducto, CategoriaProductoFullInfo categoriaProductoFullInfo)
    {
        bool isNew = categoriaProducto.IsNew;
        categoriaProducto.Save(categoriaProductoFullInfo);

        if (isNew)
        {
            currentCategoriasProducto?.Add(categoriaProducto);
            return true;
        }

        CategoriaProductoViewModel? persistent = currentCategoriasProducto?.Find(x => x.Id == categoriaProducto.Id);

        if (persistent is not null)
        {
            persistent.Update(categoriaProducto);
            return true;
        }

        return false;

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

    private async Task<List<CategoriaProductoInfo>> LoadCategoriasAsync(int franquiciaId)
    {
        var result = await Mediator.SendRequest(new ListCategoriasProducto(franquiciaId));
        return result.Match(
            onSuccess: list => list,
            onFailure: failure =>
            {
                AddFailure(failure, "cargar categorías de producto");
                return [];
            });
    }
}
