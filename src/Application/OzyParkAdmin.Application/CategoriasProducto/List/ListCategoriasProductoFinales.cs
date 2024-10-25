using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.List;

/// <summary>
/// Lista todas categorías de producto finales que pertenecen a una franquicia.
/// </summary>
/// <param name="FranquiciaId">El id de la franquicia.</param>
public sealed record ListCategoriasProductoFinales(int FranquiciaId) : IQueryListOf<CategoriaProductoInfo>;
