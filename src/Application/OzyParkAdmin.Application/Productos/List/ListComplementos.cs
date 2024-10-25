using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// Lista todos los productos de tipo complemento pertenecientes a una categoría de producto.
/// </summary>
/// <param name="CategoriaId">El id de la categoría de producto.</param>
/// <param name="ExceptoProductoId">El id del producto que no tiene que salir en la lista. Si se quiere que vayan todos, entonces el valor debe ser <c>0</c>.</param>
public sealed record ListComplementos(int CategoriaId, int ExceptoProductoId) : IQueryListOf<ProductoInfo>;
