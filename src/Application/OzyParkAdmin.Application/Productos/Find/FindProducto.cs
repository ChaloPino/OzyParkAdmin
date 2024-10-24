using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.Find;

/// <summary>
/// Busca un producto por su id.
/// </summary>
/// <param name="ProductoId">El id del producto a buscar.</param>
public sealed record FindProducto(int ProductoId) : IQuery<ProductoFullInfo>;
