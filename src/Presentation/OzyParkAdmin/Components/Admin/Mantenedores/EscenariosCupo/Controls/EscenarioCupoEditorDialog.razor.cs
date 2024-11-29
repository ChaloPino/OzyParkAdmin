using MassTransit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.List;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

public partial class EscenarioCupoEditorDialog
{
    private CreateEscenarioCupoLayout _editEscenarioCupoForm = default!;
    private bool _formEscenarioCupoValido = false;
    private bool _hasAttemptedValidation = false;
    private bool _fieldsModified = false;
    private int _activeStep;
    private int _index;
    private bool _completed;

    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public EscenarioCupoModel SelectedEscenarioCupo { get; set; }
    [Parameter] public EscenarioCupoViewModel ViewModel { get; set; } = default!;
    [Parameter] public List<CentroCostoInfo> CentrosCostos { get; set; } = new();
    [Parameter] public List<ZonaInfo> Zonas { get; set; } = new();
    [Parameter] public List<ServicioInfo> Servicios { get; set; } = new();
    [Parameter] public List<CanalVenta> CanalesVenta { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await CargarExclusionesPorFecha();
        ValidarFormularioEscenarioCupo();
        _hasAttemptedValidation = false;
        _fieldsModified = false;
        StateHasChanged();
    }

    private void ValidarFormularioEscenarioCupo()
    {
        _formEscenarioCupoValido = _editEscenarioCupoForm?.formEscenarioCupoValid == true;
    }

    private async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
    {
        if (arg.Action == StepAction.Complete)
        {
            _hasAttemptedValidation = true;
            await ControlStepCompletion(arg);
        }
        else if (arg.Action == StepAction.Activate)
        {
            await ControlStepNavigation(arg);
        }
    }

    private async Task ControlStepCompletion(StepperInteractionEventArgs arg)
    {
        switch (arg.StepIndex)
        {
            case 0:
                ValidarFormularioEscenarioCupo();
                if (!_formEscenarioCupoValido && (_fieldsModified || !_hasAttemptedValidation))
                {
                    Snackbar.Add("Completa la información del escenario cupo antes de continuar", Severity.Error);
                    arg.Cancel = true;
                }
                break;
            case 1:
                if (!SelectedEscenarioCupo.Detalles.Any() && _hasAttemptedValidation)
                {
                    Snackbar.Add("Debe agregar al menos un detalle antes de continuar", Severity.Error);
                    arg.Cancel = true;
                }
                break;
        }
    }

    private async Task ControlStepNavigation(StepperInteractionEventArgs arg)
    {
        switch (arg.StepIndex)
        {
            case 1:
                ValidarFormularioEscenarioCupo();
                if (!_formEscenarioCupoValido && (_fieldsModified || !_hasAttemptedValidation))
                {
                    Snackbar.Add("Primero complete la información del escenario cupo", Severity.Error);
                    arg.Cancel = true;
                }
                break;
            case 2:
                if (!SelectedEscenarioCupo.Detalles.Any() && _hasAttemptedValidation)
                {
                    Snackbar.Add("Debe agregar detalles antes de pasar a las exclusiones", Severity.Error);
                    arg.Cancel = true;
                }
                break;
        }
    }

    private async Task CommitItemChangesAsync()
    {
        _hasAttemptedValidation = true;
        ValidarFormularioEscenarioCupo();
        if (_formEscenarioCupoValido && SelectedEscenarioCupo.Detalles.Any())
        {
            ViewModel.Loading = true;
            Snackbar.Add("Guardando cambios...", Severity.Info);
            MudDialog.Close(DialogResult.Ok(SelectedEscenarioCupo));
            ViewModel.Loading = false;
        }
    }

    private void CancelEditingItem()
    {
        Snackbar.Add("Edición cancelada.", Severity.Warning);
        CloseDialog();
    }

    private void CloseDialog()
    {
        MudDialog.Close();
    }

    private void OnActiveIndexChanged(int newIndex)
    {
        _activeStep = newIndex;
        if (IsAllStepsValid() && _activeStep == 2) // Assuming the last step index is 2
        {
            Snackbar.Add("Todos los pasos son válidos. Puede proceder a guardar.", Severity.Success);
        }
    }

    private void OnDetalleAgregado()
    {
        StateHasChanged();
    }

    private void OnExclusionAgregada()
    {
        StateHasChanged();
    }

    private void OnFieldChanged(bool isValid)
    {
        _fieldsModified = true;
        _formEscenarioCupoValido = isValid;
        StateHasChanged();
    }

    private async Task CargarExclusionesPorFecha()
    {
        ViewModel.Loading = true;
        ListEscenarioCupoExclusionesPorFecha list = new(SelectedEscenarioCupo.Id);
        var result = await Mediator.SendRequest(list);
        ViewModel.Loading = false;

        result.Switch(
            onSuccess: SelectedEscenarioCupo.ExclusionesFecha.AddRange,
            onFailure: failure => AddFailure(failure, "buscando exclusiones de fechas"));
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }

    private bool IsAllStepsValid()
    {
        return _formEscenarioCupoValido && SelectedEscenarioCupo.Detalles.Any();
    }
}

