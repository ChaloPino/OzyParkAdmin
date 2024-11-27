using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;
using MassTransit;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.TiposDia.List;
using OzyParkAdmin.Application.Monedas.List;
using OzyParkAdmin.Application.TiposHorario.List;
using OzyParkAdmin.Application.TiposSegmentacion.List;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Application.Servicios.Search;
using OzyParkAdmin.Application.TarfiasServicio.Create;
using Org.BouncyCastle.X509;
using Microsoft.AspNetCore.Http.HttpResults;
using OzyParkAdmin.Application.TarfiasServicio.Update;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio;

/// <summary>
/// Página de tarifas de servicios.
/// </summary>
public sealed partial class Index : IDisposable, IAsyncDisposable
{
    private CancellationTokenSource? cancellationTokenSource;
    private ClaimsPrincipal? user;
    private MudDataGrid<TarifaServicioViewModel> dataGrid = default!;
    private string? searchText;
    private List<CentroCostoInfo> centrosCosto = [];
    private CentroCostoInfo centroCosto = new();
    private List<Moneda> monedas = [];
    private List<CanalVenta> canalesVenta = [];
    private List<TipoDia> tiposDia = [];
    private List<TipoHorario> tiposHorario = [];
    private List<TipoSegmentacion> tiposSegmentacion = [];
    private ObservableGridData<TarifaServicioViewModel> currentTarifas = new();
    private bool openCreate;
    private bool openEditing;
    private TarifaServicioViewModel? currentTarifa;

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
            OnLoadTiposSegmentacionAsync(),
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

    private async Task OnLoadTiposSegmentacionAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposSegmentacion());
        result.Switch(
            onSuccess: list => tiposSegmentacion = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de segmentación"));
    }

    private async Task<GridData<TarifaServicioViewModel>> SearchTarifasAsync(GridState<TarifaServicioViewModel> state)
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
            onFailure: failure => AddFailure(failure, "buscar tarifas de servicios"));

        return currentTarifas;
    }

    private Task AddTarifasAsync()
    {
        openCreate = true;
        return Task.CompletedTask;
    }

    private Task ShowEditingAsync(CellContext<TarifaServicioViewModel> context)
    {
        currentTarifa = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentTarifa is not null)
        {
            openEditing = true;
        }

        return Task.CompletedTask;
    }

    private async Task<IEnumerable<ServicioWithDetailInfo>> OnSearchServiciosAsync(CentroCostoInfo centroCosto, string? searchText, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendRequestWithCancellation(new SearchServiciosByName(centroCosto.Id, searchText), cancellationToken);
        List<ServicioWithDetailInfo> servicios = [];

        result.Switch(
            onSuccess: list => servicios = list,
            onFailure: failure => AddFailure(failure, "buscar servicios"));

        return servicios;
    }

    private async Task<bool> OnCreateTarifaServicioAsync(TarifaServicioCreateModel tarifaServicio)
    {
        CreateTarifasServicio create = tarifaServicio.ToCreate();
        var result = await Mediator.SendRequest(create);

        return await result.MatchAsync(
            onSuccess: ReloadDataGridAsync,
            onFailure: failure => AddFailure(failure, "crear tarifas de servicio"));
    }

    private async Task<bool> OnSaveTarifaServicioAsync(TarifaServicioViewModel tarifaServicio)
    {
        UpdateTarifaServicio update = tarifaServicio.ToUpdate();
        var result = await Mediator.SendRequest(update);

        return result.Match(
            onSuccess: tarifa => Update(tarifaServicio, tarifa),
            onFailure: failure => AddFailure(failure, "actualizar la tarifa de servicio"));
    }

    private bool Update(TarifaServicioViewModel viewModel, TarifaServicioFullInfo tarifa)
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
