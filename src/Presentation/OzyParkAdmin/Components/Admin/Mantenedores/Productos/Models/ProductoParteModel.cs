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
    public required ProductoInfo Parte { get; set; }

    /// <summary>
    /// La cantidad de la parte.
    /// </summary>
    public required decimal Cantidad { get; set; }

    /// <summary>
    /// Si la parte es opcional.
    /// </summary>
    public required bool EsOpcional { get; set; }
}