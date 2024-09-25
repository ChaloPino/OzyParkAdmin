namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene la información del complemento de un producto.
/// </summary>
public sealed record ProductoComplementarioInfo
{
    /// <summary>
    /// El producto complementario.
    /// </summary>
    public ProductoInfo Complemento { get; init; } = default!;

    /// <summary>
    /// El orden de despliegue.
    /// </summary>
    public int Orden { get; init; }
}