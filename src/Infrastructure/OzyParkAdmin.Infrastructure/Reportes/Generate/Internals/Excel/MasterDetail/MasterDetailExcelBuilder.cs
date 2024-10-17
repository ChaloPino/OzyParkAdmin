using DocumentFormat.OpenXml.Spreadsheet;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.MasterDetail;
internal sealed partial class MasterDetailExcelBuilder : ExcelBuilderBase<MasterDetailReport>
{
    public override byte[]? Build(MasterDetailReport report, ReportFilter reportFilter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        SharedTable sharedTable = new();
        Style style = new();
        Dictionary<int, List<ExcelColumn>> excelColumns = [];
        Helper.CreateSharedTable(template, report, dataSet, user, sharedTable, excelColumns);
        Helper.CreateStyle(template, report, excelColumns, style);

        List<Row> rows = [];
        Dictionary<int, int> titlePositions = [];
        Helper.CreateRows(template, report, dataSet, excelColumns, sharedTable, rows, titlePositions);

        IEnumerable<Column> cols = Helper.CreateColumns(excelColumns, dataSet, report);
        IEnumerable<MergeCell> merges = ExcelBuilderHelper.CreateMerges(report, template, excelColumns, titlePositions);

        Worksheet worksheet = Helper.CreateWorksheet(dataSet, excelColumns, report, template.HasHeader, rows, cols, merges);

        return BuildExcel(report.Aka, style, sharedTable, worksheet);
    }
}
