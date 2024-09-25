namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Define un producto relacionado al producto.
/// </summary>
public class ProductoRelacionado
{
    /// <summary>
    /// El producto relacionado al producto.
    /// </summary>
    public Producto Relacionado { get; private set; } = default!;

    /// <summary>
    /// El orden de despliegue del producto relacionado.
    /// </summary>
    public int Orden { get; private set; }
}