using MassTransit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.DetalleEscenarioCupo.List;
using OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.List;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
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
    private bool _loading;
    private bool _completed;

    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public EscenarioCupoModel SelectedEscenarioCupo { get; set; }
    [Parameter] public EscenarioCupoViewModel ViewModel { get; set; } = default!;
    [Parameter] public List<CentroCostoInfo> CentrosCostos { get; set; } = new();
    [Parameter] public List<ZonaInfo> Zonas { get; set; } = new();
    [Parameter] public List<ServicioInfo> Servicios { get; set; } = new();
    [Parameter] public List<CanalVenta> CanalesVenta { get; set; } = new();
    private List<DetalleEscenarioCupoInfo> Detalles { get; set; } = new();
    private List<DetalleEscenarioCupoExclusionFechaFullInfo> ExclusionesPorFecha { get; set; } = new();

    private bool _detallesModified = false;
    private bool _exclusionesModified = false;

    protected override async Task OnInitializedAsync()
    {
        _hasAttemptedValidation = false;
        _fieldsModified = false;

        await ValidarFormularioInicialAsync();

        ValidarFormularioEscenarioCupo();

        StateHasChanged();
    }

    private async Task ValidarFormularioInicialAsync()
    {
        // Esperar para garantizar que el formulario esté completamente montado
        await Task.Delay(500);

        // Forzar una revalidación del formulario
        if (_editEscenarioCupoForm != null)
        {
            await _editEscenarioCupoForm.form.Validate().ConfigureAwait(false);
        }

        _formEscenarioCupoValido = _editEscenarioCupoForm?.formEscenarioCupoValid == true;
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

    // Método llamado al agregar un detalle
    private void OnDetalleAgregado()
    {
        _detallesModified = true;
        StateHasChanged();
    }

    // Método llamado al agregar una exclusión
    private void OnExclusionAgregada()
    {
        _exclusionesModified = true;
        StateHasChanged();
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
                if (!Detalles.Any() && _hasAttemptedValidation)
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
                if (!Detalles.Any() && _hasAttemptedValidation)
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

    // Método que confirma los cambios
    private async Task CommitItemChangesAsync()
    {
        _hasAttemptedValidation = true;
        ValidarFormularioEscenarioCupo();

        if (_formEscenarioCupoValido && (Detalles.Any() || ExclusionesPorFecha.Any()))
        {
            ViewModel.Loading = true;

            MudDialog.Close(DialogResult.Ok(new EscenarioCupoDataResponse
            {
                EscenarioCupo = SelectedEscenarioCupo,
                Detalles = Detalles,
                ExclusionesPorFecha = ExclusionesPorFecha
            }));

            ViewModel.Loading = false;
        }
        else
        {
            Snackbar.Add("Debe completar todos los campos obligatorios antes de guardar.", Severity.Error);
        }
    }

    private void CancelEditingItem()
    {
        Snackbar.Add("Edición cancelada.", Severity.Warning);
    }

    private async void OnActiveIndexChanged(int newIndex)
    {
        _activeStep = newIndex;

        switch (newIndex)
        {
            case 1: await ListDetallesEscenarioCupo(); break;
            case 2: await ListDetalleEscenarioCupoExclusionesFecha(); break;
        }

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
    private void OnFieldChanged(bool isValid)
    {
        _fieldsModified = true;
        _formEscenarioCupoValido = isValid;
        StateHasChanged();
    }
    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }

    private bool IsAllStepsValid()
    {
        return _formEscenarioCupoValido && Detalles.Any();
    }

    private async Task ListDetallesEscenarioCupo()
    {
        ListDetalleEscenarioCupo list = new(SelectedEscenarioCupo.Id);

        var result = await Mediator.SendRequest(list);
        result.Switch(
            onSuccess: list =>
            {
                Detalles = list;
                _detallesModified = false;
                StateHasChanged();
            },
            onFailure: failure => AddFailure(failure, "cargar detalles de escenario cupo"));
    }

    private async Task ListDetalleEscenarioCupoExclusionesFecha()
    {
        ListEscenarioCupoExclusionesPorFecha list = new(SelectedEscenarioCupo.Id);

        var result = await Mediator.SendRequest(list);
        result.Switch(
            onSuccess: list =>
            {
                ExclusionesPorFecha = list;
                _exclusionesModified = false;
                StateHasChanged();
            },
            onFailure: failure => AddFailure(failure, "cargar exclusiones por fecha escenario cupo"));
    }


}
