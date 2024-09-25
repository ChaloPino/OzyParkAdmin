using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

/// <summary>
/// El modelo de un producto relacionado.
/// </summary>
public sealed record ProductoRelacionadoModel
{
    /// <summary>
    /// El producto relacionado.
    /// </summary>
    public required ProductoInfo Relacionado { get; set; }

    /// <summary>
    /// El orden de despliegue.
    /// </summary>
    public required int Orden { get; set; }
}