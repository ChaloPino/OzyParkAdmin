using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.Cajas.List;
using OzyParkAdmin.Application.CategoriasProducto.List;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.Contabilidad.List;
using OzyParkAdmin.Application.Franquicias.List;
using OzyParkAdmin.Application.Productos.Activar;
using OzyParkAdmin.Application.Productos.Assign;
using OzyParkAdmin.Application.Productos.Desactivar;
using OzyParkAdmin.Application.Productos.Find;
using OzyParkAdmin.Application.Productos.List;
using OzyParkAdmin.Application.Productos.Lock;
using OzyParkAdmin.Application.Productos.Search;
using OzyParkAdmin.Application.Productos.Unlock;
using OzyParkAdmin.Application.Productos;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos;

/// <summary>
/// Página del mantenedor de productos.
/// </summary>
public partial class Index
{
    private ClaimsPrincipal? user;
    private MudDataGrid<ProductoViewModel> dataGrid = default!;
    private ObservableGridData<ProductoViewModel> currentProductos = new();
    private string? searchText;

    private List<CentroCostoInfo> centrosCosto = [];
    private List<FranquiciaInfo> franquicias = [];
    private List<CajaInfo> cajas = [];
    private List<TipoProducto> tiposProducto = [];
    private List<AgrupacionContable> familias = [];

    private bool openEditing;
    private bool openCajas;
    private bool openPartes;
    private ProductoViewModel? currentProducto;

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
        Task[] loadingTasks =
        [
            LoadCentrosCostoAsync(), LoadFranquiciasAsync(), LoadCajasAsync(),
            LoadAgrupacionesContablesAsync(), LoadTiposProductoAsync(),
        ];

        await Task.WhenAll(loadingTasks);
    }

    private async Task LoadCentrosCostoAsync()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user!));
        result.Switch(
            onSuccess: list => centrosCosto = list,
            onFailure: failure => AddFailure(failure, "cargar centros de costo"));
    }

    private async Task LoadFranquiciasAsync()
    {
        var result = await Mediator.SendRequest(new ListFranquicias(user!));
        result.Switch(
            onSuccess: list => franquicias = list,
            onFailure: failure => AddFailure(failure, "cargar franquicias"));
    }

    private async Task LoadCajasAsync()
    {
        var result = await Mediator.SendRequest(new ListCajas(user!));
        result.Switch(
            onSuccess: list => cajas = list,
            onFailure: failure => AddFailure(failure, "cargar cajas"));
    }

    private async Task LoadAgrupacionesContablesAsync()
    {
        var result = await Mediator.SendRequest(new ListAgrupacionesContables());
        result.Switch(
            onSuccess: list => familias = list,
            onFailure: failure => AddFailure(failure, "cargar agrupaciones contables"));
    }

    private async Task LoadTiposProductoAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposProducto());
        result.Switch(
            onSuccess: list => tiposProducto = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de producto"));
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

    private async Task<List<ProductoInfo>> LoadComplementosAsync(int categoriaId, int exceptoProductoId)
    {
        var result = await Mediator.SendRequest(new ListComplementos(categoriaId, exceptoProductoId));
        return result.Match(
            onSuccess: list => list,
            onFailure: failure =>
            {
                AddFailure(failure, "cargar complementos de productos");
                return [];
            });
    }

    private async Task<List<ProductoInfo>> LoadProductosParteAsync(int franquiciaId, int exceptoProductoId)
    {
        var result = await Mediator.SendRequest(new ListProductosParaPartes(franquiciaId, exceptoProductoId));
        return result.Match(
            onSuccess: list => list,
            onFailure: failure =>
            {
                AddFailure(failure, "cargar partes de productos");
                return [];
            });
    }

    private async Task<GridData<ProductoViewModel>> SearchProductosAsync(GridState<ProductoViewModel> state)
    {
        SearchProductos searchProductos = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchProductos);
        result.Switch(
            onSuccess: productos => currentProductos = productos.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar productos"));
        return currentProductos;
    }

    private async Task AddProductoAsync()
    {
        currentProducto = new() { IsNew = true, };
        CellContext<ProductoViewModel> context = new(dataGrid, currentProducto);
        await ShowEditingAsync(context);
    }

    private async Task ShowEditingAsync(CellContext<ProductoViewModel> context)
    {
        currentProducto = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentProducto is not null)
        {
            await LoadCategoriasAsync(currentProducto.FranquiciaId);
            openEditing = true;
        }
    }

    private Task ShowCajasAsync(CellContext<ProductoViewModel> context)
    {
        currentProducto = context.Item;
        openCajas = true;
        return Task.CompletedTask;
    }

    private Task ShowPartesAsync(CellContext<ProductoViewModel> context)
    {
        currentProducto = context.Item;
        openPartes = true;
        return Task.CompletedTask;
    }

    private async Task LoadProductoDetailAsync(ProductoViewModel producto)
    {
        ResultOf<ProductoFullInfo> result = await Mediator.SendRequest(new FindProducto(producto.Id));

        result.Switch(
                onSuccess: info => LoadProductoInfo(producto, info),
                onFailure: failure => AddFailure(failure, "cargar el detalle del producto"));
    }

    private static void LoadProductoInfo(ProductoViewModel producto, ProductoFullInfo info)
    {
        producto.DetailLoaded = true;
        producto.Cajas = [.. info.Cajas];
        producto.Partes = info.Partes.ToModel();
    }

    private async Task<bool> SaveProductoAsync(ProductoViewModel producto)
    {
        IProductoStateChangeable changeStatus = producto.IsNew
            ? producto.ToCreate(user!)
            : producto.ToUpdate(user!);

        var result = await Mediator.SendRequest(changeStatus);
        return UpdateProducto(producto, result, producto.IsNew ? "crear producto" : "modificar producto");
    }

    private async Task SaveEsActivoAsync(ProductoViewModel producto, bool esActivo)
    {
        IProductoStateChangeable changeStatus = esActivo
            ? new ActivarProducto(producto.Id)
            : new DesactivarProducto(producto.Id);

        var result = await Mediator.SendRequest(changeStatus);
        UpdateProducto(producto, result, esActivo ? "activar producto" : "desactivar producto");
    }

    private async Task SaveEnInventarioAsync(ProductoViewModel producto, bool enInventario)
    {
        IProductoStateChangeable changeStatus = enInventario
            ? new LockProducto(producto.Id)
            : new UnlockProducto(producto.Id);

        var result = await Mediator.SendRequest(changeStatus);
        UpdateProducto(producto, result, enInventario ? "bloquear producto" : "desbloquear producto");
    }

    private async Task<bool> SaveCajasAsync(ProductoViewModel producto)
    {
        IProductoStateChangeable changeStatus = new AssignCajasToProducto(producto.Id, [.. producto.Cajas]);
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateProducto(producto, result, "asignar cajas");
    }

    private async Task<bool> SavePartesAsync(ProductoViewModel producto)
    {
        IProductoStateChangeable changeStatus = new AssignPartesToProducto(producto.Id, [.. producto.Partes.ToInfo()]);
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateProducto(producto, result, "asignar partes");
    }

    private bool UpdateProducto(ProductoViewModel producto, ResultOf<ProductoFullInfo> result, string action)
    {
        return result.Match(
            onSuccess: info => UpdateProducto(producto, info),
            onFailure: failure => AddFailure(failure, action));
    }

    private bool UpdateProducto(ProductoViewModel producto, ProductoFullInfo info)
    {
        bool isNew = producto.IsNew;
        producto.Save(info);

        if (isNew)
        {
            currentProductos?.Add(producto);
            return true;
        }

        ProductoViewModel? persistent = currentProductos?.Find(x => x.Id == producto.Id);

        if (persistent is not null)
        {
            persistent.Update(producto);
            return true;
        }

        return false;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }
}
