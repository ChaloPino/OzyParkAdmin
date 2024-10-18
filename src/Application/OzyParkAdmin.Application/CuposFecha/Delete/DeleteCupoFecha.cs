using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Delete;

/// <summary>
/// Elimina un cupo por fecha.
/// </summary>
/// <param name="Id">El id del cupo por fecha a eliminar.</param>
public sealed record DeleteCupoFecha(int Id) : Request<SuccessOrFailure>;
