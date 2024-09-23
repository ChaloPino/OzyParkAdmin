using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Servicios;
internal static class ServicioExtensions
{
    public static ImmutableArray<TramoServicioInfo> ToInfo(this IEnumerable<TramoServicio> source) =>
        [.. source.Select(ToInfo)];

    private static TramoServicioInfo ToInfo(TramoServicio tramoServicio) =>
        new() { Tramo = tramoServicio.Tramo.ToInfo(), CentroCosto = tramoServicio.CentroCosto.ToInfo(), Nombre = tramoServicio.Nombre, CantidadPermisos = tramoServicio.CantidadPermisos };

    public static ImmutableArray<CentroCostoServicioInfo> ToInfo(this IEnumerable<CentroCostoServicio> source) =>
        [.. source.Select(ToInfo)];

    private static CentroCostoServicioInfo ToInfo(CentroCostoServicio centroCostoServicio) =>
        new() { CentroCosto = centroCostoServicio.CentroCosto.ToInfo(), Nombre = centroCostoServicio.Nombre };

    public static ImmutableArray<GrupoEtarioInfo> ToInfo(this IEnumerable<GrupoEtario> source) =>
        [.. source.Select(ToInfo)];

    private static GrupoEtarioInfo ToInfo(GrupoEtario grupoEtario) =>
        new() { Id = grupoEtario.Id, Aka = grupoEtario.Aka, Descripcion = grupoEtario.Descripcion };

    public static ImmutableArray<ServicioPorCajaInfo> ToInfo(this IEnumerable<ServicioPorCaja> source) =>
        [.. source.Select(ToInfo)];

    private static ServicioPorCajaInfo ToInfo(ServicioPorCaja servicioPorCaja) =>
        new() {  Caja = servicioPorCaja.Caja.ToInfo(), NoUsaZona = servicioPorCaja.NoUsaZona };

    public static ImmutableArray<PermisoServicioInfo> ToInfo(this IEnumerable<PermisoServicio> source) =>
        [.. source.Select(ToInfo)];

    private static PermisoServicioInfo ToInfo(PermisoServicio permisoServicio) =>
        new() { Tramo = permisoServicio.Tramo.ToInfo(), CentroCosto = permisoServicio.CentroCosto.ToInfo() };

    public static ImmutableArray<ZonaPorTramoInfo> ToInfo(this IEnumerable<ZonaPorTramo> source) =>
        [.. source.Select(ToInfo)];

    private static ZonaPorTramoInfo ToInfo(ZonaPorTramo zonaPorTramo) =>
        new() { Tramo = zonaPorTramo.Tramo.ToInfo(), Zona = zonaPorTramo.Zona.ToInfo(), EsRetorno = zonaPorTramo.EsRetorno, EsCombinacion = zonaPorTramo.EsCombinacion, Orden = zonaPorTramo.Orden, EsActivo = zonaPorTramo.EsActivo };
}
