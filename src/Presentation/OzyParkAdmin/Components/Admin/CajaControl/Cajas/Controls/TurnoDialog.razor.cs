using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Controls;

/// <summary>
/// El modal del turno de caja.
/// </summary>
public partial class TurnoDialog
{
    private MudForm form = default!;
    private bool loading;

    /// <summary>
    /// El turno de la caja.
    /// </summary>
    [Parameter]
    public TurnoCajaModel Turno { get; set; } = default!;

    /// <summary>
    /// Si el modal está abierto o no.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// El evento que se dispara cuando <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Las opciones para el modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// La función para cerrar el turno.
    /// </summary>
    [Parameter]
    public Func<TurnoCajaModel, Task<bool>> CloseShift { get; set; } = (_) => Task.FromResult(false);

    /// <summary>
    /// La función para reabrir el turno.
    /// </summary>
    [Parameter]
    public Func<TurnoCajaModel, Task<bool>> ReopenShift { get; set; } = (_) => Task.FromResult(false);

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private static void ShowDetalle(ResumenTurnoModel resumen)
    {
        resumen.ShowDetail = !resumen.ShowDetail;
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }

    private async Task CloseShiftAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            loading = true;

            if (await CloseShift(Turno))
            {
                await ChangeIsOpen(false);
            }

            loading = false;
        }
    }

    private async Task ReopenShiftAsync()
    {
        loading = true;

        if (await ReopenShift(Turno))
        {
            await ChangeIsOpen(false);
        }

        loading = false;
    }
}
