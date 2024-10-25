using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los tipos de vigencia.
/// </summary>
public sealed record ListTiposVigencia : IQueryListOf<TipoVigencia>;
