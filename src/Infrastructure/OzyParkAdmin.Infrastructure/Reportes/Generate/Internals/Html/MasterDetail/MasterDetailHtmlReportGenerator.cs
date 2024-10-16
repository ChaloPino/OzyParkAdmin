using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Application.Reportes.MasterDetails;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.MasterDetail;
internal sealed class MasterDetailHtmlReportGenerator : ITypedHtmlReportGenerator
{
    public HtmlFormattedReport FormatToHtml(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        return FormatToHtml((MasterDetailReport)report, filter, dataSet, user);
    }

    public static HtmlFormattedReport FormatToHtml(MasterDetailReport report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        ExecuteDetails(report, dataSet, filter);

        List<ColumnInfo> columns = [];
        List<DataInfo> data = [];
        MasterTable? masterData = null;
        List<DetailTable> details = [];

        if (TryCreateInfo(dataSet, report.MasterResultIndex, report.DataSource.Name, report.Columns, columns, data, user))
        {
            masterData = MasterTable.FromMaster(report, columns, data);
        }

        if (report.HasDetail)
        {
            foreach (ReportDetail detail in report.Details)
            {
                columns = [];
                data = [];
                if (TryCreateInfo(dataSet, detail.DetailResultSetIndex, detail.DetailDataSource?.Name, detail.DetailColumns, columns, data, user))
                {
                    DetailTable detailData = DetailTable.FromMaster(detail, columns, data);
                    details.Add(detailData);
                }
            }
        }

        return MasterDetailHtmlFormattedReport.Create(masterData, details);
    }

    private static void ExecuteDetails(MasterDetailReport report, DataSet dataSet, ReportFilter filter)
    {
        if (report.HasDetail && !report.HasDynamicDetails && report.Details is not null)
        {
            foreach (ReportDetail detail in report.Details)
            {
                if (!detail.DetailResultSetIndex.HasValue && detail.DetailDataSource is not null)
                {
                    DataSetExecutor.ExecuteDataTable(detail.DetailDataSource, report, filter, dataSet);
                }
            }
        }
    }

    private static bool TryCreateInfo(DataSet dataSet, int? index, string? name, IEnumerable<ColumnBase> columns, List<ColumnInfo> columnsInfo, List<DataInfo> data, ClaimsPrincipal user)
    {
        DataTable? dataTable = ExtractDataTable(dataSet, index, name);

        if (dataTable is null)
        {
            return false;
        }

        List<ColumnBase>? columnsBase = [..columns
               .Where(column => column.IsAccessibleByUser(user))
            .OrderBy(c => c.Order)];

        var dataRows = dataTable.AsEnumerable();

        foreach (ColumnBase column in columnsBase)
        {
            columnsInfo.Add(ColumnInfo.FromColumn(column));
        }

        foreach (var row in dataRows)
        {
            DataInfo dataInfo = new();

            foreach (ColumnInfo column in columnsInfo)
            {
                dataInfo[column] = row[column.Name];
            }

            data.Add(dataInfo);
        }

        return true;
    }

    private static DataTable? ExtractDataTable(DataSet dataSet, int? index, string? name) =>
        index is not null ? dataSet.Tables[index.Value] : ExtractDataTableByName(dataSet, name);

    private static DataTable? ExtractDataTableByName(DataSet dataSet, string? name) =>
        !string.IsNullOrEmpty(name) ? dataSet.Tables[name] : null;
}
