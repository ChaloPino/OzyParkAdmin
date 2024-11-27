using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Application.Monedas.List;

/// <summary>
/// Lista las monedas.
/// </summary>
public sealed record ListMonedas : IQueryListOf<Moneda>;
