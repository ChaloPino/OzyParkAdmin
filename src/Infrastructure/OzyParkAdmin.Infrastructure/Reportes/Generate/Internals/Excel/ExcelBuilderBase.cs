using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal abstract class ExcelBuilderBase<TReport>
    where TReport : Report
{
    public virtual byte[]? Build(TReport report, ReportFilter reportFilter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        return null;
    }

    public virtual byte[]? Build(TReport report, ReportFilter reportFilter, ExcelReportTemplate template, DataTable dataTable, IEnumerable<DataRow> dataRows, ClaimsPrincipal user)
    {
        return null;
    }

    protected static byte[] BuildExcel(string sheetName, Style style, SharedTable sharedTable, Worksheet worksheet)
    {
        using MemoryStream ms = new();
        using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();

            ThemePart themePart = workbookPart.AddNewPart<ThemePart>();
            AddPart(themePart, ExcelHelper.GetThemeStream());
            style.Write(workbookPart);
            sharedTable.Write(workbookPart);

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            WriteWorksheet(worksheetPart, worksheet);

            using (OpenXmlWriter writer = OpenXmlWriter.Create(workbookPart))
            {
                writer.WriteStartElement(new Workbook());
                writer.WriteStartElement(new Sheets());

                writer.WriteElement(new Sheet
                {
                    Name = sheetName,
                    SheetId = 1,
                    Id = workbookPart.GetIdOfPart(worksheetPart)
                });

                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            document.Save();
        }

        return ms.ToArray();
    }

    private static void WriteWorksheet(WorksheetPart workSheetPart, Worksheet worksheet)
    {
        using OpenXmlWriter writer = OpenXmlWriter.Create(workSheetPart);
        writer.WriteElement(worksheet);
    }

    private static void AddPart<TPart>(TPart stylesPart, Stream stream)
        where TPart : OpenXmlPart
    {
        stylesPart.FeedData(stream);
    }
}
