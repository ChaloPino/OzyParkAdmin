using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.Cajas.Dia;
using OzyParkAdmin.Application.Cajas.Find;
using OzyParkAdmin.Application.Cajas.Search;
using OzyParkAdmin.Application.Cajas.Turno;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Seguridad.Permisos;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas;

/// <summary>
/// La página de control de cajas.
/// </summary>
public partial class Index
{
    private ClaimsPrincipal user = default!;
    private MudDataGrid<AperturaCajaViewModel> dataGrid = default!;
    private ObservableGridData<AperturaCajaViewModel> currentCajas = new();
    private List<CentroCostoInfo> centrosCosto = [];
    private string? searchText;
    private DateTime? searchDate = DateTime.Today;
    private CentroCostoInfo? centroCosto;
    private CajaAcciones acciones = CajaAcciones.None;

    private bool openCloseDay;
    private bool openCloseShift;
    private AperturaCajaViewModel? currentCaja;
    private TurnoCajaModel? currentTurno;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;
        acciones = await PermisoService.FindPermisosByUser(user);
        await LoadCentrosCostoAsync();
    }

    private async Task LoadCentrosCostoAsync()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user));

        result.Switch(
            onSuccess: list =>
            {
                centrosCosto = list;
                centroCosto = centrosCosto.FirstOrDefault();
            },
            onFailure: failure => AddFailure(failure, "consiguiendo centros de costo"));
    }

    private async Task SearchDateChanged(DateTime? date)
    {
        searchDate = date;
        await dataGrid.ReloadServerData();
    }

    private async Task CentroCostoChanged(CentroCostoInfo info)
    {
        centroCosto = info;
        await dataGrid.ReloadServerData();
    }

    private async Task<GridData<AperturaCajaViewModel>> LoadCajasAsync(GridState<AperturaCajaViewModel> state)
    {
        if (centroCosto is null)
        {
            await LoadCentrosCostoAsync();
        }

        if (centroCosto is not null)
        {
            SearchAperturasCaja searchCajas = state.ToSearch(searchText, searchDate, centroCosto);
            var result = await Mediator.SendRequest(searchCajas);
            result.Switch(
                onSuccess: aperturas => currentCajas = aperturas.ToGridData(dataGrid, acciones),
                onFailure: failure => AddFailure(failure, "buscando aperturas de caja"));

            return currentCajas;
        }

        return currentCajas;
    }

    private async Task OnLoadDetailsAsync(AperturaCajaViewModel aperturaCaja)
    {
        if (aperturaCaja.Id is not null)
        {
            var find = new FindAperturaCajaDetalle(aperturaCaja.Id.Value);
            var request = await Mediator.SendRequest(find);

            request.Switch(
                onSuccess: detalle =>
                {
                    aperturaCaja.Turnos = detalle.Turnos.ToModel(acciones);
                    aperturaCaja.Servicios = detalle.Servicios;
                },
                onFailure: failure => AddFailure(failure, "busando el detalle de la apertura de la caja."));
        }
    }

    private async Task ShowCloseDayAsync(CellContext<AperturaCajaViewModel> context)
    {
        await OnLoadDetailsAsync(context.Item);
        currentCaja = context.Item;
        openCloseDay = true;
        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowLastShiftAsync(CellContext<AperturaCajaViewModel> context)
    {
        if (context.Item.UltimoTurnoId is not null)
        {
            await OnLoadDetailsAsync(context.Item);
            TurnoCajaModel? turno = context.Item.Turnos.Find(x => x.Id == context.Item.UltimoTurnoId.Value);

            if (turno is not null)
            {
                await ShowShiftAsync(turno);
            }
        }
    }

    private async Task ShowShiftAsync(TurnoCajaModel turno)
    {
        currentTurno = turno;
        openCloseShift = true;
        await InvokeAsync(StateHasChanged);
    }

    private async Task<bool> CloseDayAsync(AperturaCajaViewModel apertura)
    {
        CerrarDia cerrarDia = new(apertura.Id!.Value, user, apertura.Comentario!, apertura.MontoEfectivoParaCierre, apertura.MontoTransbankParaCierre);
        var result = await Mediator.SendRequest(cerrarDia);
        return result.Match(
            onSuccess: info => UpdateDay(apertura, info),
            onFailure: failure => AddFailure(failure, "cerrar día.")
        );
    }

    private async Task<bool> ReopenDayAsync(AperturaCajaViewModel apertura)
    {
        ReabrirDia cerrarDia = new(apertura.Id!.Value);
        var result = await Mediator.SendRequest(cerrarDia);
        return result.Match(
            onSuccess: info => UpdateDay(apertura, info),
            onFailure: failure => AddFailure(failure, "cerrar día.")
        );
    }

    private async Task<bool> CloseShiftAsync(TurnoCajaModel turno)
    {
        CerrarTurno cerrarTurno = new(turno.DiaId, turno.Id, user, turno.EfectivoCierreSupervisor!.Value, turno.MontoTransbankSupervisor!.Value, turno.Comentario!, [.. turno.Detalle]);
        var result = await Mediator.SendRequest(cerrarTurno);
        return result.Match(
            onSuccess: info => UpdateShift(turno, info),
            onFailure: failure => AddFailure(failure, "cerrar turno.")
        );
    }

    private async Task<bool> ReopenShiftAsync(TurnoCajaModel turno)
    {
        ReabrirTurno reabrirTurno = new(turno.DiaId, turno.Id, [.. turno.Detalle]);
        var result = await Mediator.SendRequest(reabrirTurno);
        return result.Match(
            onSuccess: info => UpdateShift(turno, info),
            onFailure: failure => AddFailure(failure, "reabrir turno.")
        );
    }

    private static bool UpdateDay(AperturaCajaViewModel apertura, AperturaCajaInfo info)
    {
        apertura.Update(info);
        return true;
    }

    private bool UpdateShift(TurnoCajaModel turno, TurnoCajaInfo info)
    {
        turno.Update(info);
        AperturaCajaViewModel? apertura = currentCajas?.Find(x => x.Id == turno.DiaId);
        apertura?.TryUpdateLastShift(turno);
        return true;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }
}
