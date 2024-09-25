using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos;

/// <summary>
/// Request para cualquier cambio de estado del producto.
/// </summary>
public interface IProductoStateChangeable : Request<ResultOf<ProductoFullInfo>>;
