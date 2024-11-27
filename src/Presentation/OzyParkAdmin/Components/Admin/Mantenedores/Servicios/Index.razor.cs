using MassTransit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Application.Cajas.List;
using OzyParkAdmin.Application.CentrosCosto.List;
using OzyParkAdmin.Application.Franquicias.List;
using OzyParkAdmin.Application.Plantillas.List;
using OzyParkAdmin.Application.Servicios.Activar;
using OzyParkAdmin.Application.Servicios.Desactivar;
using OzyParkAdmin.Application.Servicios.Find;
using OzyParkAdmin.Application.Servicios.List;
using OzyParkAdmin.Application.Servicios.Search;
using OzyParkAdmin.Application.Servicios;
using OzyParkAdmin.Application.Tramos.List;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Shared;
using System.Security.Claims;
using OzyParkAdmin.Application.GruposEtarios.List;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios;

/// <summary>
/// Página del mantenedor de servicios.
/// </summary>
public partial class Index
{
    private ClaimsPrincipal? user;
    private MudDataGrid<ServicioViewModel> dataGrid = default!;
    private ObservableGridData<ServicioViewModel> currentServicios = new();
    private string? searchText;
    private List<FranquiciaInfo> franquicias = [];
    private List<CentroCostoInfo> centrosCosto = [];
    private List<TipoControl> tiposControl = [];
    private List<TipoDistribucion> tiposDistribucion = [];
    private List<TipoVigencia> tiposVigencia = [];
    private List<TramoInfo> tramos = [];
    private List<GrupoEtarioInfo> gruposEtarios = [];
    private List<CajaInfo> cajas = [];
    private List<ServicioInfo> servicios = [];
    private List<Plantilla> plantillas = [];

    private bool openEditing;
    private bool openCentrosCosto;
    private bool openTramos;
    private bool openPermisos;
    private bool openGruposEtarios;
    private bool openCajas;
    private ServicioViewModel? currentServicio;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationState).User;
        await LoadReferencesAsync();
    }

    private async Task LoadReferencesAsync()
    {
        Task[] loadingTasks =
        [
            LoadCentrosCostoAsync(), LoadFranquiciasAsync(), LoadTiposControlAsync(),
            LoadTiposDistribucionAsync(), LoadTiposVigenciaAsync(), LoadTramosAsync(),
            LoadGruposEtariosAsync(), LoadCajasAsync(), LoadPlantillasAsync()
        ];

        await Task.WhenAll(loadingTasks);
    }

    private async Task LoadCentrosCostoAsync()
    {
        var result = await Mediator.SendRequest(new ListCentrosCosto(user!));
        result.Switch(
            onSuccess: list => centrosCosto = list,
            onFailure: failure => AddFailure(failure, "cargar centros de costo"));
    }

    private async Task LoadFranquiciasAsync()
    {
        var result = await Mediator.SendRequest(new ListFranquicias(user!));
        result.Switch(
            onSuccess: list => franquicias = list,
            onFailure: failure => AddFailure(failure, "cargar franquicias"));
    }

    private async Task LoadTiposControlAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposControl());
        result.Switch(
            onSuccess: list => tiposControl = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de control"));
    }

    private async Task LoadTiposDistribucionAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposDistribucion());
        result.Switch(
            onSuccess: list => tiposDistribucion = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de distribución"));
    }

    private async Task LoadTiposVigenciaAsync()
    {
        var result = await Mediator.SendRequest(new ListTiposVigencia());
        result.Switch(
            onSuccess: list => tiposVigencia = list,
            onFailure: failure => AddFailure(failure, "cargar tipos de vigencia"));
    }

    private async Task LoadTramosAsync()
    {
        var result = await Mediator.SendRequest(new ListTramos());
        result.Switch(
            onSuccess: list => tramos = list,
            onFailure: failure => AddFailure(failure, "cargar tramos"));
    }

    private async Task LoadGruposEtariosAsync()
    {
        var result = await Mediator.SendRequest(new ListGruposEtarios());
        result.Switch(
            onSuccess: list => gruposEtarios = list,
            onFailure: failure => AddFailure(failure, "cargar grupos etarios"));
    }

    private async Task LoadCajasAsync()
    {
        var result = await Mediator.SendRequest(new ListCajas(user!));
        result.Switch(
            onSuccess: list => cajas = list,
            onFailure: failure => AddFailure(failure, "cargar cajas"));
    }

    private async Task LoadPlantillasAsync()
    {
        var result = await Mediator.SendRequest(new ListPlantillas());
        result.Switch(
            onSuccess: list => plantillas = list,
            onFailure: failure => AddFailure(failure, "cargar plantillas"));
    }

    private async Task LoadServiciosAsync(int franquiciaId)
    {
        var result = await Mediator.SendRequest(new ListServicios(franquiciaId));
        result.Switch(
            onSuccess: list => servicios = list,
            onFailure: failure => AddFailure(failure, "cargar servicios"));
    }

    private async Task<GridData<ServicioViewModel>> SearchServiciosAsync(GridState<ServicioViewModel> state)
    {
        SearchServicios searchServicios = state.ToSearch(user!, searchText);
        var result = await Mediator.SendRequest(searchServicios);
        result.Switch(
            onSuccess: servicios => currentServicios = servicios.ToGridData(dataGrid),
            onFailure: failure => AddFailure(failure, "buscar servicios"));

        return currentServicios;
    }

    private async Task AddServicioAsync()
    {
        currentServicio = new()
        {
            IsNew = true,
            FranquiciaId = franquicias.FirstOrDefault()?.Id ?? 0,
            CentroCosto = centrosCosto.FirstOrDefault() ?? new(),
            TipoDistribucion = tiposDistribucion.FirstOrDefault() ?? default!,
            TipoVigencia = tiposVigencia.FirstOrDefault() ?? default!,
            TipoControl = tiposControl.FirstOrDefault() ?? default!,
            PlantillaId = plantillas.FirstOrDefault()?.Id ?? 0,
            PlantillaDigitalId = plantillas.FirstOrDefault()?.Id ?? 0,
        };

        CellContext<ServicioViewModel> context = new(dataGrid, currentServicio);
        await ShowEditingAsync(context);
    }

    private async Task ShowEditingAsync(CellContext<ServicioViewModel> context)
    {
        await LoadServiciosAsync(context.Item.FranquiciaId);
        currentServicio = context.Item.Copy();
        openEditing = true;
    }

    private async Task ShowCentrosCostoAsync(CellContext<ServicioViewModel> context)
    {
        if (await LoadServicioDetailAsync(context.Item))
        {
            currentServicio = context.Item;
            openCentrosCosto = true;
        }
    }

    private async Task ShowTramosAsync(CellContext<ServicioViewModel> context)
    {
        if (await LoadServicioDetailAsync(context.Item))
        {
            currentServicio = context.Item;
            openTramos = true;
        }
    }

    private async Task ShowPermisosAsync(CellContext<ServicioViewModel> context)
    {
        if (await LoadServicioDetailAsync(context.Item))
        {
            currentServicio = context.Item;
            openPermisos = true;
        }
    }

    private async Task ShowGruposEtariosAsync(CellContext<ServicioViewModel> context)
    {
        if (await LoadServicioDetailAsync(context.Item))
        {
            currentServicio = context.Item;
            openGruposEtarios = true;
        }
    }

    private async Task ShowCajasAsync(CellContext<ServicioViewModel> context)
    {
        if (await LoadServicioDetailAsync(context.Item))
        {
            currentServicio = context.Item;
            openCajas = true;
        }
    }

    private async Task<bool> LoadServicioDetailAsync(ServicioViewModel servicio)
    {
        if (!servicio.DetailLoaded)
        {
            ResultOf<ServicioFullInfo> result = await Mediator.SendRequest(new FindServicio(servicio.Id));

            return result.Match(
                onSuccess: info => LoadServicioInfo(servicio, info),
                onFailure: failure => AddFailure(failure, "cargar el detalle del servicio"));
        }

        return true;
    }

    private static bool LoadServicioInfo(ServicioViewModel servicio, ServicioFullInfo info)
    {
        servicio.DetailLoaded = true;
        servicio.Cajas = info.Cajas.ToModel();
        servicio.CentrosCosto = info.CentrosCosto.ToModel();
        servicio.GruposEtarios = [..info.GruposEtarios];
        servicio.Permisos = info.Permisos.ToModel();
        servicio.Tramos = info.Tramos.ToModel();
        return true;
    }

    private async Task<bool> SaveServicioAsync(ServicioViewModel servicio)
    {
        IServicioStateChangeable changeStatus = servicio.IsNew
            ? servicio.ToCreate()
            : servicio.ToUpdate();

        var result = await Mediator.SendRequest(changeStatus);
        return UpdateServicio(servicio, result, servicio.IsNew ? "crear servicio" : "modificar servicio");
    }

    private async Task SaveEsActivoAsync(ServicioViewModel servicio, bool esActivo)
    {
        IServicioStateChangeable changeStatus = esActivo
            ? new ActivarServicio(servicio.Id)
            : new DesactivarServicio(servicio.Id);

        var result = await Mediator.SendRequest(changeStatus);
        UpdateServicio(servicio, result, esActivo ? "activar servicio" : "desactivar servicio");
    }

    private async Task<bool> SaveCentrosCostoAsync(ServicioViewModel servicio)
    {
        IServicioStateChangeable changeStatus = servicio.ToAssignCentrosCosto();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateServicio(servicio, result, "asignar centros de costo");
    }

    private async Task<bool> SaveTramosAsync(ServicioViewModel servicio)
    {
        IServicioStateChangeable changeStatus = servicio.ToAssignTramos();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateServicio(servicio, result, "asignar tramos");
    }

    private async Task<bool> SavePermisosAsync(ServicioViewModel servicio)
    {
        IServicioStateChangeable changeStatus = servicio.ToAssignPermisos();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateServicio(servicio, result, "asignar permisos");
    }

    private async Task<bool> SaveGruposEtariosAsync(ServicioViewModel servicio)
    {
        IServicioStateChangeable changeStatus = servicio.ToAssignGruposEtarios();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateServicio(servicio, result, "asignar grupos etarios");
    }

    private async Task<bool> SaveCajasAsync(ServicioViewModel servicio)
    {
        IServicioStateChangeable changeStatus = servicio.ToAssignCajas();
        var result = await Mediator.SendRequest(changeStatus);
        return UpdateServicio(servicio, result, "asignar cajas");
    }

    private bool UpdateServicio(ServicioViewModel servicio, ResultOf<ServicioFullInfo> result, string action)
    {
        return result.Match(
           onSuccess: (info) => UpdateServicio(servicio, info),
           onFailure: (failure) => AddFailure(failure, action)
       );
    }

    private bool UpdateServicio(ServicioViewModel servicio, ServicioFullInfo info)
    {
        bool isNew = servicio.IsNew;
        servicio.Update(info);

        if (isNew)
        {
            currentServicios?.Add(servicio);
            return true;
        }

        ServicioViewModel? persistent = currentServicios?.Find(servicio);

        if (persistent is not null)
        {
            persistent.Update(servicio);
            return true;
        }

        return false;
    }

    private bool AddFailure(Failure failure, string action)
    {
        Snackbar.AddFailure(failure, action);
        return false;
    }
}
