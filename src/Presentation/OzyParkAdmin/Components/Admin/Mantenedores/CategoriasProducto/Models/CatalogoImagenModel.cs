namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;

/// <summary>
/// El modelo del catálogo de imagen.
/// </summary>
public class CatalogoImagenModel
{    /// <summary>
     /// El aka del catálogo de imagen.
     /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El contenido de la imagen en base64.
    /// </summary>
    public string Base64 { get; set; } = string.Empty;

    /// <summary>
    /// El tipo de media del contenido.
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// El tipo de la imagen.
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    internal string Data => $"data:{MimeType};base64,{Base64}";
}
