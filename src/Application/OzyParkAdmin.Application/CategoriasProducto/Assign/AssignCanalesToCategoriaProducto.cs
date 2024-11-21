using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CategoriasProducto;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.CategoriasProducto.Assign;

/// <summary>
/// Asigna Canales de Venta a Categoria de Producto
/// </summary>
/// <param name="categoriaProductoId">ID de la categoaria de producto</param>
/// <param name="CanalesVenta">Canales de Venta a asignar</param>
public sealed record AssignCanalesToCategoriaProducto(int categoriaProductoId, ImmutableArray<CanalVenta> CanalesVenta) : ICommand<CategoriaProductoFullInfo>;
