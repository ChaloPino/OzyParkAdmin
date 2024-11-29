using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.GruposEtarios.List;

/// <summary>
/// Lista todos los grupos etarios.
/// </summary>
public sealed record ListGruposEtarios : IQueryListOf<GrupoEtarioInfo>;
