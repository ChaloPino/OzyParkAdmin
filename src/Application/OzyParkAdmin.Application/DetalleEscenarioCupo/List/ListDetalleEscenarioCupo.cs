using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.List;
public sealed record ListDetalleEscenarioCupo(int EscenarioCupoId) : IQueryListOf<DetalleEscenarioCupoInfo>;

