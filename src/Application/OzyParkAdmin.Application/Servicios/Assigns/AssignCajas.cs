using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// Asigna o desasigna cajas de un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
/// <param name="Cajas">Las cajas a ser asignadas.</param>
public sealed record AssignCajas(int ServicioId, ImmutableArray<ServicioPorCajaInfo> Cajas) : IServicioStateChangeable;
