using MassTransit.Mediator;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.List;

/// <summary>
/// Lista todas las categorías de producto que pertenecen a una franquicia.
/// </summary>
/// <param name="FranquiciaId">El id de la franquicia.</param>
public sealed record ListCategoriasProducto(int FranquiciaId) : Request<ResultListOf<CategoriaProductoInfo>>;
