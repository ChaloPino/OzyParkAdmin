namespace OzyParkAdmin.Domain.Plantillas;

/// <summary>
/// La entidad plantilla.
/// </summary>
public sealed class Plantilla
{
    private readonly List<PlantillaImagen> _imagenes = [];

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
    /// El contenido de la plantilla.
    /// </summary>
    public string Contenido { get; private set; } = string.Empty;

    /// <summary>
    /// La hoja de estilo del contenido.
    /// </summary>
    public string? Estilo { get; private set; } = default;

    /// <summary>
    /// Si la plantilla usa contenido legado.
    /// </summary>
    public bool EsLegado { get; private set; } = default!;

    /// <summary>
    /// Si la plantilla está activa o no.
    /// </summary>
    public bool EsActivo { get; private set; } = default!;

    /// <summary>
    /// La impresora asociada a la plantilla
    /// </summary>
    public Impresora Impresora { get; private set; } = default!;

    /// <summary>
    /// Las imagenes asociadas a la plantilla.
    /// </summary>
    public IEnumerable<PlantillaImagen> Imagenes => _imagenes;
}
