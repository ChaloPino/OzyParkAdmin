namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// La información de la gaveta.
/// </summary>
public sealed record GavetaInfo
{
    /// <summary>
    /// El id de la gaveta.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El aka de la gaveta.
    /// </summary>
    public string Aka { get; set; } = string.Empty;
}