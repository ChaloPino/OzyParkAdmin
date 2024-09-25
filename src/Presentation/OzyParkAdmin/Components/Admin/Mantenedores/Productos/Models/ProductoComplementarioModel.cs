using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

/// <summary>
/// El modelo de un complemento de producto.
/// </summary>
public sealed record ProductoComplementarioModel
{
    /// <summary>
    /// El propducto complementario.
    /// </summary>
    public required ProductoInfo Complemento { get; set; }

    /// <summary>
    /// El orden de despliegue del complemento.
    /// </summary>
    public required int Orden { get; set; }
}