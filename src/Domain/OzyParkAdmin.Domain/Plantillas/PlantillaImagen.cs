namespace OzyParkAdmin.Domain.Plantillas;

/// <summary>
/// Una imagen asociada a la plantilla.
/// </summary>
public sealed class PlantillaImagen
{
    /// <summary>
    /// El id de la imagen.
    /// </summary>
    public int Id { get; private init; } = default!;

    /// <summary>
    /// El aka de la imagen.
    /// </summary>
    public string Aka { get; private init; } = string.Empty;

    /// <summary>
    /// El contenido de la imagen.
    /// </summary>
    public byte[] Contenido { get; private init; } = [];

    /// <summary>
    /// El tipo del contentido.
    /// </summary>
    public string MimeType { get; private init; } = string.Empty;
}
