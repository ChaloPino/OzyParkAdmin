using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Application.Tramos.List;

/// <summary>
/// Lista todos los tramos.
/// </summary>
public sealed record ListTramos : IQueryListOf<TramoInfo>;
