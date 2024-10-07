using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;
using iText.Layout;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Genera el pdf para una zebra.
/// </summary>
public class ZebraPdfGenerator : IPdfGenerator
{
    private static readonly ConcurrentDictionary<string, XDocument> TemplatesCached = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public byte[] GenerateDocument(string html, string templatePath, string mediaType)
    {
        XDocument documentTemplate = GetDocumentTemplate(html);
        PdfParser parser = new(documentTemplate, templatePath, mediaType);
        return parser.Parse();
    }

    private static XDocument GetDocumentTemplate(string template)
    {
        return TemplatesCached.GetOrAdd(template, XDocument.Parse);
    }

    private sealed class PdfParser
    {
        private static readonly Dictionary<string, (float Width, float Height)> Sizes = new(StringComparer.OrdinalIgnoreCase)
        {
            { "zebra", (96f, 960f) }
        };

        private readonly XDocument _content;
        private readonly string _templatePath;
        private float _maxWidth;
        private (float Width, float Height) _currentSize;

        public PdfParser(XDocument content, string templatePath, string mediaType)
        {
            _content = content;
            _templatePath = templatePath;
            Sizes.TryGetValue(mediaType, out _currentSize);
        }

        public byte[] Parse()
        {
            XElement root = _content.Elements().First();

            using MemoryStream stream = new();
            PdfWriter writer = new(stream);
            PdfDocument pdf = new(writer);
            FontSet fontSet = new();
            AddFonts(pdf, fontSet, _templatePath);
            PageSize size = pdf.GetDefaultPageSize();
            _maxWidth = size.GetWidth();
            if (_currentSize.Width > 0 && _currentSize.Height > 0)
            {
                PageSize pageSize = new(_currentSize.Width, _currentSize.Height);
                pdf.SetDefaultPageSize(pageSize);
                _maxWidth = pageSize.GetWidth();
            }
            else if (root.Attribute("width") is not null)
            {
                string value = root.Attribute("width")!.Value;
                PageSize pageSize = new(float.Parse(value), size.GetHeight());
                pdf.SetDefaultPageSize(pageSize);
                _maxWidth = pageSize.GetWidth();
            }

            string defaultFontFamily = root.Attribute("font-family")?.Value ?? pdf.GetDefaultFont().GetFontProgram().GetRegistry();

            Margins margins = new();

            if (root.Attribute("margins") is not null)
            {
                string[] margs = root.Attribute("margins")!.Value.Split(',');
                margins.Left = float.Parse(margs[0]);
                margins.Top = float.Parse(margs[1]);
                margins.Right = float.Parse(margs[2]);
                margins.Bottom = float.Parse(margs[3]);
            }

            PdfPage page = pdf.AddNewPage();

            foreach (XElement element in root.Elements())
            {
                Parse(page, fontSet, defaultFontFamily, margins, element);
            }

            pdf.Close();

            return stream.ToArray();
        }

        private void Parse(PdfPage page, FontSet fontSet, string defaultFontFamily, Margins margins, XElement element)
        {
            if (element.Name == "Box")
            {
                float width = element.Attribute("width") is not null ? float.Parse(element.Attribute("width")!.Value) : _maxWidth;
                float height = element.Attribute("height") is not null ? float.Parse(element.Attribute("height")!.Value) : 0f;
                float top = element.Attribute("top") is not null ? float.Parse(element.Attribute("top")!.Value) : 0f;
                float left = element.Attribute("left") is not null ? float.Parse(element.Attribute("left")!.Value) : 0f;
                float x = left - margins.Left;
                float y = page.GetPageSize().GetHeight() - height - top - margins.Top;

                PdfCanvas pdfCanvas = new(page);
                Rectangle rectangle = new(x, y, width, height);
                pdfCanvas.Rectangle(rectangle);
                Canvas canvas = new(page, rectangle);
                canvas.SetFontProvider(new FontProvider(fontSet, defaultFontFamily));
                ParseBox(canvas, element);
                pdfCanvas.SaveState();
                canvas.Close();
            }
            else if (element.Name == "VerticalBlock")
            {
                ParseVerticalBlock(page, fontSet, defaultFontFamily, margins, element);
            }
        }

        private static void ParseBox(Canvas canvas, XElement element, bool rotate = false)
        {
            foreach (XElement child in element.Elements())
            {
                if (child.Name == "TextValue")
                {
                    canvas.Add(CreateParagraph(child, rotate));
                }
                else if (child.Name == "Image")
                {
                    Image? image = CreateImage(child);

                    if (image is not null)
                    {
                        canvas.Add(image);
                    }
                }
            }
        }

        private static void ParseVerticalBlock(PdfPage page, FontSet fontSet, string defaultFontFamily, Margins margins, XElement element)
        {
            float vheight = element.Attribute("height") is not null ? float.Parse(element.Attribute("height")!.Value) : 0f;
            float vtop = element.Attribute("top") is not null ? float.Parse(element.Attribute("top")!.Value) : 0f;
            float vleft = element.Attribute("left") is not null ? float.Parse(element.Attribute("left")!.Value) : 0f;

            foreach (XElement child in element.Elements())
            {
                if (child.Name == "Box")
                {
                    float height = child.Attribute("height") is not null ? float.Parse(child.Attribute("height")!.Value) : 0;
                    float top = child.Attribute("top") is not null ? float.Parse(child.Attribute("top")!.Value) : 0f;
                    float left = child.Attribute("left") is not null ? float.Parse(child.Attribute("left")!.Value) : 0f;
                    float x = left + vleft - margins.Left;
                    float y = page.GetPageSize().GetHeight() - vheight - height - top - vtop - margins.Top;

                    PdfCanvas pdfCanvas = new(page);
                    Rectangle rectangle = new(x, y, 16, vheight);
                    pdfCanvas.Rectangle(rectangle);
                    Canvas canvas = new(page, rectangle);
                    canvas.SetFontProvider(new FontProvider(fontSet, defaultFontFamily));

                    ParseBox(canvas, child, true);
                    canvas.Close();
                }
            }
        }

        private static Paragraph CreateParagraph(XElement element, bool rotate = false)
        {
            Paragraph paragraph = new(element.Value);
            paragraph.SetFontColor(ColorConstants.BLACK);

            if (element.Attribute("font-size") is not null)
            {
                paragraph.SetFontSize(float.Parse(element.Attribute("font-size")!.Value));
            }

            if (element.Attribute("font-family") is not null)
            {
                paragraph.SetFontFamily(element.Attribute("font-family")!.Value);
            }

            if (element.Attribute("font-style") is not null)
            {
                string style = element.Attribute("font-style")!.Value;

                if (style == "bold")
                {
                    paragraph.SetBold();
                }
                else if (style == "italic")
                {
                    paragraph.SetItalic();
                }
            }

            if (element.Attribute("alignment") != null)
            {
                string alignment = element.Attribute("alignment")!.Value;

                if (alignment == "center")
                {
                    paragraph.SetTextAlignment(TextAlignment.CENTER);
                }
                else if (alignment == "near")
                {
                    paragraph.SetTextAlignment(TextAlignment.LEFT);
                }
                else if (alignment == "far")
                {
                    paragraph.SetTextAlignment(TextAlignment.RIGHT);
                }
            }

            if (rotate)
            {
                paragraph.SetRotationAngle(1.570796);
            }

            return paragraph;
        }

        private static Image? CreateImage(XElement element)
        {
            bool isBase64 = element.Attribute("isBase64") is not null && bool.Parse(element.Attribute("isBase64")!.Value);

            if (isBase64)
            {
                Image image = new(ImageDataFactory.CreatePng(Convert.FromBase64String(element.Attribute("source")!.Value)));

                if (element.Attribute("size") is not null)
                {
                    float size = float.Parse(element.Attribute("size")!.Value);
                    image.SetWidth(size);
                    image.SetHeight(size);
                }

                if (element.Attribute("alignment") is not null)
                {
                    string alignment = element.Attribute("alignment")!.Value;

                    if (alignment == "center")
                    {
                        image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    }
                    else if (alignment == "near")
                    {
                        image.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                    }
                    else if (alignment == "far")
                    {
                        image.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                    }
                }

                return image;
            }

            return null;
        }

        private static void AddFonts(PdfDocument document, FontSet fontSet, string templatePath)
        {
            fontSet.AddDirectory(System.IO.Path.Combine(templatePath, "fonts"));

            foreach (string file in Directory.GetFiles(System.IO.Path.Combine(templatePath, "fonts"), "*.ttf"))
            {
                PdfFont pdfFont = PdfFontFactory.CreateFont(file);
                document.AddFont(pdfFont);
            }
        }

        private sealed class Margins
        {
            public float Left { get; set; }
            public float Right { get; set; }
            public float Top { get; set; }
            public float Bottom { get; set; }
        }
    }
}
