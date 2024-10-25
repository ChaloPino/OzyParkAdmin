using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Controls;

/// <summary>
/// El componente para el detalle de la apertura de un día
/// </summary>
public partial class AperturaDiaDetail
{
    private bool loading;

    /// <summary>
    /// La apertura de caja.
    /// </summary>
    [Parameter]
    public AperturaCajaViewModel? AperturaCaja { get; set; }

    /// <summary>
    /// Función para cargar los detalles.
    /// </summary>
    [Parameter]
    public Func<AperturaCajaViewModel, Task> LoadDetails { get; set; } = (_) => Task.CompletedTask;

    /// <summary>
    /// Función para mostrar los turnos.
    /// </summary>
    [Parameter]
    public Func<TurnoCajaModel, Task> ShowShift { get; set; } = (_) => Task.CompletedTask;

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (AperturaCaja?.Id is not null && AperturaCaja.Turnos.Count == 0)
        {
            await OnLoadDetailsAsync(AperturaCaja);
        }
    }

    private async Task OnLoadDetailsAsync(AperturaCajaViewModel aperturaCaja)
    {
        loading = true;
        await LoadDetails(aperturaCaja);
        loading = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnShowShiftAsync(TurnoCajaModel turno)
    {
        await ShowShift(turno);
    }

    private static Color ShiftColor(TurnoCajaModel turno) =>
       turno.Estado switch
       {
           EstadoTurno.Abierto => Color.Error,
           EstadoTurno.Pendiente => Color.Warning,
           _ => Color.Success
       };
}
