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
using OzyParkAdmin.Application.Shared;
using Azure;
using OzyParkAdmin.Domain.Productos;
using DocumentFormat.OpenXml.EMMA;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Application.Productos.Find;
using OzyParkAdmin.Application.CategoriasProducto.Find;
using OzyParkAdmin.Application.CategoriasProducto.Assign;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto;

/// <summary>
/// Pagina del Mantenedor de Categoria Producto
/// </summary>
public sealed partial class Index : IDisposable, IAsyncDisposable
{
    //TODO ver que pueda crear una categoria sin padre, es decir en el ComboBox de las Categprias debería tener un Text "Raiz" con Value: null
    private ClaimsPrincipal? user;
    private MudDataGrid<CategoriaProductoViewModel> dataGrid = default!;
    private ObservableGridData<CategoriaProductoViewModel> currentCategoriasProducto = new();
    private string? searchText;

    private List<FranquiciaInfo> franquicias = []; //Para mudSelect cuando se edite o cree nueva categoria
    private List<CanalVenta> canalesVenta = []; //Para mudSelect cuando se agrega Canales de Venta

    private bool openEditing;
    private bool openCanalesVenta;
    private bool groupable = true;

    private CategoriaProductoViewModel? currentCategoriaProducto;
    private CancellationTokenSource? _cancellationTokenSource;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;


    /// <summary>
    /// Esto es lo primero que se ejecuta cuando llega a esta pagina
    /// </summary>
    /// <returns>Una tarea que contiene una operación asíncrona.</returns>
    protected override async Task OnInitializedAsync()
    {
        Task[] loadingTasks = [LoadFranquiciasAsync(), LoadCanalesVentaAsync()];

        await Task.WhenAll(loadingTasks);

        //user = (await AuthenticationState).User;

        ////Carga de listado de Franquicias
        //var result = await Mediator.SendRequest(new ListFranquicias(user!));
        //result.Switch(
        //onSuccess: list => franquicias = list,
        //onFailure: failure => AddFailure(failure, "cargar franquicias"));
    }

    private async Task LoadCanalesVentaAsync()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
            onSuccess: list => canalesVenta = list,
            onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    private async Task LoadFranquiciasAsync()
    {
        user = (await AuthenticationState).User;

        //Carga de listado de Franquicias
        var result = await Mediator.SendRequest(new ListFranquicias(user!));
        result.Switch(
            onSuccess: list => franquicias = list,
            onFailure: failure => AddFailure(failure, "cargar franquicias"));
    }

    private async Task LoadCategoriaProductoDetalleAsync(CategoriaProductoViewModel categoriaProducto)
    {
        ResultOf<CategoriaProductoFullInfo> result = await Mediator.SendRequest(new FindCategoriaProducto(categoriaProducto.Id));
        result.Switch(
            onSuccess: info => LoadCategoriaProductoInfo(categoriaProducto, info),
            onFailure: failure => AddFailure(failure, "cargar el detalle del producto"));

    }

    private static void LoadCategoriaProductoInfo(CategoriaProductoViewModel categoriaProducto, CategoriaProductoFullInfo categoriaProductoFullInfo)
    {
        //ni idea porque en productos ocupa algo así categoriaProducto.DetailLoaded = true; ,pero no veo que se ocupe en otro lado
        categoriaProducto.CanalesVenta = [.. categoriaProductoFullInfo.CanalesVenta];
    }

    private async Task<GridData<CategoriaProductoViewModel>> SearchCategoriaServiciosAsync(GridState<CategoriaProductoViewModel> state)
    {
        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
        }

#pragma warning disable S2930 // "IDisposables" should be disposed
        _cancellationTokenSource = new CancellationTokenSource();
#pragma warning restore S2930 // "IDisposables" should be disposed

        SearchCategoriaProducto searchServicios = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequestWithCancellation(searchServicios, _cancellationTokenSource.Token);
        result.Switch(
            onSuccess: categorias => currentCategoriasProducto = categorias.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar Categoria de Productos"));

        if (state.SortDefinitions.Any() && dataGrid is not null)
        {
            //Si aplicaron algún ordenamiento ya no tiene sentido que se agrupe, ya que practicamente quedan agrupados con un solo item.
            groupable = false;
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
        DialogOptions.MaxWidth = MaxWidth.ExtraLarge;
        currentCategoriaProducto = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentCategoriaProducto is not null)
        {
            openEditing = true;
        }
        return Task.CompletedTask;
    }

    private Task ShowCanalesVentaAsync(CellContext<CategoriaProductoViewModel> context)
    {
        DialogOptions.MaxWidth = MaxWidth.Medium;
        currentCategoriaProducto = context.Item;

        if (currentCategoriaProducto is not null)
        {
            openCanalesVenta = true;
        }
        return Task.CompletedTask;
    }

    private async Task<bool> SaveCategoriaProductoAsync(CategoriaProductoViewModel categoriaProducto)
    {
        //TODO Cuando se edita una segunda vez la Categoria Padre se pierde de alguna manera.
        //TODO relacionado a lo anterior: No debería aparecer su propia categoria en la selección padre
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
                onFailure: failure => AddFailure(failure, "Modificar")
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
