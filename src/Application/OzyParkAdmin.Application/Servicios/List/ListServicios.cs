using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los servicios.
/// </summary>
/// <param name="FranquiciaId">El id de franquicia a la cual pertenecen los servicios que se buscan.</param>
public sealed record ListServicios(int FranquiciaId) : IQueryListOf<ServicioInfo>;
