using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// Contiene métodos de extensión para el <see cref="Cupo"/>.
/// </summary>
public static class CupoExtensions
{
    /// <summary>
    /// Convierte una colección de <see cref="Cupo"/> en una lista de <see cref="CupoFullInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="Cupo"/> a ser convertida.</param>
    /// <returns>La lista de <see cref="CupoFullInfo"/> convertida desde <paramref name="source"/>.</returns>
    public static List<CupoFullInfo> ToFullInfo(this IEnumerable<Cupo> source) =>
        [.. source.Select(ToFullInfo)];

    /// <summary>
    /// Convierte el <paramref name="cupo"/> en <see cref="CupoFullInfo"/>.
    /// </summary>
    /// <param name="cupo">El <see cref="Cupo"/> a convertir.</param>
    /// <returns>El <see cref="CupoFullInfo"/> convertido desde <paramref name="cupo"/>.</returns>
    public static CupoFullInfo ToFullInfo(this Cupo cupo) =>
        new()
        {
            Id = cupo.Id,
            EscenarioCupo = cupo.EscenarioCupo.ToInfo(),
            CanalVenta = cupo.CanalVenta,
            DiaSemana = cupo.DiaSemana,
            HoraInicio = cupo.HoraInicio,
            HoraFin = cupo.HoraFin,
            Total = cupo.Total,
            SobreCupo = cupo.SobreCupo,
            TopeEnCupo = cupo.TopeEnCupo,
            FechaEfectiva = cupo.FechaEfectiva,
            UltimaModificacion = cupo.UltimaModificacion,
        };
}