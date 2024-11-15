using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

public partial class EscenarioCupoEditorDialog
{
    private CreateEscenarioCupoLayout editEscenarioCupoForm = default!;
    private bool detallesAgregados = false;

    [Parameter]
    public EscenarioCupoModel SelectedEscenarioCupo { get; set; }

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

    private bool EsValidoParaGuardar =>
        editEscenarioCupoForm?.ValidateForm().Result == true && detallesAgregados;

    private async Task ChangeIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        detallesAgregados = false;
        await IsOpenChanged.InvokeAsync(isOpen);
    }

    private async Task CommitItemChangesAsync()
    {
        if (SelectedEscenarioCupo == null)
        {
            throw new InvalidOperationException("SelectedEscenarioCupo no puede ser null.");
        }

        bool isValid = await editEscenarioCupoForm.ValidateForm();

        if (isValid && EsValidoParaGuardar)
        {
            ViewModel.Loading = true;
            bool result = await CommitChanges(SelectedEscenarioCupo);

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

    private void OnDetalleAgregado()
    {
        detallesAgregados = true;
        StateHasChanged();
    }
}
