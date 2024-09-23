using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Components.Admin.Shared;
using System.Collections.ObjectModel;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Controls;

/// <summary>
/// El diálogo de la asociación de tramos.
/// </summary>
public partial class TramosDialog
{
    private ObservableCollection<TramoServicioModel> tramosPorServicio = [];

    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public ServicioViewModel? Servicio { get; set; }

    [Parameter]
    public IEnumerable<CentroCostoModel> CentrosCosto { get; set; } = [];

    [Parameter]
    public IEnumerable<TramoModel> Tramos { get; set; } = [];

    [Parameter]
    public Func<ServicioViewModel, Task<bool>>? CommitChanges { get; set; }

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
            tramosPorServicio = new(Servicio.Tramos);
        }
    }

    private void AddCentroCosto()
    {
        tramosPorServicio.Add(new TramoServicioModel());
    }

    private void RemoveCentroCosto(TramoServicioModel tramo)
    {
        tramosPorServicio.Remove(tramo);
    }

    private async Task CancelEditingItemAsync()
    {
        tramosPorServicio = [];
        await ChangeIsOpen(false);
    }

    private async Task CommitItemChangesAsync()
    {
        if (Servicio is not null && CommitChanges is not null)
        {
            Servicio.Tramos = [.. tramosPorServicio];
            bool result = await CommitChanges(Servicio);

            if (result)
            {
                await ChangeIsOpen(false);
            }
        }

    }
}