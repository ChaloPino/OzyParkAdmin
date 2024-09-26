using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// Asigna o desasigna permisos a un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
/// <param name="Permisos">Los permisos a ser asignados.</param>
public sealed record AssignPermisosToServicio(int ServicioId, ImmutableArray<PermisoServicioInfo> Permisos) : IServicioStateChangeable;
