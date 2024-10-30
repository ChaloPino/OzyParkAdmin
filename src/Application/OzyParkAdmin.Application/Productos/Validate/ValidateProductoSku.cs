using OzyParkAdmin.Application.Shared;

namespace OzyParkAdmin.Application.Productos.Validate;

/// <summary>
/// Valida si el sku del producto está duplicado.
/// </summary>
/// <param name="ProductoId">El id del producto a validar.</param>
/// <param name="FranquiciaId">El id de la franquicia.</param>
/// <param name="Sku">El sku del producto.</param>
public sealed record ValidateProductoSku(int ProductoId, int FranquiciaId, string? Sku) : ICommand;
