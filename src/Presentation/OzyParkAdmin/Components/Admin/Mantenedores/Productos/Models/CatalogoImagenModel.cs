using Microsoft.AspNetCore.Components.Forms;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

/// <summary>
/// El modelo del catálogo de imagen.
/// </summary>
public sealed record CatalogoImagenModel
{
    /// <summary>
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

    /// <summary>
    /// El archivo cargado.
    /// </summary>
    public IBrowserFile? File { get; set; }


    /// <summary>
    /// Establece el archivo cargado.
    /// </summary>
    /// <param name="file">El <see cref="IBrowserFile"/>.</param>
    /// <param name="tipo">El tipo del catálogo.</param>
    public async Task SetFile(IBrowserFile? file, string tipo)
    {
        if (file is not null)
        {
            await using MemoryStream ms = new();
            await file.OpenReadStream().CopyToAsync(ms);

            byte[] buffer = ms.ToArray();

            Aka = Path.GetFileNameWithoutExtension(file.Name);
            Base64 = Convert.ToBase64String(buffer);
            MimeType = file.ContentType;
            Tipo = tipo;
        }
    }
}