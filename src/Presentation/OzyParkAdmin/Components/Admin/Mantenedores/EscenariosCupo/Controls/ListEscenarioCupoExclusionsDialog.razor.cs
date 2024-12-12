using MassTransit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using OzyParkAdmin.Application.DetallesEscenariosExclusiones.Search;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;

public partial class ListEscenarioCupoExclusionsDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public EscenarioCupoModel SelectedEscenarioCupo { get; set; } = default!;

    [Parameter]
    public List<ServicioInfo> Servicios { get; set; } = default!;

    [Parameter]
    public List<CanalVenta> CanalesVenta { get; set; } = default!;

    [Parameter]
    public List<DiaSemana> DiasSemana { get; set; } = default!;

    [Parameter]
    public Func<IEnumerable<DetalleEscenarioCupoExclusionFullInfo>, Task> OnExclusionesUpdated { get; set; } = async (model) => await Task.CompletedTask;


    private bool loading;
    private MudDataGrid<DetalleEscenarioCupoExclusionModel> dataGrid = new();
    private ObservableGridData<DetalleEscenarioCupoExclusionModel> currentExclusions = new();
    private string? searchText;
    private HashSet<DetalleEscenarioCupoExclusionModel> exclusionesToDelete = new();
    private List<DetalleEscenarioCupoExclusionFullInfo> exclusiones = [];


    /// <summary>
    /// Busca los escenarios de cupo en base al estado de la grilla y los parámetros de búsqueda.
    /// </summary>
    private async Task<GridData<DetalleEscenarioCupoExclusionModel>> SearchEscenariosCupoExclusionesAsync(GridState<DetalleEscenarioCupoExclusionModel> state)
    {
        loading = true;

        var servicesIds = Servicios.Select(x => x.Id).ToArray();
        var canalesVentaIds = CanalesVenta.Select(x => x.Id).ToArray();
        var diasSemanaIds = DiasSemana.Select(x => x.Id).ToArray();

        SearchEscenarioCupoExclusion searchEscenariosCupos = state.ToSearch(SelectedEscenarioCupo.Id, servicesIds, canalesVentaIds, diasSemanaIds, searchText);
        var result = await Mediator.SendRequest(searchEscenariosCupos);

        result.Switch(
            onSuccess: exclusiones => currentExclusions = exclusiones.ToGridData(dataGrid),
            onFailure: failure => Snackbar.AddFailure(failure, "buscar cupos"));

        loading = false;

        return currentExclusions;
    }

    public async Task OpenCreateExclusionDialogAsync()
    {
        var parameters = new DialogParameters
        {
            {"SelectedEscenarioCupo", SelectedEscenarioCupo },
            { "Servicios", Servicios },
            { "CanalesVenta", CanalesVenta },
            { "DiasSemana", DiasSemana }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<CreateEscenarioCupoExclusionsDialog>("Crear Exclusión de Escenario de Cupo", parameters, options);

        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await ProcessExclusions((IEnumerable<DetalleEscenarioCupoExclusionFullInfo>)result.Data);
        }
    }

    public async Task OpenEditExclusionDialogAsync(DetalleEscenarioCupoExclusionModel exclusionToEdit)
    {
        var parameters = new DialogParameters
        {
            { "SelectedEscenarioCupo", SelectedEscenarioCupo },
            { "Servicios", Servicios },
            { "CanalesVenta", CanalesVenta },
            { "DiasSemana", DiasSemana },
            { "ExclusionToEdit", exclusionToEdit }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<CreateEscenarioCupoExclusionsDialog>("Editar Exclusión de Escenario de Cupo", parameters, options);

        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await ProcessExclusions((IEnumerable<DetalleEscenarioCupoExclusionFullInfo>)result.Data);
        }
    }
    /// <summary>
    /// Procesa las exclusiones que se han agregado o quitado para el escenario cupo seleccionado.
    /// </summary>
    private async Task ProcessExclusions(IEnumerable<DetalleEscenarioCupoExclusionFullInfo> exclusions)
    {
        var existingExclusions = exclusiones;

        // Identificar las exclusiones a eliminar
        var exclusionsToRemove = existingExclusions
            .Where(existing => !exclusions.Any(newEx =>
                newEx.ServicioId == existing.ServicioId &&
                newEx.CanalVentaId == existing.CanalVentaId &&
                newEx.DiaSemanaId == existing.DiaSemanaId &&
                newEx.HoraInicio == existing.HoraInicio &&
                newEx.HoraFin == existing.HoraFin))
            .ToList();

        foreach (var exclusion in exclusionsToRemove)
        {
            existingExclusions.Remove(exclusion);
        }

        // Identificar las exclusiones a agregar
        var exclusionsToAdd = exclusions
            .Where(newEx => !existingExclusions.Any(existing =>
                existing.ServicioId == newEx.ServicioId &&
                existing.CanalVentaId == newEx.CanalVentaId &&
                existing.DiaSemanaId == newEx.DiaSemanaId &&
                existing.HoraInicio == newEx.HoraInicio &&
                existing.HoraFin == newEx.HoraFin))
            .ToList();

        foreach (var exclusion in exclusionsToAdd)
        {
            existingExclusions.Add(exclusion);
        }

        if (OnExclusionesUpdated is not null)
        {
            loading = true;

            await OnExclusionesUpdated.Invoke(existingExclusions);

            loading = false;
        }
    }

    private async Task EliminarExclusiones()
    {
        bool res = await DialogService.ShowConfirmationDialogAsync("Confirmación", "¿Está seguro que desea eliminar las exclusiones seleccionadas?", "Sí", "No");

        if (res)
        {
            loading = true;

            // Eliminar las exclusiones seleccionadas directamente del escenario cupo
            foreach (var exclusion in exclusionesToDelete)
            {
                // Buscar la exclusión correspondiente en la lista de exclusiones del escenario cupo
                var exclusionToRemove = exclusiones.FirstOrDefault(existing =>
                    existing.ServicioId == exclusion.ServicioId &&
                    existing.CanalVentaId == exclusion.CanalVentaId &&
                    existing.DiaSemanaId == exclusion.DiaSemanaId &&
                    existing.HoraInicio == exclusion.HoraInicio &&
                    existing.HoraFin == exclusion.HoraFin);

                // Eliminar la exclusión si existe
                if (exclusionToRemove != null)
                {
                    exclusiones.Remove(exclusionToRemove);
                }
            }

            loading = true;

            // Invocar el evento para que se actualice la lista de exclusiones
            await OnExclusionesUpdated.Invoke(exclusiones);

            loading = false;

            // Limpiar la lista de exclusiones seleccionadas para eliminar
            exclusionesToDelete.Clear();

            // Actualizar la vista del DataGrid para reflejar los cambios
            await dataGrid.ReloadServerData();

            loading = false;
        }

    }


    private void Done()
    {
        MudDialog.Close(DialogResult.Ok(SelectedEscenarioCupo));
    }

    private void Cancell()
    {
        MudDialog.CancelAll();
    }


}
