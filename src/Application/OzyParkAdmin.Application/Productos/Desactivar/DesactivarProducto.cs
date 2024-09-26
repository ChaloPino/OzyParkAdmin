namespace OzyParkAdmin.Application.Productos.Desactivar;

/// <summary>
/// Desactiva un producto.
/// </summary>
/// <param name="ProductoId">El id del producto a desactivar.</param>
public sealed record DesactivarProducto(int ProductoId) : IProductoStateChangeable;
