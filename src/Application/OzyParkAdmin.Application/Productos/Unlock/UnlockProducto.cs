namespace OzyParkAdmin.Application.Productos.Unlock;

/// <summary>
/// Desbloquea un producto para que pueda vender.
/// </summary>
/// <param name="ProductoId">El id del producto que se quiere desbloquear.</param>
public sealed record UnlockProducto(int ProductoId) : IProductoStateChangeable;
