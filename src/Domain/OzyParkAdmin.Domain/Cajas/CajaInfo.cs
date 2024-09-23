namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Información de la caja.
/// </summary>
public sealed record CajaInfo
{
    /// <summary>
    /// El id de la caja.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka de la caja.
    /// </summary>
    public string Aka { get; init; } = default!;

    /// <summary>
    /// La descripción de la caja.
    /// </summary>
    public string Descripcion { get; init; } = default!;
}
