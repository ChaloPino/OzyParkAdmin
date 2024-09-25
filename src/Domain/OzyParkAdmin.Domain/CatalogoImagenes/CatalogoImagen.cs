
namespace OzyParkAdmin.Domain.CatalogoImagenes;

/// <summary>
/// El catálogo de imágen que puede estar asociado a un producto o una categoría de producto.
/// </summary>
public sealed class CatalogoImagen
{
    /// <summary>
    /// El aka del catálogo de imagen.
    /// </summary>
    public string Aka { get; private init; }

    /// <summary>
    /// El contenido de la imagen codificado en base64.
    /// </summary>
    public string Base64 { get; private set; }

    /// <summary>
    /// El tipo del contenido de la imagen.
    /// </summary>
    public string MimeType { get; private set; }

    /// <summary>
    /// El tipo de la imagen.
    /// </summary>
    public string Tipo { get; private set; }

    internal static CatalogoImagen Create(string aka, string base64, string mimeType, string tipo) =>
        new()
        {
            Aka = aka,
            Base64 = base64,
            MimeType = mimeType,
            Tipo = tipo
        };

    internal void Update(string base64, string mimeType, string tipo)
    {
        Base64 = base64;
        MimeType = mimeType;
        Tipo = tipo;
    }
}