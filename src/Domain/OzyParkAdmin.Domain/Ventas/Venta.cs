namespace OzyParkAdmin.Domain.Ventas;

/// <summary>
/// La entidad de venta.
/// </summary>
public sealed class Venta
{
    /// <summary>
    /// El id de venta.
    /// </summary>
    public string Id { get; private init; } = string.Empty;

    /// <summary>
    /// La fecha de venta.
    /// </summary>
    public DateOnly Fecha { get; private init; }

    /// <summary>
    /// La fecha y hora de venta.
    /// </summary>
    public DateTime FechaVenta { get; private init; }

    /// <summary>
    /// La caja donde se realizó la venta.
    /// </summary>
    public int CajaId { get; private init; }

}
