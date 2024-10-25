using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// Lista todos los productos que pueden ser usados como partes de otro producto.
/// </summary>
/// <param name="FranquiciaId">El id de la franquicia al que pertencen los productos.</param>
/// <param name="ExceptoProductoId">El id del producto que no tiene que aparecer en la lista.</param>
public sealed record ListProductosParaPartes(int FranquiciaId, int ExceptoProductoId) : IQueryListOf<ProductoInfo>;
