namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene información del producto.
/// </summary>
public sealed record ProductoInfo
{
    /// <summary>
    /// El id del producto.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka del producto.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// El sky del producto.
    /// </summary>
    public string Sku { get; init; } = string.Empty;

    /// <summary>
    /// El nombre del producto.
    /// </summary>
    public string Nombre { get; init; } = string.Empty;

    /// <summary>
    /// Si el producto está activo.
    /// </summary>
    public bool EsActivo { get; init; }
}