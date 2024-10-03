using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los servicios que pertenecen a un centro de costo.
/// </summary>
/// <param name="CentroCostoId">El id del centro de costo.</param>
public sealed record ListServiciosPorCentroCosto(int CentroCostoId) : Request<ResultListOf<ServicioInfo>>;
