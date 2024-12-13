using MassTransit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.Monedas.List;
using OzyParkAdmin.Application.Productos.Search;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Application.TarfiasProducto.Create;
using OzyParkAdmin.Application.TarfiasProducto.Update;
using OzyParkAdmin.Application.TiposDia.List;
using OzyParkAdmin.Application.TiposHorario.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasProductos.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto;

/// <summary>
/// Página de tarifas de productos.
/// </summary>
public sealed partial class Index : IDisposable, IAsyncDisposable
{
    private CancellationTokenSource? cancellationTokenSource;
    private ClaimsPrincipal? user;
    private MudDataGrid<TarifaProductoViewModel> dataGrid = default!;
    private string? searchText;
    private List<CentroCostoInfo> centrosCosto = [];
    private CentroCostoInfo centroCosto = new();
    private List<Moneda> monedas = [];
    private List<CanalVenta> canalesVenta = [];
    private List<TipoDia> tiposDia = [];
    private List<TipoHorario> tiposHorario = [];
    private ObservableGridData<TarifaProductoViewModel> currentTarifas = new();
    private bool openCreate;
    private bool openEditing;
    private TarifaProductoViewModel? currentTarifa;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;
        await LoadReferencesAsync();

        if (centrosCosto.Count > 0)
        {
            centroCosto = centrosCosto[0];
        }
    }

    private async Task LoadReferencesAsync()
    {
        Task[] taks =
        [
            OnLoadCentrosCostoAsync(),
            OnLoadMonedasAsync(),
            OnLoadCanalesVentaAsync(),
            OnLoadTiposDiaAsync(),
            OnLoadTiposHorarioAsync(),
        ];

        await Task.WhenAll(taks);
    }

    private async Task OnLoadCentrosCostoAsync()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user!));
        result.Switch(
            onSuccess: list => centrosCosto = list,
            onFailure: failure => AddFailure(failure, "cargar centros de costo"));
    }

    private async Task OnLoadCanalesVentaAsync()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
            onSuccess: list => canalesVenta = list,
            onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    private async Task OnLoadMonedasAsync()
    {
        var result = await Mediator.SendRequest(new ListMonedas());
        result.Switch(
            onSuccess: list => monedas = list,
            onFailure: failure => AddFailure(failure, "cargar monedas"));
    }

    private async Task OnLoadTiposDiaAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposDia());
        result.Switch(
            onSuccess: list => tiposDia = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de día"));
    }

    private async Task OnLoadTiposHorarioAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposHorario());
        result.Switch(
            onSuccess: list => tiposHorario = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de horario"));
    }

    private async Task<GridData<TarifaProductoViewModel>> SearchTarifasAsync(GridState<TarifaProductoViewModel> state)
    {
        if (cancellationTokenSource is not null)
        {
            await cancellationTokenSource.CancelAsync();
        }

#pragma warning disable S2930 // "IDisposables" should be disposed
        cancellationTokenSource = new();
#pragma warning restore S2930 // "IDisposables" should be disposed

        var searchTarifas = state.ToSearch(centroCosto, searchText);
        var result = await Mediator.SendRequestWithCancellation(searchTarifas, cancellationTokenSource.Token);

        result.Switch(
            onSuccess: tarifas => currentTarifas = tarifas.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar tarifas de productos"));

        return currentTarifas;
    }

    private Task AddTarifasAsync()
    {
        openCreate = true;
        return Task.CompletedTask;
    }

    private Task ShowEditingAsync(CellContext<TarifaProductoViewModel> context)
    {
        currentTarifa = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentTarifa is not null)
        {
            openEditing = true;
        }

        return Task.CompletedTask;
    }

    private async Task<IEnumerable<ProductoInfo>> OnSearchproductosAsync(CentroCostoInfo centroCosto, string? searchText, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendRequestWithCancellation(new SearchProductosByName(centroCosto.Id, searchText), cancellationToken);
        List<ProductoInfo> productos = [];

        result.Switch(
            onSuccess: list => productos = list,
            onFailure: failure => AddFailure(failure, "buscar productos"));

        return productos;
    }

    private async Task<bool> OnCreateTarifaProductoAsync(TarifaProductoCreateModel tarifaProducto)
    {
        CreateTarifasProducto create = tarifaProducto.ToCreate();
        var result = await Mediator.SendRequest(create);

        return await result.MatchAsync(
            onSuccess: ReloadDataGridAsync,
            onFailure: failure => AddFailure(failure, "crear tarifas de producto"));
    }

    private async Task<bool> OnSaveTarifaProductoAsync(TarifaProductoViewModel tarifaProducto)
    {
        UpdateTarifaProducto update = tarifaProducto.ToUpdate();
        var result = await Mediator.SendRequest(update);

        return result.Match(
            onSuccess: tarifa => Update(tarifaProducto, tarifa),
            onFailure: failure => AddFailure(failure, "actualizar la tarifa de producto"));
    }

    private bool Update(TarifaProductoViewModel viewModel, TarifaProductoFullInfo tarifa)
    {
        var persistent = currentTarifas.Find(viewModel);

        if (persistent is not null)
        {
            persistent.Update(tarifa);
            return true;
        }

        return false;
    }

    private async Task<bool> ReloadDataGridAsync(Success success, CancellationToken cancellationToken)
    {
        await dataGrid.ReloadServerData();
        return true;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        cancellationTokenSource?.Cancel();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (cancellationTokenSource is not null)
        {
            await cancellationTokenSource.CancelAsync();
        }
    }
}
