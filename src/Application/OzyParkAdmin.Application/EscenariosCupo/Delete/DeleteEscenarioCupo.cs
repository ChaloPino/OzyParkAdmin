using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.EscenariosCupo;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.ExclusionesCupo.Delete;

/// <summary>
/// Elimina varios escenarios cupo.
/// </summary>
/// <param name="EscenariosCupos">La información de los escenariosc cupos que se quieren eliminar.</param>
public sealed record DeleteEscenarioCupo(ImmutableArray<EscenarioCupoFullInfo> EscenariosCupos) : ICommand;
