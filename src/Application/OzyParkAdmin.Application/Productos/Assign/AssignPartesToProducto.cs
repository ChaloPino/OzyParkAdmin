using OzyParkAdmin.Domain.Productos;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Productos.Assign;

/// <summary>
/// Asigna las partes de un producto.
/// </summary>
/// <param name="ProductoId">El id del producto al que se le van a asignar las partes.</param>
/// <param name="Partes">Las partes a asignar al producto.</param>
public sealed record AssignPartesToProducto(int ProductoId, ImmutableArray<ProductoParteInfo> Partes) : IProductoStateChangeable;
