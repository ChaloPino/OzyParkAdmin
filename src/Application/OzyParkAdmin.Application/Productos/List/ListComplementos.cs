using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// Lista todos los productos de tipo complemento pertenecientes a una categoría de producto.
/// </summary>
/// <param name="CategoriaId">El id de la categoría de producto.</param>
public sealed record ListComplementos(int CategoriaId) : Request<ResultListOf<ProductoInfo>>;
