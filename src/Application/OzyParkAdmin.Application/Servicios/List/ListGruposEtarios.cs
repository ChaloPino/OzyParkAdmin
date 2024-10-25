using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los grupos etarios.
/// </summary>
public sealed record ListGruposEtarios : IQueryListOf<GrupoEtarioInfo>;
