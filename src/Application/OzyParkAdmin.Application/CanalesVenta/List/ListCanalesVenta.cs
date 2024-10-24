using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;

namespace OzyParkAdmin.Application.CanalesVenta.List;

/// <summary>
/// Lista todos los canales de venta.
/// </summary>
public sealed record ListCanalesVenta : IQueryListOf<CanalVenta>;
