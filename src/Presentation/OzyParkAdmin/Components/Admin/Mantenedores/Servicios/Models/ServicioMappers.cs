using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Servicios.Assigns;
using OzyParkAdmin.Application.Servicios.Create;
using OzyParkAdmin.Application.Servicios.Search;
using OzyParkAdmin.Application.Servicios.Update;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Components.Admin.Shared;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Shared;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

internal static class ServicioMappers
{
    public static SearchServicios ToSearch(this GridState<ServicioViewModel> state, ClaimsPrincipal user, string? searchText) =>
        new(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<Servicio> ToFilterExpressions(this GridState<ServicioViewModel> state)
    {
        FilterExpressionCollection<Servicio> filterExpressions = new();

        foreach (var filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<ServicioViewModel> filterDefinition, FilterExpressionCollection<Servicio> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            "CentroCosto.Nombre" => filterExpressions.Add(x => x.CentroCosto.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.Aka) => filterExpressions.Add(x => x.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.Nombre) => filterExpressions.Add(x => x.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.TipoServicio) => filterExpressions.Add(x => x.TipoServicio, filterDefinition.Operator!, filterDefinition.Value),
            "TipoDistribucion.Nombre" => filterExpressions.Add(x => x.TipoDistribucion.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "TipoVigencia.Nombre" => filterExpressions.Add(x => x.TipoVigencia.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "TipoControl.Nombre" => filterExpressions.Add(x => x.TipoControl.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.Orden) => filterExpressions.Add(x => x.Orden, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.EsConHora) => filterExpressions.Add(x => x.EsConHora, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.EsActivo) => filterExpressions.Add(x => x.EsActivo, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.EsParaVenta) => filterExpressions.Add(x => x.EsParaVenta, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<Servicio> ToSortExpressions(this GridState<ServicioViewModel> state)
    {
        SortExpressionCollection<Servicio> sortExpressions = new();

        foreach (var sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<ServicioViewModel> sortDefinition, SortExpressionCollection<Servicio> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            "CentroCosto.Nombre" => sortExpressions.Add(x => x.CentroCosto.Descripcion, sortDefinition.Descending),
            nameof(ServicioViewModel.Aka) => sortExpressions.Add(x => x.Aka, sortDefinition.Descending),
            nameof(ServicioViewModel.Nombre) => sortExpressions.Add(x => x.Nombre, sortDefinition.Descending),
            nameof(ServicioViewModel.TipoServicio) => sortExpressions.Add(x => x.TipoServicio, sortDefinition.Descending),
            "TipoDistribucion.Nombre" => sortExpressions.Add(x => x.TipoDistribucion.Descripcion, sortDefinition.Descending),
            "TipoVigencia.Nombre" => sortExpressions.Add(x => x.TipoVigencia.Descripcion, sortDefinition.Descending),
            "TipoControl.Nombre" => sortExpressions.Add(x => x.TipoControl.Aka, sortDefinition.Descending),
            nameof(ServicioViewModel.Orden) => sortExpressions.Add(x => x.Orden, sortDefinition.Descending),
            nameof(ServicioViewModel.EsConHora) => sortExpressions.Add(x => x.EsConHora, sortDefinition.Descending),
            nameof(ServicioViewModel.EsActivo) => sortExpressions.Add(x => x.EsActivo, sortDefinition.Descending),
            nameof(ServicioViewModel.EsParaVenta) => sortExpressions.Add(x => x.EsParaVenta, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<ServicioViewModel> ToGridData(this PagedList<ServicioFullInfo> servicios, IMudStateHasChanged mudStateHasChanged) =>
        new ObservableGridData<ServicioViewModel, int>(servicios.Items.Select(ToViewModel), servicios.TotalCount, mudStateHasChanged, x => x.Id);

    private static ServicioViewModel ToViewModel(ServicioFullInfo servicio) =>
        new()
        {
            Id = servicio.Id,
            Aka = servicio.Aka,
            Nombre = servicio.Nombre,
            FranquiciaId = servicio.FranquiciaId,
            CentroCosto = servicio.CentroCosto.ToModel(),
            TipoControl = servicio.TipoControl.ToModel(),
            TipoDistribucion = servicio.TipoDistribucion.ToModel(),
            TipoServicio = servicio.TipoServicio,
            TipoVigencia = servicio.TipoVigencia.ToModel(),
            NumeroVigencia = servicio.NumeroVigencia,
            NumeroValidez = servicio.NumeroValidez,
            NumeroPaxMinimo = servicio.NumeroPaxMinimo,
            NumeroPaxMaximo = servicio.NumeroPaxMaximo,
            EsConHora = servicio.EsConHora,
            EsPorTramos = servicio.EsPorTramos,
            EsParaVenta = servicio.EsParaVenta,
            Orden = servicio.Orden,
            HolguraInicio = servicio.HolguraInicio,
            HolguraFin = servicio.HolguraFin,
            EsParaMovil = servicio.EsParaMovil,
            MostrarTramos = servicio.MostrarTramos,
            EsParaBuses = servicio.EsParaBuses,
            IdaVuelta = servicio.IdaVuelta,
            HolguraEntrada = servicio.HolguraEntrada,
            Politicas = servicio.Politicas,
            ControlParental = servicio.ControlParental,
            ServicioResponsableId = servicio.ServicioResponsableId,
            Tramos = servicio.Tramos.ToModel(),
            CentrosCosto = servicio.CentrosCosto.ToModel(),
            GruposEtarios = servicio.GruposEtarios.ToModel(),
            Cajas = servicio.Cajas.ToModel(),
            Zonas = servicio.Zonas.ToModel(),
            Permisos = servicio.Permisos.ToModel(),
            PlantillaId = servicio.PlantillaId,
            PlantillaDigitalId = servicio.PlantillaDigitalId,
            EsActivo = servicio.EsActivo,
        };

    public static List<TipoControlModel> ToModel(this IEnumerable<TipoControl> source) =>
        source.Select(ToModel).ToList();
    public static TipoControlModel ToModel(this TipoControl tipoControl) =>
        new() { Id = tipoControl.Id, Nombre = tipoControl.Aka, EsActivo = tipoControl.EsActivo };

    public static List<TipoDistribucionModel> ToModel(this IEnumerable<TipoDistribucion> source) =>
        source.Select(ToModel).ToList();

    public static TipoDistribucionModel ToModel(this TipoDistribucion tipoDistribucion) =>
        new() { Id = tipoDistribucion.Id, Nombre = tipoDistribucion.Descripcion, EsActivo = tipoDistribucion.EsActivo };

    public static List<TipoVigenciaModel> ToModel(this IEnumerable<TipoVigencia> source) =>
        source.Select(ToModel).ToList();

    public static TipoVigenciaModel ToModel(this TipoVigencia tipoVigencia) =>
        new() { Id = tipoVigencia.Id, Aka = tipoVigencia.Aka, Nombre = tipoVigencia.Descripcion, EsActivo = tipoVigencia.EsActivo };

    public static List<TramoServicioModel> ToModel(this IEnumerable<TramoServicioInfo> source) =>
        source.Select(ToModel).ToList();

    private static TramoServicioModel ToModel(TramoServicioInfo tramoServicio) =>
        new() { CentroCosto = tramoServicio.CentroCosto.ToModel(), Tramo = tramoServicio.Tramo.ToModel(), Nombre = tramoServicio.Nombre, CantidadPersmisos = tramoServicio.CantidadPermisos };

    public static List<TramoModel> ToModel(this IEnumerable<TramoInfo> source) =>
        source.Select(ToModel).ToList();

    public static TramoModel ToModel(this TramoInfo tramo) =>
        new() { Id = tramo.Id, Aka = tramo.Aka, Nombre = tramo.Descripcion };

    public static List<CentroCostoServicioModel> ToModel(this IEnumerable<CentroCostoServicioInfo> source) =>
        source.Select(ToModel).ToList();

    public static CentroCostoServicioModel ToModel(this CentroCostoServicioInfo centroCostoServicio) =>
        new() { CentroCosto = centroCostoServicio.CentroCosto.ToModel(), Nombre = centroCostoServicio.Nombre };

    public static List<GrupoEtarioModel> ToModel(this IEnumerable<GrupoEtarioInfo> source) =>
        source.Select(ToModel).ToList();

    private static GrupoEtarioModel ToModel(GrupoEtarioInfo grupoEtario) =>
        new() { Id = grupoEtario.Id, Aka = grupoEtario.Aka, Nombre = grupoEtario.Descripcion };

    public static List<CajaServicioModel> ToModel(this IEnumerable<ServicioPorCajaInfo> source) =>
        source.Select(ToModel).ToList();

    private static CajaServicioModel ToModel(ServicioPorCajaInfo servicioPorCaja) =>
        new() { Caja = servicioPorCaja.Caja.ToModel(), NoUsaZona = servicioPorCaja.NoUsaZona };

    public static List<CajaModel> ToModel(this IEnumerable<CajaInfo> source) =>
        source.Select(ToModel).ToList();

    private static CajaModel ToModel(this CajaInfo cajaInfo) =>
        new() { Id = cajaInfo.Id, Aka = cajaInfo.Aka, Nombre = cajaInfo.Descripcion };

    public static List<ZonaTramoModel> ToModel(this IEnumerable<ZonaPorTramoInfo> source) =>
        source.Select(ToModel).ToList();

    private static ZonaTramoModel ToModel(ZonaPorTramoInfo zonaPorTramo) =>
        new() { Tramo = zonaPorTramo.Tramo.ToModel(), Zona = zonaPorTramo.Zona.ToModel(), EsRetorno = zonaPorTramo.EsRetorno, EsCombinacion = zonaPorTramo.EsCombinacion, Orden = zonaPorTramo.Orden, EsActivo = zonaPorTramo.EsActivo };

    public static List<ZonaModel> ToModel(this IEnumerable<ZonaInfo> source) =>
        source.Select(ToModel).ToList();

    public static ZonaModel ToModel(this ZonaInfo zona) =>
        new() { Id = zona.Id, Nombre = zona.Descripcion };

    public static List<PermisoServicioModel> ToModel(this IEnumerable<PermisoServicioInfo> source) =>
        source.Select(ToModel).ToList();

    private static PermisoServicioModel ToModel(PermisoServicioInfo permisoServicio) =>
        new() { Tramo = permisoServicio.Tramo.ToModel(), CentroCosto = permisoServicio.CentroCosto.ToModel() };

    public static List<ServicioModel> ToModel(this IEnumerable<ServicioInfo> source) =>
        source.Select(ToModel).ToList();

    private static ServicioModel ToModel(ServicioInfo servicio) =>
        new(servicio.Id, servicio.Aka, servicio.Nombre);

    public static CreateServicio ToCreate(this ServicioViewModel servicio) =>
        new(
            servicio.CentroCosto.ToInfo(),
            servicio.FranquiciaId,
            servicio.Aka,
            servicio.Nombre,
            servicio.TipoControl.ToEntity(),
            servicio.TipoDistribucion.ToEntity(),
            servicio.TipoServicio,
            servicio.TipoVigencia.ToEntity(),
            servicio.NumeroVigencia,
            servicio.NumeroValidez,
            servicio.NumeroPaxMinimo,
            servicio.NumeroPaxMaximo,
            servicio.EsConHora,
            servicio.EsPorTramos,
            servicio.EsParaVenta,
            servicio.Orden,
            servicio.HolguraInicio,
            servicio.HolguraFin,
            servicio.EsParaMovil,
            servicio.MostrarTramos,
            servicio.EsParaBuses,
            servicio.IdaVuelta,
            servicio.HolguraEntrada,
            servicio.ControlParental,
            servicio.ServicioResponsableId,
            servicio.Politicas,
            servicio.PlantillaId,
            servicio.PlantillaDigitalId);

    public static UpdateServicio ToUpdate(this ServicioViewModel servicio) =>
        new(
            servicio.Id,
            servicio.CentroCosto.ToInfo(),
            servicio.FranquiciaId,
            servicio.Aka,
            servicio.Nombre,
            servicio.TipoControl.ToEntity(),
            servicio.TipoDistribucion.ToEntity(),
            servicio.TipoServicio,
            servicio.TipoVigencia.ToEntity(),
            servicio.NumeroVigencia,
            servicio.NumeroValidez,
            servicio.NumeroPaxMinimo,
            servicio.NumeroPaxMaximo,
            servicio.EsConHora,
            servicio.EsPorTramos,
            servicio.EsParaVenta,
            servicio.Orden,
            servicio.HolguraInicio,
            servicio.HolguraFin,
            servicio.EsParaMovil,
            servicio.MostrarTramos,
            servicio.EsParaBuses,
            servicio.IdaVuelta,
            servicio.HolguraEntrada,
            servicio.ControlParental,
            servicio.ServicioResponsableId,
            servicio.Politicas,
            servicio.PlantillaId,
            servicio.PlantillaDigitalId);

    private static TipoControl ToEntity(this TipoControlModel tipoControl) =>
        new(tipoControl.Id, tipoControl.Nombre, tipoControl.EsActivo);

    private static TipoDistribucion ToEntity(this TipoDistribucionModel tipoDistribucion) =>
        new(tipoDistribucion.Id, tipoDistribucion.Nombre, tipoDistribucion.EsActivo);

    private static TipoVigencia ToEntity(this TipoVigenciaModel tipoVigencia) =>
        new(tipoVigencia.Id, tipoVigencia.Aka, tipoVigencia.Nombre, tipoVigencia.EsActivo);

    public static AssignCentrosCostoToServicio ToAssignCentrosCosto(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.CentrosCosto.ToInfo());

    public static AssignTramosToServicio ToAssignTramos(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Tramos.ToInfo());

    public static AssignPermisosToServicio ToAssignPermisos(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Permisos.ToInfo());

    public static AssignZonasToServicio ToAssignZonas(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Zonas.ToInfo());

    public static AssignGruposEtariosToServicio ToAssignGruposEtarios(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.GruposEtarios.ToInfo());

    public static AssignCajasToServicio ToAssignCajas(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Cajas.ToInfo());

    private static ImmutableArray<CentroCostoServicioInfo> ToInfo(this IEnumerable<CentroCostoServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static CentroCostoServicioInfo ToInfo(CentroCostoServicioModel centroCostoServicio) =>
        new() { CentroCosto = centroCostoServicio.CentroCosto.ToInfo(), Nombre = centroCostoServicio.Nombre };

    private static ImmutableArray<TramoServicioInfo> ToInfo(this IEnumerable<TramoServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static TramoServicioInfo ToInfo(TramoServicioModel tramoServicio) =>
        new() { CentroCosto = tramoServicio.CentroCosto.ToInfo(), Tramo = tramoServicio.Tramo.ToInfo(), Nombre = tramoServicio.Nombre, CantidadPermisos = tramoServicio.CantidadPersmisos };

    private static TramoInfo ToInfo(this TramoModel tramo) =>
        new() { Id = tramo.Id, Aka = tramo.Aka, Descripcion = tramo.Nombre };

    private static ImmutableArray<PermisoServicioInfo> ToInfo(this IEnumerable<PermisoServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static PermisoServicioInfo ToInfo(PermisoServicioModel permiso) =>
        new() {  Tramo = permiso.Tramo.ToInfo(), CentroCosto = permiso.CentroCosto.ToInfo() };

    private static ImmutableArray<ZonaPorTramoInfo> ToInfo(this IEnumerable<ZonaTramoModel> source) =>
        [.. source.Select(ToInfo)];

    private static ZonaPorTramoInfo ToInfo(ZonaTramoModel zonaTramo) =>
        new() { Tramo = zonaTramo.Tramo.ToInfo(), Zona = zonaTramo.Zona.ToInfo(), EsRetorno = zonaTramo.EsRetorno, EsCombinacion = zonaTramo.EsCombinacion, Orden = zonaTramo.Orden, EsActivo = zonaTramo.EsActivo };

    private static ZonaInfo ToInfo(this ZonaModel zona) =>
        new() { Id = zona.Id, Descripcion = zona.Nombre };

    private static ImmutableArray<GrupoEtarioInfo> ToInfo(this IEnumerable<GrupoEtarioModel> source) =>
        [.. source.Select(ToInfo)];

    private static GrupoEtarioInfo ToInfo(GrupoEtarioModel grupoEtario) =>
        new() { Id = grupoEtario.Id, Aka = grupoEtario.Aka, Descripcion = grupoEtario.Nombre };

    private static ImmutableArray<ServicioPorCajaInfo> ToInfo(this IEnumerable<CajaServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static ServicioPorCajaInfo ToInfo(CajaServicioModel cajaServicio) =>
        new() { Caja = cajaServicio.Caja.ToInfo(), NoUsaZona = cajaServicio.NoUsaZona };

    private static CajaInfo ToInfo(this CajaModel caja) =>
        new() { Id = caja.Id, Aka = caja.Aka, Descripcion = caja.Nombre };
}
