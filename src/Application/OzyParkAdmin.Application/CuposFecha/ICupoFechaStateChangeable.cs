using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;

namespace OzyParkAdmin.Application.CuposFecha;

/// <summary>
/// Request para cualquier cambio de estado de un cupo fecha.
/// </summary>
public interface ICupoFechaStateChangeable : ICommand<CupoFechaFullInfo>;
