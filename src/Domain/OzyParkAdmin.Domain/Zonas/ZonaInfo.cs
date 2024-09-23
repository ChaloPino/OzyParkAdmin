namespace OzyParkAdmin.Domain.Zonas;

/// <summary>
/// Información de una zona.
/// </summary>
public sealed record ZonaInfo
{
    /// <summary>
    /// El id de la zona.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// La descripción de una zona.
    /// </summary>
    public string Descripcion { get; init; } = default!;
}
