using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.Create;
public sealed record CreateDetalleEscenarioCupo(
    int EscenarioCupoId,
    IEnumerable<DetalleEscenarioCupoInfo> Detalles
    ) : ICommand;
