using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Controls;

/// <summary>
/// El modal para crear varias omisiones de cupo.
/// </summary>
public partial class CreateOmisionesDialog
{
    private MudForm form = default!;
    private OmisionesCupoExclusionModel omisionesCrear = new();

    /// <summary>
    /// Si el modal está abierto o no.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Evento que se dispara cuando <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// La función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<OmisionesCupoExclusionModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <summary>
    /// Las opciones del modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Los escenarios de cupo.
    /// </summary>
    [Parameter]
    public List<EscenarioCupoInfo> EscenariosCupo { get; set; } = [];

    /// <summary>
    /// Los canales de venta.
    /// </summary>
    [Parameter]
    public List<CanalVenta> CanalesVenta { get; set; } = [];

    private static string CanalesVentaDescriptions(List<string> canalesVenta) => canalesVenta switch
    {
        { Count: 0 } => "Seleccione canales de venta",
        { Count: 1 } => canalesVenta[0],
        _ => $"{canalesVenta.Count} canales de venta seleccionados",
    };

    private static string EscenariosCupoDescriptions(List<string> escenariosCupo) => escenariosCupo switch
    {
        { Count: 0 } => "Seleccione escenarios de cupo",
        { Count: 1 } => escenariosCupo[0],
        _ => $"{escenariosCupo.Count} escenarios de cupo seleccionados",
    };

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        omisionesCrear = new();
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            omisionesCrear.Loading = true;
            bool result = await CommitChanges(omisionesCrear);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            omisionesCrear.Loading = false;
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }
}
