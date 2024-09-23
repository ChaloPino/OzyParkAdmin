namespace OzyParkAdmin.Domain.Zonas;

/// <summary>
/// La entidad zona.
/// </summary>
public sealed class Zona
{
    /// <summary>
    /// El id de la zona.
    /// </summary>
    public int Id { get; private set; }


    /// <summary>
    /// El aka de la zona.
    /// </summary>
    public string Aka { get; private set; } = default!;

    /// <summary>
    /// La descripción de la zona.
    /// </summary>
    public string Descripcion { get; private set; } = default!;

    /// <summary>
    /// El umbral de reingreso.
    /// </summary>
    public int UmbralReingreso { get; private set; }
}