using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Servicios.Assigns;

/// <summary>
/// Asigna o desasigna centros de costo a un servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio.</param>
/// <param name="CentrosCosto">Los centros de costo a asignar.</param>
public sealed record AssignCentrosCostoToServicio(int ServicioId, ImmutableArray<CentroCostoServicioInfo> CentrosCosto) : IServicioStateChangeable;
