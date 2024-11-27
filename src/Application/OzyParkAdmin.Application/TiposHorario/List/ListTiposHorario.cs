using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Application.TiposHorario.List;

/// <summary>
/// Lista todos los tipos de horario.
/// </summary>
public sealed record ListTiposHorario : IQueryListOf<TipoHorario>;
