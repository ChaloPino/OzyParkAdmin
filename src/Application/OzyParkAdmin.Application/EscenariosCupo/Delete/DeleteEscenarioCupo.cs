using OzyParkAdmin.Application.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Delete;

/// <summary>
/// Elimina varios escenarios cupo.
/// </summary>
/// <param name="EscenariosCupos">La información de los escenariosc cupos que se quieren eliminar.</param>
public sealed record DeleteEscenarioCupo(int[] ids) : ICommand;
