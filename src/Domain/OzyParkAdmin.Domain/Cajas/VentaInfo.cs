namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// La información de una venta asociada a un turno.
/// </summary>
public class VentaInfo
{
    /// <summary>
    /// El id de la venta.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// El tipo de anulación de la venta.
    /// </summary>
    public string TipoAnulacion { get; set; } = string.Empty;
}