using Microsoft.AspNetCore.Components.Forms;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;

/// <summary>
/// El modelo del catálogo de imagen.
/// </summary>
public sealed record CatalogoImagenModel
{
    private int? _ancho;
    private int? _alto;

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
    /// Define si es que la imagen tiene definido un tamaño.
    /// </summary>
    public bool HasSize => _ancho is not null;

    /// <summary>
    /// Verifica si se puede editar el aka.
    /// </summary>
    public bool CanEditAka => File is not null;


    /// <summary>
    /// Establece el archivo cargado.
    /// </summary>
    /// <param name="file">El <see cref="IBrowserFile"/>.</param>
    /// <param name="tipo">El tipo del catálogo.</param>
    public async Task SetFile(IBrowserFile? file, string tipo)
    {
        File = file;

        if (file is not null)
        {
            await using MemoryStream ms = new();
            await file.OpenReadStream().CopyToAsync(ms);

            byte[] buffer = ms.ToArray();

            Aka = Path.GetFileNameWithoutExtension(file.Name).ToUpperInvariant();
            Base64 = Convert.ToBase64String(buffer);
            MimeType = file.ContentType;
            Tipo = tipo;
            _ancho = null;
            _alto = null;
        }
    }

    internal (int Ancho, int Alto) Size => (_ancho!.Value, _alto!.Value);

    /// <summary>
    /// Establece el ancho y el alto de la imagen.
    /// </summary>
    /// <param name="ancho">El ancho de la imagen.</param>
    /// <param name="alto">El alto de la imagen.</param>
    public void SetSize(int ancho, int alto)
    {
        _ancho = ancho;
        _alto = alto;
    }
}
