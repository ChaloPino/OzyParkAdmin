using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// Modal para asignar grupos etarios a un servicio.
/// </summary>
public partial class GruposEtariosDialog
{
    private IEnumerable<GrupoEtarioInfo> gruposEtarios = [];

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
    /// El servicio al que se le van a asignar grupos etarios.
    /// </summary>
    [Parameter]
    public ServicioViewModel Servicio { get; set; } = new();

    /// <summary>
    /// Los grupos etarios que se pueden asignar al servicio.
    /// </summary>
    [Parameter]
    public IEnumerable<GrupoEtarioInfo> GruposEtarios { get; set; } = [];

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
            gruposEtarios = new List<GrupoEtarioInfo>(Servicio.GruposEtarios);
        }
    }

    private async Task CancelEditingItemAsync()
    {
        gruposEtarios = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        Servicio.GruposEtarios = [.. gruposEtarios];
        bool result = await CommitChanges(Servicio);

        if (result)
        {
            await ChangeIsOpen(false);
        }
    }
}
