using OzyParkAdmin.Domain.Cajas;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Productos.Assign;

/// <summary>
/// Asigna cajas a un producto.
/// </summary>
/// <param name="ProductoId">El id del producto al que se le van a asignar la cajas.</param>
/// <param name="Cajas">Las cajas a asignar al producto.</param>
public sealed record AssignCajasToProducto(int ProductoId, ImmutableArray<CajaInfo> Cajas) : IProductoStateChangeable;
