using iText.IO.Font;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Fonts;

internal static class CalibriFonts
{
    private static FontProgram? _calibri;
    private static FontProgram? _calibrib;
    private static FontProgram? _calibrii;
    private static FontProgram? _calibril;
    private static FontProgram? _calibrili;
    private static FontProgram? _calibriz;

    internal static FontProgram Calibri => GetFont("calibri", ref _calibri);
    internal static FontProgram CalibriBold => GetFont("calibrib", ref _calibrib);
    internal static FontProgram CalibriItalic => GetFont("calibrii", ref _calibrii);
    internal static FontProgram CalibriLight => GetFont("calibril", ref _calibril);
    internal static FontProgram CalibriLigthItalic => GetFont("calibrili", ref _calibrili);
    internal static FontProgram CalibriBoldItalic => GetFont("calibriz", ref _calibriz);

    private static FontProgram GetFont(string resourceName, ref FontProgram? font)
    {
        if (font is null)
        {
            byte[] buffer = ReadResource(resourceName);
            font = FontProgramFactory.CreateFont(buffer);
        }
        return font;
    }

    private static byte[] ReadResource(string resourceName)
    {
        Stream stream = typeof(CalibriFonts).Assembly.GetManifestResourceStream($"OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Fonts.{resourceName}.ttf")!;
        stream.Position = 0;
        byte[] buffer = new byte[stream.Length];

        int readed;

        for (int totalBytesCopied = 0; totalBytesCopied < stream.Length; totalBytesCopied += readed)
        {
            readed = stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
        }

        return buffer;
    }
}
