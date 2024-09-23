using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// Asigna o desasigna grupos etarios de un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
/// <param name="GruposEtarios">Los grupos etarios a ser asignados.</param>
public sealed record AssignGruposEtarios(int ServicioId, ImmutableArray<GrupoEtarioInfo> GruposEtarios) : IServicioStateChangeable;
