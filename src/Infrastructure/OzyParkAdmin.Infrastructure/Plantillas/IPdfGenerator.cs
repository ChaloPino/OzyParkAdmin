namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Generador de documentos pdf.
/// </summary>
public interface IPdfGenerator
{
    /// <summary>
    /// Genera el pdf desde un html.
    /// </summary>
    /// <param name="html">El html.</param>
    /// <param name="templatePath">La ruta del contenido de la plantilla.</param>
    /// <param name="mediaType">El tipo de contenido.</param>
    /// <returns>El binario del pdf.</returns>
    byte[] GenerateDocument(string html, string templatePath, string mediaType);
}
