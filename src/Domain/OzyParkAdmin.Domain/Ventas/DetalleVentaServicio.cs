namespace OzyParkAdmin.Domain.Ventas;

/// <summary>
/// El detalle de venta para un servicio.
/// </summary>
public sealed class DetalleVentaServicio : DetalleVenta
{
    /// <summary>
    /// El id del servicio.
    /// </summary>
    public int ServicioId { get; private init; }

    /// <summary>
    /// El id del tramo.
    /// </summary>
    public int TramoId { get; private init; }

    /// <summary>
    /// El inicio de vigencia.
    /// </summary>
    public DateTime InicioVigencia { get; private init; }

    /// <summary>
    /// El fin de vigencia.
    /// </summary>
    public DateTime FinVigencia { get; private init; }
}
