using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// Asigna o desasigna zonas de un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
/// <param name="Zonas">Las zonas a ser asignadas.</param>
public sealed record AssignZonas(int ServicioId, ImmutableArray<ZonaPorTramoInfo> Zonas) : IServicioStateChangeable;
