namespace OzyParkAdmin.Application.Productos.Activar;

/// <summary>
/// Activa un producto.
/// </summary>
/// <param name="ProductoId">El id del producto a activar.</param>
public sealed record ActivarProducto(int ProductoId) : IProductoStateChangeable;
