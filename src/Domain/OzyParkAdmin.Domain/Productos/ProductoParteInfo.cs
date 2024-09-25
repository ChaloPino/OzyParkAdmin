namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene la información de una parte del producto.
/// </summary>
public sealed record ProductoParteInfo
{
    /// <summary>
    /// La parte del producto.
    /// </summary>
    public ProductoInfo Parte { get; init; } = default!;

    /// <summary>
    /// La cantidad de la parte.
    /// </summary>
    public decimal Cantidad { get; init; }

    /// <summary>
    /// Si la parte es opcional.
    /// </summary>
    public bool EsOpcional { get; init; }
}