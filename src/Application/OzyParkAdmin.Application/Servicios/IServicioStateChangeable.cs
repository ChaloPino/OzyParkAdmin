using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios;

/// <summary>
/// Request para cualquier cambio de estado del servicio.
/// </summary>
public interface IServicioStateChangeable : ICommand<ServicioFullInfo>;
