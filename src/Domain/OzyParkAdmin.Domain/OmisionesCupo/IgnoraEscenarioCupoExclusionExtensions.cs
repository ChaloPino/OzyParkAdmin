using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Domain.OmisionesCupo;

/// <summary>
/// Contiene métodos de extensión para transformar un <see cref="IgnoraEscenarioCupoExclusion"/>.
/// </summary>
public static class IgnoraEscenarioCupoExclusionExtensions
{
    /// <summary>
    /// Convierte una lista de <see cref="IgnoraEscenarioCupoExclusion"/> a una lista de <see cref="IgnoraEscenarioCupoExclusionFullInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="IgnoraEscenarioCupoExclusion"/> a ser convertida.</param>
    /// <returns>La lista de <see cref="IgnoraEscenarioCupoExclusionFullInfo"/> convertida desde <paramref name="source"/>.</returns>
    public static IEnumerable<IgnoraEscenarioCupoExclusionFullInfo> ToFullInfo(this IEnumerable<IgnoraEscenarioCupoExclusion> source) =>
        [.. source.Select(ToFullInfo)];

    private static IgnoraEscenarioCupoExclusionFullInfo ToFullInfo(this IgnoraEscenarioCupoExclusion omision) =>
        new()
        {
            EscenarioCupo = omision.EscenarioCupo.ToInfo(),
            CanalVenta = omision.CanalVenta,
            FechaIgnorada = omision.FechaIgnorada,
        };
}
