namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene información de un producto relacionado.
/// </summary>
public sealed record ProductoRelacionadoInfo
{
    /// <summary>
    /// El producto relacionado.
    /// </summary>
    public ProductoInfo Relacionado { get; init; } = default!;

    /// <summary>
    /// El orden de despliegue.
    /// </summary>
    public int Orden { get; init; }
}