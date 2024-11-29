using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.List;
public sealed record ListEscenarioCupoExclusionesPorFecha(
    int escenarioCupoId
    ) : IQueryListOf<DetalleEscenarioCupoExclusionFechaFullInfo>;
