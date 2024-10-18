using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Qoi;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace OzyParkAdmin.Infrastructure.CatalogoImagenes;

/// <summary>
/// Servicio para manipulación de imágenes.
/// </summary>
public sealed class ImagenService : IInfrastructure
{
    private const string JpegMimeType = "image/jpeg";
    private const string WebpMimeType = "image/webp";
    private const string PngMimeType = "image/png";
    private const string BmpMimeType = "image/bmp";
    private const string GifMimeType = "image/gif";
    private const string QoiMimeType = "image/qoi";
    private const string TgaMimeType = "image/tga";
    private const string PbmMimeType = "image/x-portable-bitmap";
    private const string TiffMimeType = "image/tiff";

    private static readonly Dictionary<string, Func<Image, MemoryStream, Task>> SaveQualityFunctions = new(StringComparer.OrdinalIgnoreCase)
    {
        [JpegMimeType] = SaveJpegAsync,
        [WebpMimeType] = SaveWebpAsync,
        [PngMimeType] = SavePngAsync,
        [BmpMimeType] = SaveBmpAsync,
        [GifMimeType] = SaveGifAsync,
        [QoiMimeType] = SaveQoiAsync,
        [TgaMimeType] = SaveTgaAsync,
        [PbmMimeType] = SavePbmAsync,
        [TiffMimeType] = SaveTiffAsync,
    };

    private readonly ImageFormatManager _manager;
    /// <summary>
    /// Crea una nueva instancia de <see cref="ImagenService"/>.
    /// </summary>
    /// <param name="manager"></param>
    public ImagenService(ImageFormatManager manager)
    {
        ArgumentNullException.ThrowIfNull(manager);
        _manager = manager;
    }
    /// <summary>
    /// Redimensiona el tamaño de una imagen.
    /// </summary>
    /// <param name="base64">La imagen en base64.</param>
    /// <param name="mimeType">El tipo de la imagen.</param>
    /// <param name="width">El ancho nuevo de la imagen.</param>
    /// <param name="height">El alto nuevo de la imagen.</param>
    /// <returns>Si el <paramref name="mimeType"/> existe retorna la imagen redimensionada; en caso contrario retorna la misma imagen.</returns>
    public async Task<string> RedimensionarImagen(string base64, string mimeType, int width, int height)
    {
        if (_manager.TryFindFormatByMimeType(mimeType, out var format))
        {
            byte[] buffer = Convert.FromBase64String(base64);

            using var image = Image.Load(buffer);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Lanczos3,
            }));

            await using MemoryStream ms = new();
            Task saveTask = SaveQualityFunctions.TryGetValue(mimeType, out var saveAsync)
                ? saveAsync(image, ms)
                : image.SaveAsync(ms, format);

            await saveTask;

            byte[] resizedImage = ms.ToArray();
            return Convert.ToBase64String(resizedImage);
        }

        return base64;
    }

    private static Task SaveJpegAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new JpegEncoder
        {
            Quality = 90
        });

    private static Task SaveWebpAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new WebpEncoder
        {
            Quality = 90
        });

    private static Task SavePngAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new PngEncoder());

    private static Task SaveBmpAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new BmpEncoder());

    private static Task SaveGifAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new GifEncoder());

    private static Task SaveQoiAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new QoiEncoder());

    private static Task SaveTgaAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new TgaEncoder());

    private static Task SavePbmAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new PbmEncoder());

    private static Task SaveTiffAsync(Image image, MemoryStream ms) =>
        image.SaveAsync(ms, new TiffEncoder());

    /// <summary>
    /// Consigue el tamaño de una imagen que está codificada en base64.
    /// </summary>
    /// <param name="base64">La imagen en base64.</param>
    /// <param name="mimeType">El tipo de la imagen.</param>
    /// <returns>El tamaño (ancho y alto) de la imagen si el <paramref name="mimeType"/> existe; en caso contrario, no retorna el tamaño.</returns>
    public (int Width, int Heigth) ConseguirAnchoAlto(string base64, string mimeType)
    {
        if (_manager.TryFindFormatByMimeType(mimeType, out _))
        {
            byte[] buffer = Convert.FromBase64String(base64);

            using var image = Image.Load(buffer);
            return (image.Size.Width, image.Size.Height);
        }

        return (0, 0);
    }
}
