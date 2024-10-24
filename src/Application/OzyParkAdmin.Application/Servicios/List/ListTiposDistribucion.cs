using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los tipos de distribución.
/// </summary>
public sealed record ListTiposDistribucion : IQueryListOf<TipoDistribucion>;
