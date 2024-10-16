using OzyParkAdmin.Domain.Reportes.Charts;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// La información de un gráfico.
/// </summary>
public class ChartMetaInfo
{
    /// <summary>
    /// El id del reporte.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del reporte.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// El ancho del gráfico.
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// El alto del gráfico.
    /// </summary>
    public string? Heigth { get; set; }

    /// <summary>
    /// El tipo de gráfico.
    /// </summary>
    public ChartType? Type { get; set; }

    /// <summary>
    /// Los datos del gráfico.
    /// </summary>
    public ChartDataInfo Data { get; set; } = default!;

    /// <summary>
    /// Las opciones del gráfico.
    /// </summary>
    public ChartOptionsInfo Options { get; set; } = default!;

    /// <summary>
    /// Crea un <see cref="ChartMetaInfo"/> desde <paramref name="chart"/>.
    /// </summary>
    /// <param name="chart">El <see cref="Chart"/> a ser convertido.</param>
    /// <param name="report">El <see cref="ChartReport"/>.</param>
    /// <param name="dataSet">El <see cref="DataSet"/>.</param>
    /// <param name="user">El usuario.</param>
    /// <returns>El nuevo <see cref="ChartMetaInfo"/>.</returns>
    public static ChartMetaInfo Create(Chart chart, ChartReport report, DataSet dataSet, ClaimsPrincipal user)
    {
        return new()
        {
            Id = chart.Id,
            Name = chart.Name,
            Width = chart.Width,
            Heigth = chart.Height,
            Type = chart.Type,
            Options = ChartOptionsInfo.Create(chart),
            Data = ChartDataInfo.Create(chart, dataSet, report, user)
        };
    }
}
