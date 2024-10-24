using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.Zonas.List;

/// <summary>
/// Lista todas las zonas.
/// </summary>
public sealed record ListZonas : IQueryListOf<ZonaInfo>;
