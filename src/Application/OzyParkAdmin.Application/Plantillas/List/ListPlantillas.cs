using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Plantillas;

namespace OzyParkAdmin.Application.Plantillas.List;

/// <summary>
/// Lista todas las plantillas.
/// </summary>
public sealed record ListPlantillas : IQueryListOf<Plantilla>;
