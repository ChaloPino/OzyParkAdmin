using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Application.TiposSegmentacion.List;

/// <summary>
/// Lista todos los tipos de segmentación.
/// </summary>
public sealed record ListTiposSegmentacion : IQueryListOf<TipoSegmentacion>;
