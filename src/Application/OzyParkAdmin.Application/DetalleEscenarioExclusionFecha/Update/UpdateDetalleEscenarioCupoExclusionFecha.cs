using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.Update;
public sealed record UpdateDetalleEscenarioCupoExclusionFecha(
    int EscenarioCupoId,
    IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> ExclusionesFecha
    ) : ICommand;
