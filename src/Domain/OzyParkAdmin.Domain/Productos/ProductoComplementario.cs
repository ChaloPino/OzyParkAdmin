

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Define un producto complementario.
/// </summary>
public sealed class ProductoComplementario
{
    /// <summary>
    /// El complemento del producto.
    /// </summary>
    public Producto Complemento { get; private set; } = default!;

    /// <summary>
    /// El orden de despliegue del complemento.
    /// </summary>
    public int Orden { get; private set; }

    internal static ProductoComplementario Create(Producto producto, int orden) =>
        new() { Complemento = producto, Orden = orden };

    internal void Update(int orden) =>
        Orden = orden;
}