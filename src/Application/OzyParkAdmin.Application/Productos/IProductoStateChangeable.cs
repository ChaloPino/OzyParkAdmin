using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos;

/// <summary>
/// Comando para cualquier cambio de estado del producto.
/// </summary>
public interface IProductoStateChangeable : ICommand<ProductoFullInfo>;
