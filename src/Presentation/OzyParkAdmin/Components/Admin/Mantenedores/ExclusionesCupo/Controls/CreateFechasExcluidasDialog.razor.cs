using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Controls;

/// <summary>
/// Modal para crear varias exclusiones por fecha.
/// </summary>
public partial class CreateFechasExcluidasDialog
{
    private MudForm form = default!;
    private FechasExcluidasCupoModel fechasExcluidas = new();

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
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<FechasExcluidasCupoModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    /// <summary>
    /// Opciones para el modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    /// <summary>
    /// Los centros de costo.
    /// </summary>
    [Parameter]
    public List<CentroCostoInfo> CentrosCosto { get; set; } = [];

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

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        fechasExcluidas = new();
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            fechasExcluidas.Loading = true;
            bool result = await CommitChanges(fechasExcluidas);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            fechasExcluidas.Loading = false;
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }
}
