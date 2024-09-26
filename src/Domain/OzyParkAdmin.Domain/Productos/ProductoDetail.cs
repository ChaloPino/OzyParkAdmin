namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Los detalles del producto que se quieren consultar.
/// </summary>
[Flags]
public enum ProductoDetail
{
    /// <summary>
    /// No se incluye nada.
    /// </summary>
    None = 0,

    /// <summary>
    /// Incluir las cajas asignadas.
    /// </summary>
    Cajas = 1,

    /// <summary>
    /// Incluir las partes del producto.
    /// </summary>
    Partes = 2,

    /// <summary>
    /// Incluir los complementos del producto.
    /// </summary>
    Complementos = 4,

    /// <summary>
    /// Incluir los productos relacionados.
    /// </summary>
    Relacionados = 8,

    /// <summary>
    /// Incluir todos los detalles del producto.
    /// </summary>
    Todo = Cajas | Partes | Complementos | Relacionados,
}
