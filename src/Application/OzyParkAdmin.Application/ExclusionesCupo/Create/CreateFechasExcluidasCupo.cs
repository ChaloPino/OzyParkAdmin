using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.ExclusionesCupo.Create;

/// <summary>
/// Crea varias fechas de exclusión para los cupos.
/// </summary>
/// <param name="CentroCosto">El centro de costo.</param>
/// <param name="CanalesVenta">Lista de canales de venta.</param>
/// <param name="FechaDesde">La fecha desde para crear las fechas de exclusión.</param>
/// <param name="FechaHasta">La fecha hasta para crear las fechas de exclusión.</param>
public sealed record CreateFechasExcluidasCupo(
    CentroCostoInfo CentroCosto,
    ImmutableArray<CanalVenta> CanalesVenta,
    DateOnly FechaDesde,
    DateOnly FechaHasta) : ICommand;
