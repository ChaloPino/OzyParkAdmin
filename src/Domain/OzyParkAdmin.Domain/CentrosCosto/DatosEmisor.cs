namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// Datos del emisor.
/// </summary>
public class DatosEmisor
{
    /// <summary>
    /// Rut del emisor.
    /// </summary>
    public string RutEmisor { get; private set; } = string.Empty;

    /// <summary>
    /// Razón social del emisor.
    /// </summary>
    public string RazonSocial { get; private set; } = string.Empty;

    /// <summary>
    /// Giro comercial del emisor.
    /// </summary>
    public string? GiroComercial { get; private set; }

    /// <summary>
    /// Dirección del emisor.
    /// </summary>
    public string Direccion { get; private set; } = string.Empty;

    /// <summary>
    /// Comuna del emisor.
    /// </summary>
    public string? Comuna { get; private set; }

    /// <summary>
    /// Ciudad del emisor.
    /// </summary>
    public string? Ciudad { get; private set; }

    /// <summary>
    /// Código token del emisor.
    /// </summary>
    public string? CodigoToken { get; private set; }

    /// <summary>
    /// Dirección url de SII.
    /// </summary>
    public string? UrlSII { get; private set; }

    /// <summary>
    /// Teléfono del emisor.
    /// </summary>
    public string? Telefono { get; private set; }

    /// <summary>
    /// Usuario para el servicio de generación de boletas electrónicas.
    /// </summary>
    public string? Usuario { get; private set; }

    /// <summary>
    /// Contraseña para el servicio de generación de boletas electrónicas.
    /// </summary>
    public string? Clave { get; private set; }

    /// <summary>
    /// Nombre del proveedor de boletas.
    /// </summary>
    public string ProveedorBoletas { get; private set; } = string.Empty;

    /// <summary>
    /// Plantilla usada para generar las boletas para ciertos servicios de generación de boletas..
    /// </summary>
    public int? PlantillaBoleta { get; private set; }

    /// <summary>
    /// Crea nuevos datos del emisor.
    /// </summary>
    /// <param name="rutEmisor">RUT del emisor.</param>
    /// <param name="razonSocial">Razón social.</param>
    /// <param name="giroComercial">Giro comercial.</param>
    /// <param name="direccion">Dirección.</param>
    /// <param name="comuna">Comuna.</param>
    /// <param name="ciudad">Ciudad.</param>
    /// <param name="codigoToken">Código token.</param>
    /// <param name="urlSII">Url SII.</param>
    /// <param name="telefono">Teléfono.</param>
    /// <param name="usuario">Usuario.</param>
    /// <param name="clave">Clave.</param>
    /// <param name="proveedorBoletas">Proveedor de boletas.</param>
    /// <param name="plantillaBoleta">Plantilla de la boleta.</param>
    /// <returns>Nuevo <see cref="DatosEmisor"/>.</returns>
    public static DatosEmisor Crear(
        string rutEmisor,
        string razonSocial,
        string? giroComercial,
        string direccion,
        string? comuna,
        string? ciudad,
        string codigoToken,
        string? urlSII,
        string telefono,
        string? usuario,
        string? clave,
        string proveedorBoletas,
        int? plantillaBoleta) =>
        new()
        {
            RutEmisor = rutEmisor,
            RazonSocial = razonSocial,
            GiroComercial = giroComercial,
            Direccion = direccion,
            Comuna = comuna,
            Ciudad = ciudad,
            CodigoToken = codigoToken,
            UrlSII = urlSII,
            Telefono = telefono,
            Usuario = usuario,
            Clave = clave,
            ProveedorBoletas = proveedorBoletas,
            PlantillaBoleta = plantillaBoleta
        };
}