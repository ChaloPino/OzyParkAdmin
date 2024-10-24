using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.EscenariosCupo.List;
using OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo;

/// <summary>
/// La página del mantenedor de omisiones de cupo.
/// </summary>
public partial class Index
{
    private MudDataGrid<IgnoraEscenarioCupoExclusionViewModel> dataGrid = default!;
    private HashSet<IgnoraEscenarioCupoExclusionViewModel> omisionesToDelete = [];
    private bool loading;
    private string? searchText;
    private bool createOpen;
    private List<CanalVenta> canalesVenta = [];
    private List<EscenarioCupoInfo> escenariosCupo = [];
    private ClaimsPrincipal? user;

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
            LoadEscenariosCupo(), LoadCanalesVenta()
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

    private async Task<GridData<IgnoraEscenarioCupoExclusionViewModel>> SearchOmisionesAsync(GridState<IgnoraEscenarioCupoExclusionViewModel> state)
    {
        var result = await Mediator.SendRequest(state.ToSearch(searchText));
        return result.Match(
            onSuccess: omisiones => omisiones.ToGridData(),
            onFailure: failure =>
            {
                AddFailure(failure, "buscar omisiones de cupo");
                return new GridData<IgnoraEscenarioCupoExclusionViewModel>();
            });
    }

    private Task AddOmisionesAsync()
    {
        createOpen = true;
        return Task.CompletedTask;
    }

    private async Task<bool> OnSaveOmisionesAsync(OmisionesCupoExclusionModel model)
    {
        loading = true;
        var response = await Mediator.SendRequest(model.ToCreate());

        bool result = await response.MatchAsync(
            onSuccess: RefreshAsync,
            onFailure: failure => AddFailure(failure, "crear omisiones de fechas")
        );

        loading = false;

        return result;
    }

    private async Task DeleteOmisionesAsync()
    {
        loading = true;

        var response = await Mediator.SendRequest(omisionesToDelete.ToDelete());
        await response.SwitchAsync(
            onSuccess: DeleteAsync,
            onFailure: failure => AddFailure(failure, "eliminar omisiones de fechas")
        );

        loading = false;
    }

    private async Task DeleteAsync(Success success, CancellationToken cancellationToken)
    {
        await RefreshAsync(success, cancellationToken);
        omisionesToDelete.Clear();
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
