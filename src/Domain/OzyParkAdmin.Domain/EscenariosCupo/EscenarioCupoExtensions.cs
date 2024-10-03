namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// Contiene métodos de extensión para <see cref="EscenarioCupo"/>.
/// </summary>
public static class EscenarioCupoExtensions
{
    /// <summary>
    /// Convierte el <paramref name="escenarioCupo"/> a <see cref="EscenarioCupoInfo"/>.
    /// </summary>
    /// <param name="escenarioCupo">El <see cref="EscenarioCupo"/> a ser convertido.</param>
    /// <returns>El <see cref="EscenarioCupoInfo"/> convertido desde <paramref name="escenarioCupo"/>.</returns>
    public static EscenarioCupoInfo ToInfo(this EscenarioCupo escenarioCupo) =>
        new() {  Id = escenarioCupo.Id, Nombre = escenarioCupo.Nombre, EsActivo = escenarioCupo.EsActivo };
}
