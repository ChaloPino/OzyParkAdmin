using DocumentFormat.OpenXml.Spreadsheet;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal class ExcelBuilder<TReport> : ExcelBuilderBase<TReport>
    where TReport : Report
{
    public override byte[]? Build(TReport report, ReportFilter reportFilter, ExcelReportTemplate template, DataTable dataTable, IEnumerable<DataRow> dataRows, ClaimsPrincipal user)
    {
        List<ColumnBase> columns = [.. report.Columns.Cast<ColumnBase>().Where(column => column.IsAccessibleByUser(user)).OrderBy(c => c.Order)];
        List<ExcelColumn> excelColumns = columns.ConvertAll(c => new ExcelColumn(c));
        List<ExcelFilter> excelFilters = report.Filters
                                            .Where(f => f is not HiddenFilter)
                                            .OrderBy(f => f.Order)
                                            .Select(f => new ExcelFilter(f, f.GetText(reportFilter.GetFilter(f.Id)))).ToList();

        List<DataRow> rows = dataRows.ToList();

        IDictionary<string, object?>? totals = ExcelBuilderHelper.CreateTotals(columns, rows);

        SharedTable sharedTable = SharedTableHelper.CreateSharedTable(template, excelFilters, excelColumns, rows, totals);

        Style style = StyleHelper.CreateStyle(template, excelFilters, excelColumns);

        IEnumerable<Row> data = ExcelBuilderHelper.CreateRowData(template, excelFilters, excelColumns, rows, totals);

        IEnumerable<Column> cols = ExcelBuilderHelper.CreateColumns(excelColumns, rows, totals);

        Worksheet worksheet = ExcelBuilderHelper.CreateWorksheet(columns.Count, rows.Count, excelFilters.Count, totals != null, template.HasHeader, data, cols);

        return BuildExcel(report.Aka, style, sharedTable, worksheet);
    }
}
