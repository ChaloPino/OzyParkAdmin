using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.Find;

/// <summary>
/// Busca un servicio dado el id de servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio a buscar.</param>
public sealed record FindServicio(int ServicioId) : IQuery<ServicioFullInfo>;
