using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Validate;

/// <summary>
/// Valida si el aka del producto está duplicado.
/// </summary>
/// <param name="ProductoId">El id del producto a validar.</param>
/// <param name="FranquiciaId">El id de la franquicia.</param>
/// <param name="Aka">El aka del producto.</param>
public sealed record ValidateProductoAka(int ProductoId, int FranquiciaId, string? Aka) : Request<SuccessOrFailure>;
