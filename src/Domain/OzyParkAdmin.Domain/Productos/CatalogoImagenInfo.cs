namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// La información del catálogo de imagen.
/// </summary>
public sealed record CatalogoImagenInfo
{
    /// <summary>
    /// El aka del catálogo de imagen.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// El contenido de la imagen en base64.
    /// </summary>
    public string Base64 { get; init; } = string.Empty;

    /// <summary>
    /// El mime type de la imagen.
    /// </summary>
    public string MimeType { get; init; } = string.Empty;

    /// <summary>
    /// El tipo de la imagen.
    /// </summary>
    public string Tipo { get; init; } = string.Empty;
}
