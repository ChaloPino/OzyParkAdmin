using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

/// <summary>
/// El modelo de la parte de un producto.
/// </summary>
public sealed record ProductoParteModel
{
    /// <summary>
    /// La parte del producto.
    /// </summary>
    public ProductoInfo Parte { get; set; } = default!;

    /// <summary>
    /// La cantidad de la parte.
    /// </summary>
    public decimal Cantidad { get; set; } = 1;

    /// <summary>
    /// Si la parte es opcional.
    /// </summary>
    public bool EsOpcional { get; set; }
}