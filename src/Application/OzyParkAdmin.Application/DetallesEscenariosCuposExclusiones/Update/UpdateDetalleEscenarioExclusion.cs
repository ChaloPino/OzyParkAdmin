using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;

namespace OzyParkAdmin.Application.DetallesEscenariosExclusiones.Update;
/// <summary>
/// Actualiza un escenario cupo
/// </summary>
/// <param name="Id"></param>
/// <param name="escenarioCupoId">El escenario cupo padre.</param>
/// <param name="exclusiones">Las Exclusiones del escenario.</param>
public sealed record UpdateDetalleEscenarioExclusion(
    int escenarioCupoId,
    IEnumerable<DetalleEscenarioCupoExclusionFullInfo> exclusiones) : ICommand;
