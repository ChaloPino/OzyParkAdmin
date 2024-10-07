namespace OzyParkAdmin.Domain.Ventas;

/// <summary>
/// El detalle de una venta.
/// </summary>
public abstract class DetalleVenta
{
    /// <summary>
    /// La línea de la venta.
    /// </summary>
    public int Linea { get; private set; }

    /// <summary>
    /// El centro de costo asociado a la venta.
    /// </summary>
    public int CentroCostoId { get; private set; }

    /// <summary>
    /// La cantidad.
    /// </summary>
    public int Cantidad { get; private set; }

    /// <summary>
    /// El total acumulado.
    /// </summary>
    public decimal TotalAcumulado { get; private set; }
}