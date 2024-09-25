using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Validate;

/// <summary>
/// Valida el aka de un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio a validar.</param>
/// <param name="FranquiciaId">El id de franquicia.</param>
/// <param name="Aka">El aka del servicio.</param>
public sealed record ValidateServicioAka(int ServicioId, int FranquiciaId, string? Aka) : Request<SuccessOrFailure>;
