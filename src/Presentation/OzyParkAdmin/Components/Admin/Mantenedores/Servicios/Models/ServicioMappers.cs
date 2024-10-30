using Microsoft.CodeAnalysis.CSharp.Syntax;
using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Servicios.Assigns;
using OzyParkAdmin.Application.Servicios.Create;
using OzyParkAdmin.Application.Servicios.Search;
using OzyParkAdmin.Application.Servicios.Update;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Productos;
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
        SortExpressionCollection<Servicio> sortExpressions = state.SortDefinitions.Count == 0
             ? SortExpressionCollection<Servicio>.CreateDefault(x => x.Nombre, false)
             : new SortExpressionCollection<Servicio>();

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
            CentroCosto = servicio.CentroCosto,
            TipoControl = servicio.TipoControl,
            TipoDistribucion = servicio.TipoDistribucion,
            TipoServicio = servicio.TipoServicio,
            TipoVigencia = servicio.TipoVigencia,
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
            GruposEtarios = [..servicio.GruposEtarios],
            Cajas = servicio.Cajas.ToModel(),
            Permisos = servicio.Permisos.ToModel(),
            PlantillaId = servicio.PlantillaId,
            PlantillaDigitalId = servicio.PlantillaDigitalId,
            EsActivo = servicio.EsActivo,
        };

    public static List<TramoServicioModel> ToModel(this IEnumerable<TramoServicioInfo> source) =>
        source.Select(ToModel).ToList();

    private static TramoServicioModel ToModel(TramoServicioInfo tramoServicio) =>
        new() { CentroCosto = tramoServicio.CentroCosto, Tramo = tramoServicio.Tramo, Nombre = tramoServicio.Nombre, CantidadPersmisos = tramoServicio.CantidadPermisos };

    public static List<CentroCostoServicioModel> ToModel(this IEnumerable<CentroCostoServicioInfo> source) =>
        source.Select(ToModel).ToList();

    public static CentroCostoServicioModel ToModel(this CentroCostoServicioInfo centroCostoServicio) =>
        new() { CentroCosto = centroCostoServicio.CentroCosto, Nombre = centroCostoServicio.Nombre };

    public static List<CajaServicioModel> ToModel(this IEnumerable<ServicioPorCajaInfo> source) =>
        source.Select(ToModel).ToList();

    private static CajaServicioModel ToModel(ServicioPorCajaInfo servicioPorCaja) =>
        new() { Caja = servicioPorCaja.Caja, NoUsaZona = servicioPorCaja.NoUsaZona };

    public static List<PermisoServicioModel> ToModel(this IEnumerable<PermisoServicioInfo> source) =>
        source.Select(ToModel).ToList();

    private static PermisoServicioModel ToModel(PermisoServicioInfo permisoServicio) =>
        new() { Tramo = permisoServicio.Tramo, CentroCosto = permisoServicio.CentroCosto };

    public static CreateServicio ToCreate(this ServicioViewModel servicio) =>
        new(
            servicio.CentroCosto,
            servicio.FranquiciaId,
            servicio.Aka,
            servicio.Nombre,
            servicio.TipoControl,
            servicio.TipoDistribucion,
            servicio.TipoServicio,
            servicio.TipoVigencia,
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
            servicio.CentroCosto,
            servicio.FranquiciaId,
            servicio.Aka,
            servicio.Nombre,
            servicio.TipoControl,
            servicio.TipoDistribucion,
            servicio.TipoServicio,
            servicio.TipoVigencia,
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

    public static AssignCentrosCostoToServicio ToAssignCentrosCosto(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.CentrosCosto.ToInfo());

    public static AssignTramosToServicio ToAssignTramos(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Tramos.ToInfo());

    public static AssignPermisosToServicio ToAssignPermisos(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Permisos.ToInfo());

    public static AssignGruposEtariosToServicio ToAssignGruposEtarios(this ServicioViewModel servicio) =>
        new(servicio.Id, [..servicio.GruposEtarios]);

    public static AssignCajasToServicio ToAssignCajas(this ServicioViewModel servicio) =>
        new(servicio.Id, servicio.Cajas.ToInfo());

    private static ImmutableArray<CentroCostoServicioInfo> ToInfo(this IEnumerable<CentroCostoServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static CentroCostoServicioInfo ToInfo(CentroCostoServicioModel centroCostoServicio) =>
        new() { CentroCosto = centroCostoServicio.CentroCosto, Nombre = centroCostoServicio.Nombre };

    private static ImmutableArray<TramoServicioInfo> ToInfo(this IEnumerable<TramoServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static TramoServicioInfo ToInfo(TramoServicioModel tramoServicio) =>
        new() { CentroCosto = tramoServicio.CentroCosto, Tramo = tramoServicio.Tramo, Nombre = tramoServicio.Nombre, CantidadPermisos = tramoServicio.CantidadPersmisos };

    private static ImmutableArray<PermisoServicioInfo> ToInfo(this IEnumerable<PermisoServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static PermisoServicioInfo ToInfo(PermisoServicioModel permiso) =>
        new() {  Tramo = permiso.Tramo, CentroCosto = permiso.CentroCosto };

    private static ImmutableArray<ServicioPorCajaInfo> ToInfo(this IEnumerable<CajaServicioModel> source) =>
        [.. source.Select(ToInfo)];

    private static ServicioPorCajaInfo ToInfo(CajaServicioModel cajaServicio) =>
        new() { Caja = cajaServicio.Caja, NoUsaZona = cajaServicio.NoUsaZona };

    public static string ToLabel(this TipoVigencia tipoVigencia) =>
        string.Equals(tipoVigencia.Aka, "dd", StringComparison.OrdinalIgnoreCase) ? "Días" : "Hora";

    public static string ToVigencia(this TipoVigencia tipoVigencia) =>
        string.Equals(tipoVigencia.Aka, "dd", StringComparison.OrdinalIgnoreCase) ? "día" : "hora";
}
