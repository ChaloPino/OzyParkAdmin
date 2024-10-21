using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Listed;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Paginated;
internal sealed class PaginatedPdfReportGenerator : TypedPdfReportGenerator<PaginatedReport>
{
    public PaginatedPdfReportGenerator()
        : base(new PdfBuilder<PaginatedReport>())
    {
    }

    protected override byte[]? CreateExcel(PaginatedReport report, ReportFilter filter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        DataTable dataTable = ExtractDataTable(report, dataSet);
        return Builder.Build(report, filter, template, dataTable, user);
    }

    private static DataTable ExtractDataTable(PaginatedReport report, DataSet dataSet)
    {
        return report switch
        {
            { ServerSide: true, TotalRecordsDataSource: null, DataResultIndex: null } => throw new InvalidOperationException("Si el reporte se ejecuta en el lado del servidor, debe asociarse la 'Fuente de datos para totales' o el 'Íncide de resultado de datos'."),
            { ServerSide: true, DataResultIndex: not null, TotalRecordsResultIndex: not null } => dataSet.Tables[report.DataResultIndex.Value],
            _ => dataSet.Tables[0],
        };
    }
}
