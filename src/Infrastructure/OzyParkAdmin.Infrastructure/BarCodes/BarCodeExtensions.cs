namespace OzyParkAdmin.Infrastructure.BarCodes;

/// <summary>
/// Contiene métodos de extensón para generar código QR.
/// </summary>
public static class BarCodeExtensions
{
    /// <summary>
    /// Genera un código de barra y lo convierte en base64.
    /// </summary>
    /// <param name="value">El valor para generar código de barras.</param>
    /// <param name="width">El ancho de la imagen.</param>
    /// <param name="height">El alto de la imagen.</param>
    /// <param name="mimeType">El tipo de contenido.</param>
    /// <returns>El código barras en base64.</returns>
    public static string ToBarCodeBase64Image(this string value, int width, int height, string mimeType)
    {
        return value.ToBarCode(BarCodeOptionsBase64Image.Create(width, height, mimeType));
    }

    /// <summary>
    /// Genera un código de barra y lo convierte en base64.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que convierte a código de barras.</typeparam>
    /// <param name="value">El contenido del código de barras.</param>
    /// <param name="options">Las opciones para la generación del código debarras.</param>
    /// <returns>El código de barras en base64.</returns>
    public static string ToBarCode<T>(this string value, BarCodeOptions<T> options)
    {
        IBarCodeService<T> barCodeService = BarCodeProvider.GetBarCode<T>();
        T code = barCodeService.GenerateBarCode(value, options);
        return options.Converter(code);
    }
}
