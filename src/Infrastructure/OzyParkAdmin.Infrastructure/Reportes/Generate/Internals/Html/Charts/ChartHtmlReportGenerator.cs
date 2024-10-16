using Microsoft.CodeAnalysis.CSharp.Syntax;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Charts;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Charts;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Charts;
internal sealed class ChartHtmlReportGenerator : ITypedHtmlReportGenerator
{
    public HtmlFormattedReport FormatToHtml(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        return FormatToHtml((ChartReport)report, filter, dataSet, user);
    }

    private static HtmlFormattedReport FormatToHtml(ChartReport report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        ExecuteCharts(report, dataSet, filter);

        List<ChartMetaInfo> charts = [];

        foreach (var chart in report.Charts.Where(chart => chart.IsAccessibleByUser(user)))
        {
            ChartMetaInfo chartInfo = CreateChartInfo(chart, report, dataSet, user);
            charts.Add(chartInfo);
        }

        return ChartHtmlFormattedReport.Create(charts);
    }

    private static ChartMetaInfo CreateChartInfo(Chart chart, ChartReport report, DataSet dataSet, ClaimsPrincipal user) =>
        ChartMetaInfo.Create(chart, report, dataSet, user);

    private static void ExecuteCharts(ChartReport report, DataSet dataSet, ReportFilter filter)
    {
        foreach (Chart chart in report.Charts)
        {
            if (chart.TableDataSource is not null)
            {
                DataSetExecutor.ExecuteDataTable(chart.TableDataSource, report, filter, dataSet);
            }

            if (chart.DataSets is null)
            {
                continue;
            }

            foreach (ChartDataSet chartDataSet in chart.DataSets.Where(ds => ds.DataSource is not null))
            {
                DataSetExecutor.ExecuteDataTable(chartDataSet.DataSource!, report, filter, dataSet);
            }
        }

        // Se ejecuta al final por que los labels no se tiene que contabilizar como índices de las otras fuentes de datos.
        foreach (Chart chart in report.Charts.Where(ch => ch.LabelDataSource is not null))
        {
            DataSetExecutor.ExecuteDataTable(chart.LabelDataSource!, report, filter, dataSet);
        }
    }
}
