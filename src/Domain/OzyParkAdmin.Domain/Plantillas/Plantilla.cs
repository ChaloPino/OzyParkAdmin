namespace OzyParkAdmin.Domain.Plantillas;

/// <summary>
/// La entidad plantilla.
/// </summary>
public sealed class Plantilla
{
    /// <summary>
    /// El id de la plantilla.
    /// </summary>
    public int Id { get; private set; } = default!;

    /// <summary>
    /// El aka de la plantilla
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// La descripción de la plantilla.
    /// </summary>
    public string Descripcion { get; private set; } = string.Empty;

    /// <summary>
    /// Si la plantilla está activa o no.
    /// </summary>
    public bool EsActivo { get; private set; } = default!;
}
