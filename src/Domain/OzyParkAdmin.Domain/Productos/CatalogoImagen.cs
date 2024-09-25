namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// El catálogo de imágen que puede estar asociado a un producto o una categoría de producto.
/// </summary>
public sealed class CatalogoImagen
{
    /// <summary>
    /// El aka del catálogo de imagen.
    /// </summary>
    public string Aka { get; private init; } = string.Empty;

    /// <summary>
    /// El contenido de la imagen codificado en base64.
    /// </summary>
    public string Base64 { get; private set; } = string.Empty;

    /// <summary>
    /// El tipo del contenido de la imagen.
    /// </summary>
    public string MimeType { get; private set; } = string.Empty;

    /// <summary>
    /// El tipo de la imagen.
    /// </summary>
    public string Tipo { get; private set; } = string.Empty;
}