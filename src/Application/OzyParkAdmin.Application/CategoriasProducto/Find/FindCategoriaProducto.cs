using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;


namespace OzyParkAdmin.Application.CategoriasProducto.Find;

/// <summary>
/// Busca por Id de Categoria de Producto
/// </summary>
/// <param name="categoriaProductoId">Id de la categoria de producto a buscar</param>
public sealed record FindCategoriaProducto(int categoriaProductoId) : IQuery<CategoriaProductoFullInfo>;