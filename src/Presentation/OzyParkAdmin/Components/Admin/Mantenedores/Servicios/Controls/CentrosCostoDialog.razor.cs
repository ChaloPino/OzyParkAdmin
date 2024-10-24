using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// Modal para asignar centros de costo a un servicio.
/// </summary>
public partial class CentrosCostoDialog
{
    private MudForm form = default!;
    private ObservableCollection<CentroCostoServicioModel> centrosCostoPorServicio = [];

    /// <summary>
    /// Las opciones para el modal.
    /// </summary>
    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

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
    /// El servicio al que se le van a asignar centros de costo.
    /// </summary>
    [Parameter]
    public ServicioViewModel Servicio { get; set; } = new();

    /// <summary>
    /// Los centros de costo que se pueden asignar.
    /// </summary>
    [Parameter]
    public IEnumerable<CentroCostoInfo> CentrosCosto { get; set; } = [];

    /// <summary>
    /// Función para guardar los cambios.
    /// </summary>
    [Parameter]
    public Func<ServicioViewModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        if (Servicio is not null)
        {
            centrosCostoPorServicio = new(Servicio.CentrosCosto);
        }
    }

    private void AddCentroCosto()
    {
        centrosCostoPorServicio.Add(new CentroCostoServicioModel());
    }

    private void RemoveCentroCosto(CentroCostoServicioModel centroCosto)
    {
        centrosCostoPorServicio.Remove(centroCosto);
    }

    private async Task CancelEditingItemAsync()
    {
        centrosCostoPorServicio = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            Servicio.CentrosCosto = [.. centrosCostoPorServicio];
            bool result = await CommitChanges(Servicio);

            if (result)
            {
                await ChangeIsOpen(false);
            }
        }
    }
}
