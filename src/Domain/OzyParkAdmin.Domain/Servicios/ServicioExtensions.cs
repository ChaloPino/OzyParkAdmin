using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Contiene métodos de extensión de <see cref="Servicio"/>.
/// </summary>
public static class ServicioExtensions
{
    /// <summary>
    /// Convierte el <paramref name="servicio"/> a un <see cref="ServicioInfo"/>.
    /// </summary>
    /// <param name="servicio">El <see cref="Servicio"/> a convertir.</param>
    /// <returns>El <see cref="ServicioInfo"/> convertido de <paramref name="servicio"/>.</returns>
    public static ServicioInfo ToInfo(this Servicio servicio) =>
        new() {  Id = servicio.Id, Aka = servicio.Aka, Nombre = servicio.Nombre };

    /// <summary>
    /// Convierte una lista de <see cref="TramoServicio"/> en una lista inmutable de <see cref="TramoServicioInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="TramoServicio"/> a convertir.</param>
    /// <returns>La lista inmutable de <see cref="TramoServicioInfo"/> convertida de <paramref name="source"/>.</returns>
    public static ImmutableArray<TramoServicioInfo> ToInfo(this IEnumerable<TramoServicio> source) =>
        [.. source.Select(ToInfo)];

    private static TramoServicioInfo ToInfo(TramoServicio tramoServicio) =>
        new() { Tramo = tramoServicio.Tramo.ToInfo(), CentroCosto = tramoServicio.CentroCosto.ToInfo(), Nombre = tramoServicio.Nombre, CantidadPermisos = tramoServicio.CantidadPermisos };

    /// <summary>
    /// Convierte una lista de <see cref="CentroCostoServicio"/> en una lista inmutable de <see cref="CentroCostoServicioInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="CentroCostoServicio"/> a convertir.</param>
    /// <returns>La lista inmutable de <see cref="CentroCostoServicioInfo"/> convertida de <paramref name="source"/>,</returns>
    public static ImmutableArray<CentroCostoServicioInfo> ToInfo(this IEnumerable<CentroCostoServicio> source) =>
        [.. source.Select(ToInfo)];

    private static CentroCostoServicioInfo ToInfo(CentroCostoServicio centroCostoServicio) =>
        new() { CentroCosto = centroCostoServicio.CentroCosto.ToInfo(), Nombre = centroCostoServicio.Nombre };

    /// <summary>
    /// Convierte una lista de <see cref="ServicioPorCaja"/> en una lista inmutable de <see cref="ServicioPorCajaInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="ServicioPorCaja"/> a convertir.</param>
    /// <returns>La lista inmutable de <see cref="ServicioPorCajaInfo"/> convertida de <paramref name="source"/>.</returns>
    public static ImmutableArray<ServicioPorCajaInfo> ToInfo(this IEnumerable<ServicioPorCaja> source) =>
        [.. source.Select(ToInfo)];

    private static ServicioPorCajaInfo ToInfo(ServicioPorCaja servicioPorCaja) =>
        new() {  Caja = servicioPorCaja.Caja.ToInfo(), NoUsaZona = servicioPorCaja.NoUsaZona };

    /// <summary>
    /// Convierte una lista de <see cref="PermisoServicio"/> en una lista inmutable de <see cref="PermisoServicioInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="PermisoServicio"/> a convertir.</param>
    /// <returns>La lista inmutable de <see cref="PermisoServicioInfo"/> convertida de <paramref name="source"/>.</returns>
    public static ImmutableArray<PermisoServicioInfo> ToInfo(this IEnumerable<PermisoServicio> source) =>
        [.. source.Select(ToInfo)];

    private static PermisoServicioInfo ToInfo(PermisoServicio permisoServicio) =>
        new() { Tramo = permisoServicio.Tramo.ToInfo(), CentroCosto = permisoServicio.CentroCosto.ToInfo() };

    /// <summary>
    /// Convierte una lista de <see cref="Servicio"/> en una lista de <see cref="ServicioWithDetailInfo"/>.
    /// </summary>
    /// <param name="servicios">La lista de <see cref="Servicio"/> a convertir.</param>
    /// <param name="gruposEtarios">Una lista de <see cref="GrupoEtario"/> que se usan como elementos predeterminados cuando un servicio no tiene asociados grupos etarios.</param>
    /// <returns>La lista de <see cref="ServicioWithDetailInfo"/> convertida de <paramref name="servicios"/>.</returns>
    public static List<ServicioWithDetailInfo> ToDetailInfo(this IEnumerable<Servicio> servicios, IEnumerable<GrupoEtario> gruposEtarios)
        => [.. servicios.Select(x => x.ToDetailInfo(gruposEtarios))];

    /// <summary>
    /// Convierte un <see cref="Servicio"/> en un <see cref="ServicioWithDetailInfo"/>.
    /// </summary>
    /// <param name="servicio">El <see cref="Servicio"/> a convertir.</param>
    /// <param name="gruposEtarios">Una lista de <see cref="GrupoEtario"/> que se usan como elementos predeterminados cuando un servicio no tiene asociados grupos etarios.</param>
    /// <returns>El <see cref="ServicioWithDetailInfo"/> convertido de <paramref name="servicio"/>.</returns>
    public static ServicioWithDetailInfo ToDetailInfo(this Servicio servicio, IEnumerable<GrupoEtario> gruposEtarios) =>
        new()
        {
            Id = servicio.Id,
            Aka = servicio.Aka,
            Nombre = servicio.Nombre,
            Tramos = servicio.Tramos.ToInfo(),
            GruposEtarios = servicio.GruposEtarios.ToInfo(gruposEtarios),
        };
}
