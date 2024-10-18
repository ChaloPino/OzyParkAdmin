using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// Contiene métodos de extensión para <see cref="FechaExcluidaCupo"/>.
/// </summary>
public static class FechaExcluidaCupoExtensions
{
    /// <summary>
    /// Convierte una lista de <see cref="FechaExcluidaCupo"/> en una lista de <see cref="FechaExcluidaCupoFullInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="FechaExcluidaCupo"/> a convertir.</param>
    /// <returns>La lista de <see cref="FechaExcluidaCupoFullInfo"/> convertida de <paramref name="source"/>.</returns>
    public static List<FechaExcluidaCupoFullInfo> ToFullInfo(this IEnumerable<FechaExcluidaCupo> source) =>
        [..source.Select(ToFullInfo)];

    /// <summary>
    /// Convierte una <see cref="FechaExcluidaCupo"/> en <see cref="FechaExcluidaCupoFullInfo"/>.
    /// </summary>
    /// <param name="fechaExcluida">La <see cref="FechaExcluidaCupo"/> a convertir.</param>
    /// <returns>La <see cref="FechaExcluidaCupoFullInfo"/> convertida de <paramref name="fechaExcluida"/>.</returns>
    public static FechaExcluidaCupoFullInfo ToFullInfo(this FechaExcluidaCupo fechaExcluida) =>
        new() { CentroCosto = fechaExcluida.CentroCosto.ToInfo(), CanalVenta = fechaExcluida.CanalVenta, Fecha = fechaExcluida.Fecha };
}
