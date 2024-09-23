using MassTransit.Mediator;
using OzyParkAdmin.Domain.Plantillas;

namespace OzyParkAdmin.Application.Plantillas.List;

/// <summary>
/// Lista todas las plantillas.
/// </summary>
public sealed record ListPlantillas : Request<ResultListOf<Plantilla>>;
