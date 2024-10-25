using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// Lista todos los tipos de producto.
/// </summary>
public sealed record ListTiposProducto : IQueryListOf<TipoProducto>;
