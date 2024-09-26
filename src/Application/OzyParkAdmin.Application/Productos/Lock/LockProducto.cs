namespace OzyParkAdmin.Application.Productos.Lock;

/// <summary>
/// Bloqua un producto para que no se pueda vender..
/// </summary>
/// <param name="ProductoId">El id del producto que se quiere bloquear.</param>
public sealed record LockProducto(int ProductoId) : IProductoStateChangeable;
