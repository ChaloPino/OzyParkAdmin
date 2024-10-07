namespace OzyParkAdmin.Infrastructure.BarCodes;

/// <summary>
/// Opciones de configuración para generar el código de barras.
/// </summary>
/// <typeparam name="T">El tipo de elemento para generar el código de barras.</typeparam>
public class BarCodeOptions<T>
{
    /// <summary>
    /// El ancho.
    /// </summary>
    public required int Width { get; set; }

    /// <summary>
    /// El alto.
    /// </summary>
    public required int Height { get; set; }

    /// <summary>
    /// Si se deshabilitado el ECI.
    /// </summary>
    public bool DisableECI { get; set; } = true;

    /// <summary>
    /// El conjunto de caracteres.
    /// </summary>
    public string CharacterSet { get; set; } = "UTF-8";

    /// <summary>
    /// El convertidor a base64.
    /// </summary>
    public Func<T, string> Converter { get; set; } = default!;
}
