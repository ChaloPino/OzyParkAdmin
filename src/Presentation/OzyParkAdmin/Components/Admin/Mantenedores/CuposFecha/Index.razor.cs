using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor.Extensions;
using MudBlazor;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.CuposFecha.Create;
using OzyParkAdmin.Application.CuposFecha.Delete;
using OzyParkAdmin.Application.CuposFecha.Search;
using OzyParkAdmin.Application.CuposFecha.Update;
using OzyParkAdmin.Application.DiasSemana.List;
using OzyParkAdmin.Application.EscenariosCupo.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Controls;
using OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha;

/// <summary>
/// Página del mantenedor de cupos por fecha.
/// </summary>
public partial class Index
{
    private ClaimsPrincipal? user;
    private MudDataGrid<CupoFechaViewModel> dataGrid = default!;
    private CalendarDialog calendario = default!;
    private ObservableGridData<CupoFechaViewModel> currentCuposFecha = new();
    private string? searchText;

    private List<EscenarioCupoInfo> escenariosCupo = [];
    private List<CanalVenta> canalesVenta = [];
    private List<DiaSemana> diasSemana = [];

    private bool openCreating;
    private bool openDeleting;
    private bool openBatchEditing;
    private bool openEditing;
    private bool openCalendario;
    private CupoFechaViewModel? currentCupoFecha;

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
            LoadEscenariosCupo(), LoadCanalesVenta(), LoadDiasSemana(),
    ];

        await Task.WhenAll(loadingTasks);
    }

    private async Task LoadEscenariosCupo()
    {
        var result = await Mediator.SendRequest(new ListEscenariosCupo(user!));
        result.Switch(
            onSuccess: list => escenariosCupo = list,
            onFailure: failure => AddFailure(failure, "cargar escenarios de cupo"));
    }

    private async Task LoadCanalesVenta()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
            onSuccess: list => canalesVenta = list,
            onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    private async Task LoadDiasSemana()
    {
        var result = await Mediator.SendRequest(new ListDiasSemana());
        result.Switch(
            onSuccess: list => diasSemana = list,
            onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    private async Task<GridData<CupoFechaViewModel>> SearchCuposAsync(GridState<CupoFechaViewModel> state)
    {
        SearchCuposFecha searchCupos = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchCupos);
        result.Switch(
            onSuccess: cuposPorFecha => currentCuposFecha = cuposPorFecha.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar cupos por fecha"));
        return currentCuposFecha;
    }

    private Task AddCupoFechaAsync()
    {
        openCreating = true;
        return Task.CompletedTask;
    }

    private Task ShowEditingAsync(CellContext<CupoFechaViewModel> context)
    {
        currentCupoFecha = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentCupoFecha is not null)
        {
            openEditing = true;
        }

        return Task.CompletedTask;
    }

    private Task EditCuposFechaAsync()
    {
        openBatchEditing = true;
        return Task.CompletedTask;
    }

    private async Task DeleteAsync(CellContext<CupoFechaViewModel> context)
    {
        bool res = await DialogService.ShowConfirmationDialogAsync("Confirmación", $"¿Está seguro que desea eliminar el cupo de la fecha '{context.Item.Fecha:dd-MM-yyyy}'?", "Sí", "No");

        if (res)
        {
            var result = await Mediator.SendRequest(new DeleteCupoFecha(context.Item.Id));

            await result.SwitchAsync(
                onSuccess: RefreshAsync,
                onFailure: failure => AddFailure(failure, "eliminar cupo por fecha"));
        }
    }

    private Task DeleteCuposFechaAsync()
    {
        openDeleting = true;
        return Task.CompletedTask;
    }

    private async Task<bool> DeleteCuposFechaAsync(CuposFechaDeleteModel cuposModel)
    {
        var result = await Mediator.SendRequest(cuposModel.ToDelete());
        return await result.MatchAsync(
                onSuccess: RefreshAsync,
                onFailure: failure => AddFailure(failure, "eliminar cupos por fecha"));
    }

    private async Task ShowCalendarioAsync()
    {
        await calendario.ShowAsync();
    }

    private async Task<bool> SaveCuposFechaAsync(CuposFechaModel cuposFecha)
    {
        CreateCuposFecha changeStatus = cuposFecha.ToCreate();
        var result = await Mediator.SendRequest(changeStatus);
        return await CreateCuposFechaAsync(result);
    }

    private async Task<bool> SaveEditCuposFechaAsync(CuposFechaEditModel cuposFecha)
    {
        UpdateCuposFecha changeStatus = cuposFecha.ToUpdate();
        var result = await Mediator.SendRequest(changeStatus);
        return await CreateCuposFechaAsync(result);
    }

    private async Task<bool> CreateCuposFechaAsync(SuccessOrFailure result)
    {
        return await result.MatchAsync(
           onSuccess: RefreshAsync,
           onFailure: (failure) => AddFailure(failure, "crear cupos")
       );
    }

    private async Task<bool> RefreshAsync(Success success, CancellationToken cancellationToken)
    {
        await dataGrid.ReloadServerData();
        return true;
    }

    private async Task<bool> SaveCupoFechaAsync(CupoFechaViewModel cupo)
    {
        UpdateCupoFecha changeStatus = cupo.ToUpdate();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateCupoFecha(cupo, result, "modificar cupo");
    }

    private bool UpdateCupoFecha(CupoFechaViewModel cupo, ResultOf<CupoFechaFullInfo> result, string action)
    {
        return result.Match(
           onSuccess: (info) => UpdateCupoFecha(cupo, info),
           onFailure: (failure) => AddFailure(failure, action)
       );
    }

    private bool UpdateCupoFecha(CupoFechaViewModel cupo, CupoFechaFullInfo info)
    {
        cupo.Save(info);

        CupoFechaViewModel? persistent = currentCuposFecha?.Find(c => c.Id == cupo.Id);

        if (persistent is not null)
        {
            persistent.Update(cupo);
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
