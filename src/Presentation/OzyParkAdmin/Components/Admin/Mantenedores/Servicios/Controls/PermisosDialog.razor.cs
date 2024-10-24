using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// Modal para asignar permisos a un servicio.
/// </summary>
public partial class PermisosDialog
{
    private MudForm form = default!;
    private ObservableCollection<PermisoServicioModel> permisosPorServicio = [];

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
    /// Evento que se dispara cuando <see cref="IsOpen"/> cambia.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// El servicio al que se le van a asignar permisos.
    /// </summary>
    [Parameter]
    public ServicioViewModel Servicio { get; set; } = new();

    /// <summary>
    /// Los centros de costo que se pueden asignar como permiso.
    /// </summary>
    [Parameter]
    public IEnumerable<CentroCostoInfo> CentrosCosto { get; set; } = [];

    /// <summary>
    /// Los tramos que se pueden asignar como permiso.
    /// </summary>
    [Parameter]
    public IEnumerable<TramoInfo> Tramos { get; set; } = [];

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
            permisosPorServicio = new(Servicio.Permisos);
        }
    }

    private void AddCentroCosto()
    {
        permisosPorServicio.Add(new PermisoServicioModel());
    }

    private void RemoveCentroCosto(PermisoServicioModel permiso)
    {
        permisosPorServicio.Remove(permiso);
    }

    private async Task CancelEditingItemAsync()
    {
        permisosPorServicio = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        await form.Validate();

        if (form.IsValid)
        {
            Servicio.Permisos = [.. permisosPorServicio];
            bool result = await CommitChanges(Servicio);

            if (result)
            {
                await ChangeIsOpen(false);
            }
        }
    }
}
