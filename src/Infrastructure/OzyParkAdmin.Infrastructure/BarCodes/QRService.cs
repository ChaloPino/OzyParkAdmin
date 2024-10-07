using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ZXing.ImageSharp;
using ZXing.QrCode;
using BarcodeFormat = ZXing.BarcodeFormat;

namespace OzyParkAdmin.Infrastructure.BarCodes;
internal class QRService : IBarCodeService<byte[]>
{
    public byte[] GenerateBarCode(string value, BarCodeOptions<byte[]> options)
    {
        var writer = new BarcodeWriter<RgbaVector>()
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions()
            {
                DisableECI = options.DisableECI,
                CharacterSet = options.CharacterSet,
                Width = options.Width,
                Height = options.Height,
            }
        };

        var vector = writer.Write(value);
        using MemoryStream stream = new();
        vector.Save(stream, new PngEncoder());
        return stream.ToArray();
    }
}
