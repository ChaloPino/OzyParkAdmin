using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.Cajas;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// Modal para asignar cajas a los servicios.
/// </summary>
public partial class CajasDialog
{
    private MudForm form = default!;
    private ObservableCollection<CajaServicioModel> cajasPorServicio = [];

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
    /// El servicio al que se le van a asignar cajas.
    /// </summary>
    [Parameter]
    public ServicioViewModel Servicio { get; set; } = new();

    /// <summary>
    /// Las cajas que se pueden asignar.
    /// </summary>
    [Parameter]
    public IEnumerable<CajaInfo> Cajas { get; set; } = [];

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
            cajasPorServicio = new(Servicio.Cajas);
        }
    }

    private void AddCentroCosto()
    {
        cajasPorServicio.Add(new CajaServicioModel());
    }

    private void RemoveCentroCosto(CajaServicioModel caja)
    {
        cajasPorServicio.Remove(caja);
    }

    private async Task CancelEditingItemAsync()
    {
        cajasPorServicio = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            Servicio.Cajas = [.. cajasPorServicio];

            bool result = await CommitChanges(Servicio);

            if (result)
            {
                await ChangeIsOpen(false);
            }
        }
    }
}
