using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// Los datos necesarios para realizar el filtrado del reporte al generarse.
/// </summary>
/// <param name="FilterValues">Los valores de filtrado.</param>
/// <param name="SortExpressions">Los valores de ordenamiento.</param>
/// <param name="Page">La página que se quiere consultar.</param>
/// <param name="PageSize">El tamaño de la página.</param>
public sealed record ReportFilter(ImmutableArray<FilterValue> FilterValues, SortExpressionCollection<DataRow> SortExpressions, int Page, int PageSize)
{
    /// <summary>
    /// Trata de conseguir el valor de un filtro.
    /// </summary>
    /// <param name="filterId">El id del filtro.</param>
    /// <param name="value">El valor conseguido.</param>
    /// <returns><c>true</c> si es que el <paramref name="filterId"/> existe en la lista de valores de filtros; en caso contrario <c>false</c>.</returns>
    public bool TryGetFilter(int filterId, [NotNullWhen(true)] out object? value)
    {
        value = FilterValues.FirstOrDefault(x => x.FilterId == filterId)?.Value;
        return value is not null;
    }

    /// <summary>
    /// Consigue el valor de un filtro.
    /// </summary>
    /// <param name="filterId">El id del filtro.</param>
    /// <returns>El valor conseguido del filtro.</returns>
    public object? GetFilter(int filterId) =>
        TryGetFilter(filterId, out object? value) ? value : null;


        
}
