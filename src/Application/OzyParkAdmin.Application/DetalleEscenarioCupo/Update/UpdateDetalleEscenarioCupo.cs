using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.Update;
public sealed record UpdateDetalleEscenarioCupo(
    int EscenarioCupoId,
    IEnumerable<DetalleEscenarioCupoInfo> Detalles) : ICommand;
