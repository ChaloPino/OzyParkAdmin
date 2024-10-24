using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Controls;

/// <summary>
/// El componente para el modal del cierre de cajas.
/// </summary>
public partial class CierreCajaDialog
{
    private MudForm form = default!;
    private bool loading;
    private bool reviewed;

    /// <summary>
    /// La apertura a cerrar.
    /// </summary>
    [Parameter]
    public AperturaCajaViewModel Apertura { get; set; } = default!;

    /// <summary>
    /// Si el modal está abierto o cerrado.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento que se dispara cuando <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Las opciones del modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Función para cerrar el día.
    /// </summary>
    [Parameter]
    public Func<AperturaCajaViewModel, Task<bool>> CloseDay { get; set; } = (_) => Task.FromResult(false);

    /// <summary>
    /// Función para reabrir el día.
    /// </summary>
    [Parameter]
    public Func<AperturaCajaViewModel, Task<bool>> ReopenDay { get; set; } = (_) => Task.FromResult(false);

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CancelEditingItemAsync()
    {
        reviewed = false;
        await ChangeIsOpen(false);
    }

    private async Task CloseDayAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            loading = true;

            if (await CloseDay(Apertura))
            {
                await ChangeIsOpen(false);
            }

            loading = false;
        }
    }

    private async Task ReopenDayAsync()
    {
        loading = true;

        if (await ReopenDay(Apertura))
        {
            await ChangeIsOpen(false);
        }

        loading = false;
    }
}
