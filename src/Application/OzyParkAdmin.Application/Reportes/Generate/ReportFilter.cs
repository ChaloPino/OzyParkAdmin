using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;
using System.Data;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// Los datos necesarios para realizar el filtrado del reporte al generarse.
/// </summary>
/// <param name="FilterValues">Los valores de filtrado.</param>
/// <param name="SortExpressions">Los valores de ordenamiento.</param>
/// <param name="Page">La página que se quiere consultar.</param>
/// <param name="PageSize">El tamaño de la página.</param>
public sealed record ReportFilter(ImmutableArray<FilterValue> FilterValues, SortExpressionCollection<DataRow> SortExpressions, int Page, int PageSize);
