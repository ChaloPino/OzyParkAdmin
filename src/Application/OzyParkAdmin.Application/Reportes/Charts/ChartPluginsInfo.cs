using OzyParkAdmin.Domain.Reportes.Charts;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// La información de plugins para el gráfico.
/// </summary>
public sealed class ChartPluginsInfo : ChartDictionary
{
    internal static ChartPluginsInfo Create(Chart chart)
    {
        ChartPluginsInfo info = new();
        chart.Decimation.AddDecimationTo(info);
        chart.Legend.AddLegendTo(info);
        chart.Title.AddTitleTo("title", info);
        chart.Subtitle.AddTitleTo("subtitle", info);
        chart.Tooltip.AddTooltipTo(info);
        chart.DataLabels.AddDataLabels(info);
        return info;
    }
}