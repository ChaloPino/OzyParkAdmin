using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

/// <summary>
/// El modal para editar un cupo.
/// </summary>
public partial class EscenarioCupoCreateDialog
{
    private CreateEscenarioCupoLayout createEscenarioCupoForm = default!;
    private EscenarioCupoModel escenarioCupo = new();
    private bool detallesAgregados = false; // Nueva propiedad para rastrear si hay detalles agregados

    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public Func<EscenarioCupoModel, Task<bool>> CommitChanges { get; set; } = (_) => Task.FromResult(true);

    [Parameter]
    public EscenarioCupoViewModel ViewModel { get; set; } = default!;

    [Parameter]
    public DialogOptions? DialogOptions { get; set; }

    [Parameter]
    public List<CentroCostoInfo> CentrosCostos { get; set; } = new();

    [Parameter]
    public List<ZonaInfo> Zonas { get; set; } = new();

    [Parameter]
    public List<ServicioInfo> Servicios { get; set; } = new();

    // Propiedad para habilitar/deshabilitar el botón "Guardar"
    private bool EsValidoParaGuardar =>
        createEscenarioCupoForm?.ValidateForm().Result == true && detallesAgregados;

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        escenarioCupo = new();
        detallesAgregados = false; // Resetear el estado de detalles agregados
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        bool isValid = await createEscenarioCupoForm.ValidateForm();

        if (isValid && EsValidoParaGuardar)
        {
            ViewModel.Loading = true;
            bool result = await CommitChanges(escenarioCupo);

            if (result)
            {
                await ChangeIsOpen(false);
            }

            ViewModel.Loading = false;
        }
    }

    private async Task CancelEditingItemAsync()
    {
        await ChangeIsOpen(false);
    }

    // Método para manejar el evento cuando se agrega un detalle
    private void OnDetalleAgregado()
    {
        detallesAgregados = true;
        StateHasChanged(); // Forzar la actualización de la UI para reflejar el cambio
    }
}
