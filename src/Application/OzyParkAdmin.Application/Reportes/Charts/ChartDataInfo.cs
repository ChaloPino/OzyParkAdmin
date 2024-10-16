using OzyParkAdmin.Domain.Reportes.Charts;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// La información de los datos del gráfico.
/// </summary>
public class ChartDataInfo : ChartDictionary
{
    internal static ChartDataInfo Create(Chart chart, DataSet dataSet, ChartReport report, ClaimsPrincipal user)
    {
        ChartDataInfo info = [];

        if (chart.Type != ChartType.Table)
        {
            List<ChartDataSetInfo> dataSets = chart.DataSets.Select(x => ChartDataSetInfo.Create(x, dataSet, report)).ToList();
            info.Add("datasets", dataSets);
            List<object?> labels = CreateLabels(chart, dataSet, report);

            if (labels.Count > 0)
            {
                info.Add("labels", labels);
            }
        }
        else
        {
            ChartTableInfo? table = ChartHelper.ExtractDataForTable(chart, report, dataSet, user);

            if (table is not null)
            {
                info.Add("table", table);
            }
        }

        return info;
    }

    private static List<object?> CreateLabels(Chart chart, DataSet dataSet, ChartReport report)
    {
        List<object?> labels = [];

        if (!string.IsNullOrEmpty(chart.Labels))
        {
            string[] localLabels = chart.Labels.Split([','], StringSplitOptions.RemoveEmptyEntries);
            labels.AddRange(localLabels.Select(x => x.Trim()));
        }

        if (chart.LabelDataSource is not null || chart.LabelDataSourceIndex is not null)
        {
            DataTable? labelDataTable = ChartHelper.ExtractDataTable(dataSet, chart.LabelDataSourceIndex, chart.LabelDataSource?.Name ?? report.DataSource.Name);
            ChartHelper.ExtractData(labelDataTable, chart.LabelColumnName, labels);
        }

        if (chart.DataSets is not null)
        {
            foreach (ChartDataSet chartDataSet in chart.DataSets.Where(c => c.DataType == ChartDataType.ObjectToArray))
            {
                DataTable? dataTable = ChartHelper.ExtractDataTable(dataSet, chartDataSet.DataSourceIndex, chartDataSet.DataSource?.Name ?? report.DataSource.Name);
                ChartHelper.ExtractColumnsAsLabels(dataTable, labels);
            }
        }

        return labels;
    }
}