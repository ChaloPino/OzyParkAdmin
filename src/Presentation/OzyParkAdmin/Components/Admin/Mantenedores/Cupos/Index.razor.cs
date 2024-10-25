using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.Cupos.Create;
using OzyParkAdmin.Application.Cupos.Search;
using OzyParkAdmin.Application.Cupos.Update;
using OzyParkAdmin.Application.DiasSemana.List;
using OzyParkAdmin.Application.EscenariosCupo.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Controls;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos;

/// <summary>
/// La página de mantención de cupos.
/// </summary>
public partial class Index
{
    private ClaimsPrincipal? user;
    private MudDataGrid<CupoViewModel> dataGrid = default!;
    private CalendarDialog calendario = default!;
    private ObservableGridData<CupoViewModel> currentCupos = new();
    private string? searchText;

    private List<EscenarioCupoInfo> escenariosCupo = [];
    private List<CanalVenta> canalesVenta = [];
    private List<DiaSemana> diasSemana = [];

    private bool openCreating;
    private bool openEditing;
    private bool openCalendario;
    private CupoViewModel? currentCupo;

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
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los escenarios de cupo"));
    }

    private async Task LoadCanalesVenta()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
            onSuccess: list => canalesVenta = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los canales de venta"));
    }

    private async Task LoadDiasSemana()
    {
        var result = await Mediator.SendRequest(new ListDiasSemana());
        result.Switch(
            onSuccess: list => diasSemana = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los días de semana"));
    }

    private async Task<GridData<CupoViewModel>> SearchCuposAsync(GridState<CupoViewModel> state)
    {
        SearchCupos searchCupos = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchCupos);
        result.Switch(
            onSuccess: cupos => currentCupos = cupos.ToGridData(dataGrid),
            onFailure: failure => Snackbar.AddFailure(failure, "buscar cupos"));
        return currentCupos;
    }

    private Task AddCupoAsync()
    {
        openCreating = true;
        return Task.CompletedTask;
    }

    private Task ShowEditingAsync(CellContext<CupoViewModel> context)
    {
        currentCupo = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (currentCupo is not null)
        {
            openEditing = true;
        }

        return Task.CompletedTask;
    }

    private async Task ShowCalendarioAsync()
    {
        await calendario.ShowAsync();
    }

    private async Task<bool> SaveCuposAsync(CuposModel cupos)
    {
        CreateCupos changeStatus = cupos.ToCreate();
        var result = await Mediator.SendRequest(changeStatus);
        return CreateCupos(result);
    }

    private bool CreateCupos(ResultOf<List<CupoFullInfo>> result)
    {
        return result.Match(
           onSuccess: (cupos) => CreateCupos(cupos),
           onFailure: (failure) => AddFailure(failure, "crear cupos")
       );
    }

    private bool CreateCupos(List<CupoFullInfo> cupos)
    {
        foreach (var cupo in cupos)
        {
            currentCupos?.Add(cupo.ToViewModel());
        }

        return true;
    }

    private async Task<bool> SaveCupoAsync(CupoViewModel cupo)
    {
        UpdateCupo changeStatus = cupo.ToUpdate();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateCupo(cupo, result, "modificar cupo");
    }

    private bool UpdateCupo(CupoViewModel cupo, ResultOf<CupoFullInfo> result, string action)
    {
        return result.Match(
           onSuccess: (info) => UpdateCupo(cupo, info),
           onFailure: (failure) => AddFailure(failure, action)
       );
    }

    private bool UpdateCupo(CupoViewModel cupo, CupoFullInfo info)
    {
        cupo.Save(info);

        CupoViewModel? persistent = currentCupos?.Find(c => c.Id == cupo.Id);

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
