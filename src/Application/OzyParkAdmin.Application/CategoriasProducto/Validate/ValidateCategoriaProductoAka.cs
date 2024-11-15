using OzyParkAdmin.Application.Shared;

namespace OzyParkAdmin.Application.CategoriasProducto.Validate;

/// <summary>
/// Valida si el Aka de la categoria de prodcuto esta duplicado
/// </summary>
/// <param name="CategoriaProductoId">ID de la Categoria de Producto</param>
/// <param name="FranquiciaId">Id de la Franquicia</param>
/// <param name="Aka">Aka a Validar</param>
public sealed record ValidateCategoriaProductoAka(int CategoriaProductoId, int FranquiciaId, string? Aka) : ICommand;
