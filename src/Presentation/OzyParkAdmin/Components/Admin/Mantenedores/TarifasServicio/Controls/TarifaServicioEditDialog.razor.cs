using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Controls;

/// <summary>
/// Modal para editar una tarifa de servicio.
/// </summary>
public partial class TarifaServicioEditDialog
{
    private MudForm form = default!;
    private bool loading;

    /// <summary>
    /// Opciones para el modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Si el modal está abierto o no.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento que se dispara si <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// La tarifa a editar.
    /// </summary>
    [Parameter]
    public TarifaServicioViewModel Tarifa { get; set; } = new();

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<TarifaServicioViewModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
        Tarifa = new();
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (!form.IsValid)
        {
            return;
        }

        if (CommitChanges is not null)
        {
            loading = true;
            bool result = await CommitChanges(Tarifa);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            loading = false;
        }
    }
}
