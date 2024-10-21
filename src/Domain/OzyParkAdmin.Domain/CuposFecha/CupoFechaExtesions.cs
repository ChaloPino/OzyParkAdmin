using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Domain.CuposFecha;
/// <summary>
/// Contiene métodos de extensión para el <see cref="CupoFecha"/>.
/// </summary>
public static class CupoFechaExtensions
{
    /// <summary>
    /// Convierte una colección de <see cref="CupoFecha"/> en una lista de <see cref="CupoFechaFullInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="CupoFecha"/> a ser convertida.</param>
    /// <returns>La lista de <see cref="CupoFullInfo"/> convertida desde <paramref name="source"/>.</returns>
    public static List<CupoFechaFullInfo> ToFullInfo(this IEnumerable<CupoFecha> source) =>
        [.. source.Select(ToFullInfo)];

    /// <summary>
    /// Convierte el <paramref name="cupoFecha"/> en <see cref="CupoFechaFullInfo"/>.
    /// </summary>
    /// <param name="cupoFecha">El <see cref="CupoFecha"/> a convertir.</param>
    /// <returns>El <see cref="CupoFechaFullInfo"/> convertido desde <paramref name="cupoFecha"/>.</returns>
    public static CupoFechaFullInfo ToFullInfo(this CupoFecha cupoFecha) =>
        new()
        {
            Id = cupoFecha.Id,
            Fecha = cupoFecha.Fecha,
            EscenarioCupo = cupoFecha.EscenarioCupo.ToInfo(),
            CanalVenta = cupoFecha.CanalVenta,
            DiaSemana = cupoFecha.DiaSemana,
            HoraInicio = cupoFecha.HoraInicio,
            HoraFin = cupoFecha.HoraFin,
            Total = cupoFecha.Total,
            SobreCupo = cupoFecha.SobreCupo,
            TopeEnCupo = cupoFecha.TopeEnCupo,
        };
}
