namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// Contiene información básica del escenario de cupo.
/// </summary>
public sealed record EscenarioCupoInfo
{
    /// <summary>
    /// El id del escenario de cupo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del escenario de cupo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Si el escenario de cupo está activo.
    /// </summary>
    public bool EsActivo { get; set; }
}
