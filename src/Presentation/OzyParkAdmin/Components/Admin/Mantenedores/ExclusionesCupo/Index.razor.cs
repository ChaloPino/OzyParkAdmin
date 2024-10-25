using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor.Extensions;
using MudBlazor;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.ExclusionesCupo.Create;
using OzyParkAdmin.Application.ExclusionesCupo.Search;
using OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo;

/// <summary>
/// La página del mantenedor de exclusiones de cupo.
/// </summary>
public partial class Index
{
    private bool loading;
    private ClaimsPrincipal? user;
    private MudDataGrid<FechaExcluidaCupoViewModel> dataGrid = default!;
    private ObservableGridData<FechaExcluidaCupoViewModel> currentFechasExcluidas = new();
    private HashSet<FechaExcluidaCupoViewModel> fechasExcluidasToDelete = [];
    private string? searchText;

    private List<CentroCostoInfo> centrosCosto = [];
    private List<CanalVenta> canalesVenta = [];

    private bool openCreating;

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
            LoadCentrosCosto(), LoadCanalesVenta()
        ];

        await Task.WhenAll(loadingTasks);
    }

    private async Task LoadCentrosCosto()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user!));
        result.Switch(
            onSuccess: list => centrosCosto = list,
            onFailure: failure => AddFailure(failure, "cargar centros de costo"));
    }

    private async Task LoadCanalesVenta()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
         onSuccess: list => canalesVenta = list,
         onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    private async Task<GridData<FechaExcluidaCupoViewModel>> SearchFechasExcluidasAsync(GridState<FechaExcluidaCupoViewModel> state)
    {
        SearchFechasExcluidasCupo searchFechasExcluidas = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchFechasExcluidas);
        result.Switch(
            onSuccess: fechasExcluidas => currentFechasExcluidas = fechasExcluidas.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar fechas excluidas"));
        return currentFechasExcluidas;
    }

    private Task AddFechasExcluidasAsync()
    {
        openCreating = true;
        return Task.CompletedTask;
    }

    private async Task<bool> SaveFechasExcluidasAsync(FechasExcluidasCupoModel fechasExcluidasCupo)
    {
        CreateFechasExcluidasCupo changeStatus = fechasExcluidasCupo.ToCreate();
        var result = await Mediator.SendRequest(changeStatus);
        return await result.MatchAsync(
           onSuccess: RefreshAsync,
           onFailure: (failure) => AddFailure(failure, "crear fechas excluidas para los cupos")
       );
    }

    private async Task DeleteFechaExcluidasAsync()
    {
        bool res = await DialogService.ShowConfirmationDialogAsync("Confirmación", "¿Está seguro que desea eliminar las fechas excluidas para cupos seleccionadas?", "Sí", "No");

        if (res)
        {
            loading = true;

            var result = await Mediator.SendRequest(fechasExcluidasToDelete.ToDelete());
            await result.SwitchAsync(
                    onSuccess: DeleteFechasExcluidasSelectedAsync,
                    onFailure: failure => AddFailure(failure, "eliminar fechas excluidas para cupos"));

            loading = false;
        }
    }

    private async Task DeleteFechasExcluidasSelectedAsync(Success success, CancellationToken cancellationToken)
    {
        fechasExcluidasToDelete.Clear();
        await RefreshAsync(success, cancellationToken);
    }

    private async Task<bool> RefreshAsync(Success success, CancellationToken cancellationToken)
    {
        await dataGrid.ReloadServerData();
        return true;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }
}
