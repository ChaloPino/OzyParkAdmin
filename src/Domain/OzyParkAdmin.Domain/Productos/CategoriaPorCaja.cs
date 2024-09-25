using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// La caja asignada a una categoría de producto.
/// </summary>
public sealed class CategoriaPorCaja
{
    /// <summary>
    /// La caja asignada a la categoría de producto.
    /// </summary>
    public Caja Caja { get; private set; } = default!;

    /// <summary>
    /// El orden que se despliega la categóría de producto en la caja.
    /// </summary>
    public int Orden { get; private set; }
}