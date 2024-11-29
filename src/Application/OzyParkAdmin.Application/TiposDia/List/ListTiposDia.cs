using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Application.TiposDia.List;

/// <summary>
/// Lista los tipos de días.
/// </summary>
public sealed record ListTiposDia : IQueryListOf<TipoDia>;
