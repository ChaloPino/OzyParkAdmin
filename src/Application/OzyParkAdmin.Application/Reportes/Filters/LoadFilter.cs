using MassTransit.Mediator;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Application.Reportes.Filters;

/// <summary>
/// Carga un filtro de tipo lista.
/// </summary>
/// <param name="ReportId">El id del reporte al que pertenece el filtro.</param>
/// <param name="FilterId">El id del filtro.</param>
/// <param name="Parameters">Los valores de parámetros que se requieren para la ejecución.</param>
public sealed record LoadFilter(Guid ReportId, int FilterId, string?[] Parameters) : Request<ResultListOf<ItemOption>>;
