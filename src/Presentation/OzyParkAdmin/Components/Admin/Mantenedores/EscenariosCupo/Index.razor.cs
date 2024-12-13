using DocumentFormat.OpenXml.Office2013.Excel;
using MassTransit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Extensions;
using OzyParkAdmin.Application.CanalesVenta.List;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.DetalleEscenarioCupo.Create;
using OzyParkAdmin.Application.DetalleEscenarioCupo.Update;
using OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.Create;
using OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.Update;
using OzyParkAdmin.Application.DetallesEscenariosExclusiones.Update;
using OzyParkAdmin.Application.DiasSemana.List;
using OzyParkAdmin.Application.EscenariosCupo.Create;
using OzyParkAdmin.Application.EscenariosCupo.Search;
using OzyParkAdmin.Application.EscenariosCupo.Update;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Servicios.List;
using OzyParkAdmin.Application.Zonas.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Controls;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo;

public partial class Index
{
    private bool loading;
    private ClaimsPrincipal? user;
    private MudDataGrid<EscenarioCupoModel> dataGrid = default!;
    private ObservableGridData<EscenarioCupoModel> currentEscenariosCupo = new();
    private HashSet<EscenarioCupoModel> escenariosCupoToDelete = [];
    private string? searchText;

    private List<CentroCostoInfo> centrosCosto = [];
    private List<ZonaInfo> zonas = [];
    private List<ServicioInfo> servicios = [];
    private List<CanalVenta> canalesVenta = [];
    private List<DiaSemana> diasSemana = [];
    private EscenarioCupoModel? selectedEscenarioCupo;

    private EscenarioCupoViewModel viewModel = new();

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    /// <inheritdoc/>
    /// Inicializa el componente cargando el estado de autenticación del usuario y las referencias necesarias.
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;
        await LoadReferencesAsync();
    }

    /// <summary>
    /// Carga todas las referencias necesarias para el componente, como centros de costo, zonas, servicios y canales de venta.
    /// </summary>
    private async Task LoadReferencesAsync()
    {

        var user = (await AuthenticationState).User;

        var franquicias = user.GetFranquicias();

        var franquicia = franquicias is null ? 1 : franquicias[0];

        Task[] loadingTasks =
        [
            LoadCentrosCosto(), LoadEscenariosCupoAsync(), LoadServiciosAsync(franquicia), LoadCanalesVentaAsync(), LoadDiasSemana()
        ];

        await Task.WhenAll(loadingTasks);
    }

    /// <summary>
    /// Carga la lista de días de la semana desde el servidor.
    /// </summary>
    private async Task LoadDiasSemana()
    {
        var result = await Mediator.SendRequest(new ListDiasSemana());
        result.Switch(
            onSuccess: list => diasSemana = list,
            onFailure: failure => AddFailure(failure, "cargar canales de venta"));
    }

    /// <summary>
    /// Carga la lista de centros de costo desde el servidor.
    /// </summary>
    private async Task LoadCentrosCosto()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user!));
        result.Switch(
            onSuccess: list => centrosCosto = list,
            onFailure: failure => AddFailure(failure, "cargar centros de costo"));
    }

    /// <summary>
    /// Carga la lista de servicios por franquicia.
    /// </summary>
    /// <param name="franquiciaId">ID de la franquicia para la cual se desean cargar los servicios.</param>
    private async Task LoadServiciosAsync(int franquiciaId)
    {
        var result = await Mediator.SendRequest(new ListServicios(franquiciaId));
        result.Switch(
            onSuccess: list => servicios = list,
            onFailure: failure => AddFailure(failure, "cargar servicios"));
    }

    /// <summary>
    /// Carga la lista de canales de venta desde el servidor.
    /// </summary>
    private async Task LoadCanalesVentaAsync()
    {
        var result = await Mediator.SendRequest(new ListCanalesVenta());
        result.Switch(
            onSuccess: list => canalesVenta = list,
            onFailure: failure => Snackbar.AddFailure(failure, "cargar los canales de venta"));
    }

    /// <summary>
    /// Carga la lista de escenarios de cupo desde el servidor.
    /// </summary>
    private async Task LoadEscenariosCupoAsync()
    {
        var result = await Mediator.SendRequest(new ListZonas());
        result.Switch(
            onSuccess: list => zonas = list,
            onFailure: failure => AddFailure(failure, "cargar zonas"));
    }

    /// <summary>
    /// Busca los escenarios de cupo según el estado de la grilla y los parámetros de búsqueda.
    /// </summary>
    /// <param name="state">El estado actual de la grilla, que incluye información de filtrado, paginación y ordenación.</param>
    private async Task<GridData<EscenarioCupoModel>> SearchEscenariosCupoAsync(GridState<EscenarioCupoModel> state)
    {
        SearchEscenariosCupo searchEscenariosCupos = state.ToSearch(user!, zonas.Select(z => z.Id).ToArray(), searchText);
        var result = await Mediator.SendRequest(searchEscenariosCupos);
        result.Switch(
            onSuccess: escenarios => currentEscenariosCupo = escenarios.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar escenarios cupo"));
        return currentEscenariosCupo;
    }



    /// <summary>
    /// Guarda un nuevo escenario de cupo en el servidor.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo que se desea guardar.</param>
    private async Task<bool> SaveEscenarioCupoAsync(EscenarioCupoModel escenarioCupo, IEnumerable<DetalleEscenarioCupoInfo> detalles, IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusiones)
    {
        CreateEscenarioCupo changeStatus = escenarioCupo.ToCreate();
        var result = await Mediator.SendRequest(changeStatus);
        return await result.MatchAsync(
           onSuccess: async (y, x) =>
           {
               await SaveEscenarioCupoDetallesAsync(y.Id, detalles);

               await SaveDetallesEscenarioCupoExclusionesPorFechaAsync(y.Id, exclusiones);

               return true;

           },
           onFailure: (failure) => AddFailure(failure, "crear escenario cupo")
       );
    }


    /// <summary>
    /// Guarda los detalles del escenario cupo que se acaba de crear.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo que se desea guardar.</param>
    private async Task<bool> SaveEscenarioCupoDetallesAsync(int escenarioCupoId, IEnumerable<DetalleEscenarioCupoInfo> detalles)
    {

        foreach (var detalle in detalles)
        {
            detalle.EscenarioCupoId = escenarioCupoId;
        }

        CreateDetalleEscenarioCupo changeStatus = new(escenarioCupoId, detalles);

        var result = await Mediator.SendRequest(changeStatus);

        return result.Match(
           onSuccess: (y) => true,
           onFailure: (failure) => AddFailure(failure, "guardar detalles escenario cupo")
       );
    }

    /// <summary>
    /// Guarda las exclusiones por fecha del escenario cupo que se acaba de crear.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo que se desea guardar.</param>
    private async Task<bool> SaveDetallesEscenarioCupoExclusionesPorFechaAsync(int escenarioCupoId, IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusiones)
    {

        foreach (var exclusion in exclusiones)
        {
            exclusion.EscenarioCupoId = escenarioCupoId;
        }

        CreateDetalleEscenarioCupoExclusionFecha changeStatus = new(escenarioCupoId, exclusiones);

        var result = await Mediator.SendRequest(changeStatus);

        return result.Match(
           onSuccess: _ => true,
           onFailure: (failure) => AddFailure(failure, "guardar detalles escenario cupo")
       );
    }



    /// <summary>
    /// Actualiza las exclusiones de un escenario de cupo en el servidor.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo que se desea actualizar.</param>
    private async Task<bool> UpdateEscenarioCupoExclusionsAsync(IEnumerable<DetalleEscenarioCupoExclusionFullInfo> exclusiones)
    {
        UpdateDetalleEscenarioExclusion changeStatus = exclusiones.ToUpdateExclusions(exclusiones.First().EscenarioCupoId);

        var result = await Mediator.SendRequest(changeStatus);

        return result.Match(
           onSuccess: (info) => true,
           onFailure: (failure) => AddFailure(failure, "modificar escenario cupo exclusiones")
       );
    }

    /// <summary>
    /// Actualiza un escenario de cupo existente en el servidor.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo que se desea actualizar.</param>
    private async Task<bool> UpdateEscenarioCupoAsync(EscenarioCupoModel escenarioCupo)
    {
        UpdateEscenarioCupo changeStatus = escenarioCupo.ToUpdate();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateEscenarioCupo(escenarioCupo, result, "modificar escenario cupo");
    }

    /// <summary>
    /// Cambia el estado activo/inactivo de un escenario de cupo.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo cuyo estado se desea cambiar.</param>
    private async Task<bool> ChangeStatusAsync(EscenarioCupoModel escenarioCupo)
    {
        escenarioCupo.EsActivo = !escenarioCupo.EsActivo;
        return await UpdateEscenarioCupoAsync(escenarioCupo).ConfigureAwait(false);
    }

    /// <summary>
    /// Cambia la propiedad de control de holgura (`EsHoraInicio`) de un escenario de cupo.
    /// </summary>
    /// <param name="escenarioCupo">El modelo de escenario de cupo cuyo control de holgura se desea cambiar.</param>
    private async Task<bool> ChangeControlaHolgura(EscenarioCupoModel escenarioCupo)
    {
        escenarioCupo.EsHoraInicio = !escenarioCupo.EsHoraInicio;
        return await UpdateEscenarioCupoAsync(escenarioCupo).ConfigureAwait(false);
    }

    /// <summary>
    /// Método auxiliar para actualizar un escenario de cupo y manejar los errores.
    /// </summary>
    /// <param name="model">El modelo de escenario de cupo que se desea actualizar.</param>
    /// <param name="result">Resultado de la operación de actualización.</param>
    /// <param name="action">Acción que se está ejecutando, para propósitos de logging y mensajes de error.</param>
    private bool UpdateEscenarioCupo(EscenarioCupoModel model, ResultOf<EscenarioCupoFullInfo> result, string action)
    {
        return result.Match(
           onSuccess: (info) => UpdateEscenarioCupo(model, info),
           onFailure: (failure) => AddFailure(failure, action)
       );
    }

    /// <summary>
    /// Método auxiliar para actualizar las exclusiones de un escenario de cupo y manejar los errores.
    /// </summary>
    /// <param name="model">El modelo de escenario de cupo que se desea actualizar.</param>
    /// <param name="result">Resultado de la operación de actualización.</param>
    /// <param name="action">Acción que se está ejecutando, para propósitos de logging y mensajes de error.</param>
    private bool UpdateEscenarioCupoExclusions(EscenarioCupoModel model, SuccessOrFailure result, string action)
    {
        return result.Match(
           onSuccess: (info) => UpdateEscenarioCupoExclusion(model),
           onFailure: (failure) => AddFailure(failure, action)
       );
    }

    /// <summary>
    /// Actualiza un escenario de cupo existente en la lista local y sus exclusiones.
    /// </summary>
    /// <param name="model">El modelo de escenario de cupo que se desea actualizar.</param>
    private bool UpdateEscenarioCupoExclusion(EscenarioCupoModel model)
    {
        EscenarioCupoModel? persistent = currentEscenariosCupo?.Find(c => c.Id == model.Id);

        if (persistent is not null)
        {
            persistent.Update(model);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Actualiza el modelo de un escenario de cupo con nueva información recibida.
    /// </summary>
    /// <param name="model">El modelo de escenario de cupo que se desea actualizar.</param>
    /// <param name="info">Información completa del escenario de cupo obtenida del servidor.</param>
    private bool UpdateEscenarioCupo(EscenarioCupoModel model, EscenarioCupoFullInfo info)
    {
        model.Save(info);

        EscenarioCupoModel? persistent = currentEscenariosCupo?.Find(c => c.Id == model.Id);

        if (persistent is not null)
        {
            persistent.Update(model);
            return true;
        }

        return false;
    }


    private async Task<bool> UpdateDetallesEscenarioCupo(int escenarioCupoId, IEnumerable<DetalleEscenarioCupoInfo> detalles)
    {
        UpdateDetalleEscenarioCupo request = new(escenarioCupoId, detalles);

        var result = await Mediator.SendRequest(request);

        return result.Match(
            onSuccess: _ => true,
            onFailure: failure =>
            {
                Snackbar.AddFailure(failure, "actualizar detalles del escenario");
                return false;
            });
    }

    private async Task<bool> UpdateExclusionesPorFechaEscenarioCupo(int escenarioCupoId, IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusionesFecha)
    {
        UpdateDetalleEscenarioCupoExclusionFecha request = new(escenarioCupoId, exclusionesFecha);

        var result = await Mediator.SendRequest(request);

        return result.Match(
            onSuccess: _ => true,
            onFailure: failure =>
            {
                Snackbar.AddFailure(failure, "actualizar exclusiones por fecha del escenario");
                return false;
            });
    }


    /// <summary>
    /// Elimina los escenarios de cupo seleccionados, después de solicitar confirmación al usuario.
    /// </summary>
    private async Task DeleteEscenarioCupo()
    {
        bool res = await DialogService.ShowConfirmationDialogAsync("Confirmación", "¿Está seguro que desea eliminar los escenarios seleccionados?", "Sí", "No");

        if (res)
        {
            viewModel.Loading = true;

            var result = await Mediator.SendRequest(escenariosCupoToDelete.ToDelete());
            await result.SwitchAsync(
                    onSuccess: DeleteFechasExcluidasSelectedAsync,
                    onFailure: failure => AddFailure(failure, "eliminar fechas excluidas para cupos"));

            viewModel.Loading = false;
        }
    }

    /// <summary>
    /// Elimina las fechas excluidas seleccionadas después de eliminar un escenario de cupo.
    /// </summary>
    /// <param name="success">Resultado de éxito de la operación de eliminación.</param>
    /// <param name="cancellationToken">Token de cancelación para la tarea.</param>
    private async Task DeleteFechasExcluidasSelectedAsync(Success success, CancellationToken cancellationToken)
    {
        escenariosCupoToDelete.Clear();
        await RefreshAsync(cancellationToken);
    }

    /// <summary>
    /// Recarga la grilla de escenarios de cupo después de una actualización o eliminación.
    /// </summary>
    /// <param name="escenario">El escenario actualizado que se desea reflejar en la grilla.</param>
    /// <param name="cancellationToken">Token de cancelación para la tarea.</param>
    private async Task<bool> RefreshAsync(EscenarioCupoFullInfo escenario, CancellationToken cancellationToken)
    {
        await dataGrid.ReloadServerData();
        return true;
    }

    /// <summary>
    /// Recarga la grilla de escenarios de cupo después de una actualización o eliminación.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la tarea.</param>
    private async Task<bool> RefreshAsync(CancellationToken cancellationToken)
    {
        await dataGrid.ReloadServerData();
        return true;
    }

    /// <summary>
    /// Maneja el registro de errores y muestra un mensaje al usuario.
    /// </summary>
    /// <param name="failure">El fallo ocurrido durante la operación.</param>
    /// <param name="action">La acción que se intentó realizar cuando ocurrió el fallo.</param>
    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        loading = false;
        return false;
    }

    /// <summary>
    /// Muestra el formulario para editar un escenario de cupo seleccionado.
    /// </summary>
    /// <param name="context">El contexto de la celda seleccionada.</param>
    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
    {
        { nameof(EscenarioCupoCreateDialog.CentrosCostos), centrosCosto },
        { nameof(EscenarioCupoCreateDialog.Zonas), zonas },
        { nameof(EscenarioCupoCreateDialog.Servicios), servicios },
        { nameof(EscenarioCupoCreateDialog.CanalesVenta), canalesVenta },
    };

        var options = new DialogOptions
        {
            FullScreen = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<EscenarioCupoCreateDialog>("Crear Escenario de Cupo", parameters, options);

        var result = await dialog.Result;


        if (result != null && !result.Canceled)
        {
            var response = result.Data as EscenarioCupoDataResponse;

            if (response is null)
            {
                Snackbar.Add("No se recibió data actualizada.", Severity.Warning, config =>
                {
                    config.VisibleStateDuration = 2000;
                    config.RequireInteraction = false;
                });

                return;
            }

            EscenarioCupoModel? createdEscenarioCupo = response.EscenarioCupo;
            List<DetalleEscenarioCupoInfo> detalles = response.Detalles;
            List<DetalleEscenarioCupoExclusionFechaFullInfo> exclusiones = response.ExclusionesPorFecha;

            await SaveEscenarioCupoAsync(createdEscenarioCupo, detalles, exclusiones);

        }


        //--------------------------------------

        //if (result != null && !result.Canceled)
        //{
        //    EscenarioCupoModel? createdEscenarioCupo = result.Data as EscenarioCupoModel;
        //    if (createdEscenarioCupo != null)
        //    {
        //        viewModel.Loading = true;

        //        StateHasChanged();

        //        Snackbar.Add($"Guardando Escenario '{createdEscenarioCupo.Nombre}' ...", Severity.Info, config =>
        //        {
        //            config.VisibleStateDuration = 2000;
        //            config.RequireInteraction = false;
        //        });

        //        await SaveEscenarioCupoAsync(createdEscenarioCupo, createdEscenarioCupo);

        //        Snackbar.Add("Escenario guardado exitosamente.", Severity.Success, config =>
        //        {
        //            config.VisibleStateDuration = 2000;
        //            config.RequireInteraction = false;
        //        });


        //        viewModel.Loading = false;

        //        StateHasChanged();
        //    }
        //}
    }

    /// <summary>
    /// Muestra el formulario para editar un escenario de cupo seleccionado.
    /// </summary>
    /// <param name="context">El contexto de la celda seleccionada.</param>
    private async Task ShowEscenarioCupoEditingAsync(CellContext<EscenarioCupoModel> context)
    {
        var cloned = dataGrid.CloneStrategy.CloneObject(context.Item);

        selectedEscenarioCupo = cloned;

        var parameters = new DialogParameters
    {
        { nameof(EscenarioCupoEditorDialog.CentrosCostos), centrosCosto },
        { nameof(EscenarioCupoEditorDialog.Zonas), zonas },
        { nameof(EscenarioCupoEditorDialog.Servicios), servicios },
        { nameof(EscenarioCupoEditorDialog.CanalesVenta), canalesVenta },
        { nameof(EscenarioCupoEditorDialog.SelectedEscenarioCupo), selectedEscenarioCupo },
        { nameof(EscenarioCupoEditorDialog.ViewModel), viewModel }
    };

        var options = new DialogOptions
        {
            FullScreen = true,
            CloseButton = true,
            CloseOnEscapeKey = true,
        };

        var dialog = await DialogService.ShowAsync<EscenarioCupoEditorDialog>("Editar Escenario de Cupo", parameters, options);

        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            var response = result.Data as EscenarioCupoDataResponse;

            if (response is null)
            {
                Snackbar.Add("No se recibió data actualizada.", Severity.Warning, config =>
                {
                    config.VisibleStateDuration = 2000;
                    config.RequireInteraction = false;
                });

                return;
            }

            EscenarioCupoModel? updatedEscenarioCupo = response.EscenarioCupo;
            List<DetalleEscenarioCupoInfo> detalles = response.Detalles;
            List<DetalleEscenarioCupoExclusionFechaFullInfo> exclusiones = response.ExclusionesPorFecha;

            if (updatedEscenarioCupo != null)
            {
                Snackbar.Add($"Guardando Escenario '{updatedEscenarioCupo.Nombre}' ...", Severity.Info, config =>
                {
                    config.VisibleStateDuration = 2000;
                    config.RequireInteraction = false;
                });

                viewModel.Loading = true;

                bool isUpdated = await UpdateEscenarioCupoAsync(updatedEscenarioCupo);

                if (isUpdated)
                {
                    Snackbar.Add("Escenario actualizado exitosamente.", Severity.Success, config =>
                    {
                        config.VisibleStateDuration = 2000;
                        config.RequireInteraction = false;
                    });
                }
            }
            else
            {
                Snackbar.Add("No se detectaron cambios en el escenario. Se actualizarán los detalles y exclusiones.", Severity.Warning);
            }

            bool detallesUpdated = await UpdateDetallesEscenarioCupo(selectedEscenarioCupo!.Id, detalles);
            if (!detallesUpdated)
            {
                Snackbar.Add("Ocurrió un error al actualizar los detalles del escenario.", Severity.Error);
            }
            bool exclusionesUpdated = await UpdateExclusionesPorFechaEscenarioCupo(selectedEscenarioCupo!.Id, exclusiones);
            if (!exclusionesUpdated)
            {
                Snackbar.Add("Ocurrió un error al actualizar las exclusiones del escenario.", Severity.Error);
            }

            viewModel.Loading = false;
        }
    }


    /// <summary>
    /// Muestra el formulario para editar las exclusiones de un escenario de cupo seleccionado.
    /// </summary>
    /// <param name="context">El contexto de la celda seleccionada.</param>
    private Task ShowEditingExclusionsAsync(CellContext<EscenarioCupoModel> context)
    {
        var cloned = dataGrid.CloneStrategy.CloneObject(context.Item);

        if (cloned is not null)
        {
            selectedEscenarioCupo = cloned;
        }
        else
        {
            Snackbar.Add("Debe haberse seleccionado un Escenario.", Severity.Warning);
            return Task.CompletedTask;
        }


        var parameters = new DialogParameters
        {
            { nameof(ListEscenarioCupoExclusionsDialog.Servicios), servicios },
            { nameof(ListEscenarioCupoExclusionsDialog.CanalesVenta), canalesVenta },
            { nameof(ListEscenarioCupoExclusionsDialog.DiasSemana), diasSemana },
            { nameof(ListEscenarioCupoExclusionsDialog.SelectedEscenarioCupo), selectedEscenarioCupo },
            { nameof(ListEscenarioCupoExclusionsDialog.OnExclusionesUpdated), (Func<IEnumerable<DetalleEscenarioCupoExclusionFullInfo>, Task>)HandleExclusionFromEscenarioCupo }
        };

        var options = new DialogOptions
        {
            FullScreen = true,
            CloseButton = true,
            CloseOnEscapeKey = true

        };

        return DialogService.ShowAsync<ListEscenarioCupoExclusionsDialog>($"Exclusiones para {selectedEscenarioCupo!.Nombre}", parameters: parameters, options: options);
    }

    /// <summary>
    /// Maneja las exclusiones de un escenario de cupo y las actualiza en el servidor.
    /// </summary>
    /// <param name="updatedEscenario">El modelo de escenario de cupo actualizado con exclusiones modificadas.</param>
    private async Task HandleExclusionFromEscenarioCupo(IEnumerable<DetalleEscenarioCupoExclusionFullInfo> exclusiones)
    {
        await UpdateEscenarioCupoExclusionsAsync(exclusiones);
    }

    /// <summary>
    /// Maneja el cambio de selección de un elemento en la grilla.
    /// </summary>
    /// <param name="selectedItem">El item seleccionado de la grilla.</param>
    private async Task OnSelectedItemChanged(EscenarioCupoModel selectedItem)
    {
        bool wasRemoved = HandleSelection(selectedItem);

        if (wasRemoved)
        {
            Snackbar.Add($"{selectedItem.Nombre} no se puede eliminar ya que tiene cupos asociados.", Severity.Warning, config =>
            {
                config.VisibleStateDuration = 2000;
                config.RequireInteraction = false;
            });
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Maneja el cambio de selección de varios elementos en la grilla.
    /// </summary>
    /// <param name="selectedItems">Los items seleccionados de la grilla.</param>
    private async Task OnSelectedItemsChanged(HashSet<EscenarioCupoModel> selectedItems)
    {
        // Contar cuántos elementos no se pudieron marcar
        int notSelectableCount = selectedItems.Count(item => HandleSelection(item));

        if (notSelectableCount > 0)
        {
            string message = notSelectableCount == 1
                ? "1 elemento no se puede marcar para eliminación ya que tiene cupos asociados."
                : $"{notSelectableCount} elementos no se pueden marcar para eliminación ya que tienen cupos asociados.";

            Snackbar.Add(message, Severity.Warning, config =>
            {
                config.VisibleStateDuration = 2000;
                config.RequireInteraction = false;
            });
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Maneja la selección de un item en la grilla.
    /// </summary>
    /// <param name="item">El item que se intenta seleccionar o deseleccionar.</param>
    /// <returns>True si el item no se pudo seleccionar, false si se pudo seleccionar o no se requirió ninguna acción.</returns>
    private bool HandleSelection(EscenarioCupoModel item)
    {
        if (item is null) return false;

        if (!item.PuedeSerEliminado)
        {
            // Si el item no puede ser eliminado y estamos intentando seleccionarlo, retornamos true para indicar que no puede ser marcado
            return true;
        }
        else
        {
            // Si el item puede ser eliminado y no está aún en la lista, lo agregamos
            if (!escenariosCupoToDelete.Contains(item))
            {
                escenariosCupoToDelete.Add(item);
            }
        }

        return false; // El elemento se pudo seleccionar o no se requirió ninguna acción
    }
}
