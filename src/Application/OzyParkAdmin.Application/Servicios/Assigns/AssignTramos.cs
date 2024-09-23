using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// Asigna o desasigna tramos a un servicio.
/// </summary>
/// <param name="ServicioId">El id de servicio.</param>
/// <param name="Tramos">Los tramos a ser asignados.</param>
public sealed record AssignTramos(int ServicioId, ImmutableArray<TramoServicioInfo> Tramos) : IServicioStateChangeable;
