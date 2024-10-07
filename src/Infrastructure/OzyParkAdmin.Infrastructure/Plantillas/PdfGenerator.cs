using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf;
using iText.StyledXmlParser.Css.Media;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// El generador de pdf.
/// </summary>
public sealed class PdfGenerator : IPdfGenerator
{
    /// <inheritdoc/>
    public byte[] GenerateDocument(string html, string templatePath, string mediaType)
    {
        using MemoryStream ms = new();
        using PdfWriter writer = new(ms);
        using PdfDocument pdf = new(writer);
        ConverterProperties properties = new();
        properties.SetBaseUri(templatePath);
        MediaDeviceDescription mediaDeviceDescription = new(mediaType);
        properties.SetMediaDeviceDescription(mediaDeviceDescription);
        DefaultFontProvider fontProvider = new(true, true, false);
        fontProvider.AddDirectory(Path.Combine(templatePath, "fonts"));
        properties.SetFontProvider(fontProvider);

        HtmlConverter.ConvertToPdf(html, pdf, properties);
        return ms.ToArray();
    }
}