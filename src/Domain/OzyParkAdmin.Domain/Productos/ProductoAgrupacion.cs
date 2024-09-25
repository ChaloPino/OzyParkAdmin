namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Define la agrupación contable asociada a un producto.
/// </summary>
public sealed class ProductoAgrupacion
{
    /// <summary>
    /// La agrupación contable asociada al producto.
    /// </summary>
    public AgrupacionContable AgrupacionContable { get; private set; } = default!;
}