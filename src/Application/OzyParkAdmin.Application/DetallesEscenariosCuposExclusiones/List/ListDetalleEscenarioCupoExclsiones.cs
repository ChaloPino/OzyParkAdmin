using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;

namespace OzyParkAdmin.Application.DetallesEscenariosCuposExclusiones.List;
public sealed record ListDetalleEscenarioCupoExclsiones(int EscenarioCupoId) : IQueryListOf<DetalleEscenarioCupoExclusionFullInfo> { }
