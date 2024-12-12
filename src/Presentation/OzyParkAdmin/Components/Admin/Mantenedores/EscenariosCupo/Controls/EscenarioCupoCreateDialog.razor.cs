using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

public partial class EscenarioCupoCreateDialog
{
    private CreateEscenarioCupoLayout _createEscenarioCupoForm = default!;
    private bool _formEscenarioCupoValido = false;
    private bool _hasAttemptedValidation = false;
    private bool _fieldsModified = false;
    private int _activeStep;
    private int _index;
    private bool _completed;

    [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
    [Parameter] public EscenarioCupoModel SelectedEscenarioCupo { get; set; } = new();
    [Parameter] public List<CentroCostoInfo> CentrosCostos { get; set; } = new();
    [Parameter] public List<ZonaInfo> Zonas { get; set; } = new();
    [Parameter] public List<ServicioInfo> Servicios { get; set; } = new();
    [Parameter] public List<CanalVenta> CanalesVenta { get; set; } = new();

    private List<DetalleEscenarioCupoInfo> detalles { get; set; } = new();

    private List<DetalleEscenarioCupoExclusionFechaFullInfo> exclusionesPorFecha { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _hasAttemptedValidation = false;
        _fieldsModified = false;
        StateHasChanged();
    }

    private void ValidarFormularioEscenarioCupo()
    {
        _formEscenarioCupoValido = _createEscenarioCupoForm?.formEscenarioCupoValid == true;
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
                    Snackbar.Add("Completa la información del escenario cupo antes de continuar", Severity.Error, config =>
                    {
                        config.VisibleStateDuration = 2000;
                        config.RequireInteraction = false;
                    });
                    arg.Cancel = true;
                }
                break;
            case 1:
                if (!detalles.Any() && _hasAttemptedValidation)
                {
                    Snackbar.Add("Debe agregar al menos un detalle antes de continuar", Severity.Error, config =>
                    {
                        config.VisibleStateDuration = 2000;
                        config.RequireInteraction = false;
                    });
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
                    Snackbar.Add("Primero complete la información del escenario cupo", Severity.Error, config =>
                    {
                        config.VisibleStateDuration = 2000;
                        config.RequireInteraction = false;
                    });
                    arg.Cancel = true;
                }
                break;
            case 2:
                if (!detalles.Any() && _hasAttemptedValidation)
                {
                    Snackbar.Add("Debe agregar detalles antes de pasar a las exclusiones", Severity.Error, config =>
                    {
                        config.VisibleStateDuration = 2000;
                        config.RequireInteraction = false;
                    });
                    arg.Cancel = true;
                }
                break;
        }
    }

    private async Task CommitItemChangesAsync()
    {
        _hasAttemptedValidation = true;
        ValidarFormularioEscenarioCupo();
        if (_formEscenarioCupoValido && detalles.Any())
        {
            MudDialog.Close(DialogResult.Ok(SelectedEscenarioCupo));
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
        if (IsAllStepsValid() && _activeStep == 2)
        {
            Snackbar.Add("Todos los pasos son válidos. Puede proceder a guardar.", Severity.Success, config =>
            {
                config.VisibleStateDuration = 5000;
                config.RequireInteraction = false;
            });

            _completed = true;
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

    private bool IsAllStepsValid()
    {
        return _formEscenarioCupoValido && detalles.Any();
    }
}
