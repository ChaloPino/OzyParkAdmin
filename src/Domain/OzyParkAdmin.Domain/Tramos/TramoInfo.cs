namespace OzyParkAdmin.Domain.Tramos;

/// <summary>
/// Información del tramo.
/// </summary>
public sealed record TramoInfo
{
    /// <summary>
    /// El id del tramo.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka del tramo.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// La descripción del tramo.
    /// </summary>
    public string Descripcion { get; init; } = string.Empty;
}
