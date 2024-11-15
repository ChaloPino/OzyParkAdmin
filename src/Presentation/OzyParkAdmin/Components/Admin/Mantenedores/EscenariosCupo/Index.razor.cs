using MassTransit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Extensions;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.EscenariosCupo.Create;
using OzyParkAdmin.Application.EscenariosCupo.Search;
using OzyParkAdmin.Application.EscenariosCupo.Update;
using OzyParkAdmin.Application.Servicios.List;
using OzyParkAdmin.Application.Zonas.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo;

public partial class Index
{
    private bool loading;
    private ClaimsPrincipal? user;
    private MudDataGrid<EscenarioCupoModel> dataGrid = default!;
    private ObservableGridData<EscenarioCupoModel> currentEscenariosCupo = new();
    private HashSet<EscenarioCupoModel> escenariosCupoToDelete = [];
    private string? searchText;

    private List<CentroCostoInfo> centrosCosto = [];
    private List<ZonaInfo> zonas = [];
    private List<ServicioInfo> servicios = [];
    private EscenarioCupoModel? currentEscenarioCupo;

    private bool openCreating;
    private bool openEditing;
    private EscenarioCupoViewModel viewModel = new();

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
            LoadCentrosCosto(), LoadEscenariosCupo(), LoadServiciosAsync(1)
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

    private async Task LoadServiciosAsync(int franquiciaId)
    {
        var result = await Mediator.SendRequest(new ListServicios(franquiciaId));
        result.Switch(
            onSuccess: list => servicios = list,
            onFailure: failure => AddFailure(failure, "cargar servicios"));
    }

    private async Task LoadEscenariosCupo()
    {
        var result = await Mediator.SendRequest(new ListZonas());
        result.Switch(
         onSuccess: list => zonas = list,
         onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    private async Task<GridData<EscenarioCupoModel>> SearchEscenariosCupoAsync(GridState<EscenarioCupoModel> state)
    {
        SearchEscenariosCupo searchEscenariosCupos = state.ToSearch(user!, zonas.Select(z => z.Id).ToArray(), searchText);
        var result = await Mediator.SendRequest(searchEscenariosCupos);
        result.Switch(
            onSuccess: escenarios => currentEscenariosCupo = escenarios.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar escenarios cupo"));
        return currentEscenariosCupo;
    }

    private Task AddEscenarioCupo()
    {
        openCreating = true;
        return Task.CompletedTask;
    }

    private async Task<bool> SaveEscenarioCupoAsync(EscenarioCupoModel escenarioCupo)
    {
        CreateEscenarioCupo changeStatus = escenarioCupo.ToCreate();
        var result = await Mediator.SendRequest(changeStatus);
        return await result.MatchAsync(
           onSuccess: RefreshAsync,
           onFailure: (failure) => AddFailure(failure, "crear escenario cupo")
       );
    }

    private async Task<bool> UpdateEscenarioCupoAsync(EscenarioCupoModel escenarioCupo)
    {
        UpdateEscenarioCupo changeStatus = escenarioCupo.ToUpdate();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateEscenarioCupo(escenarioCupo, result, "modificar escenario cupo");
    }

    private async Task<bool> ChangeStatusAsync(EscenarioCupoModel escenarioCupo)
    {
        escenarioCupo.EsActivo = !escenarioCupo.EsActivo;
        return await UpdateEscenarioCupoAsync(escenarioCupo).ConfigureAwait(false); ;
    }

    private async Task<bool> ChangeControlaHolgura(EscenarioCupoModel escenarioCupo)
    {
        escenarioCupo.EsHoraInicio = !escenarioCupo.EsHoraInicio;
        return await UpdateEscenarioCupoAsync(escenarioCupo).ConfigureAwait(false);
    }

    private bool UpdateEscenarioCupo(EscenarioCupoModel model, ResultOf<EscenarioCupoFullInfo> result, string action)
    {
        return result.Match(
           onSuccess: (info) => UpdateEscenarioCupo(model, info),
           onFailure: (failure) => AddFailure(failure, action)
       );
    }

    private bool UpdateEscenarioCupo(EscenarioCupoModel model, EscenarioCupoFullInfo info)
    {
        model.Save(info);

        EscenarioCupoModel? persistent = currentEscenariosCupo?.Find(c => c.Id == model.Id);

        if (persistent is not null)
        {
            persistent.Update(model);
            return true;
        }

        return false;
    }

    private async Task DeleteEscenarioCupo()
    {
        bool res = await DialogService.ShowConfirmationDialogAsync("Confirmación", "¿Está seguro que desea eliminar los escenarios seleccionados?", "Sí", "No");

        if (res)
        {
            loading = true;

            var result = await Mediator.SendRequest(escenariosCupoToDelete.ToDelete());
            await result.SwitchAsync(
                    onSuccess: DeleteFechasExcluidasSelectedAsync,
                    onFailure: failure => AddFailure(failure, "eliminar fechas excluidas para cupos"));

            loading = false;
        }
    }

    private async Task DeleteFechasExcluidasSelectedAsync(Success success, CancellationToken cancellationToken)
    {
        escenariosCupoToDelete.Clear();
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

    private Task ShowEditingAsync(CellContext<EscenarioCupoModel> context)
    {
        var cloned = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (cloned is not null)
        {
            currentEscenarioCupo = cloned;
            openEditing = true;
        }

        return Task.CompletedTask;
    }
}
