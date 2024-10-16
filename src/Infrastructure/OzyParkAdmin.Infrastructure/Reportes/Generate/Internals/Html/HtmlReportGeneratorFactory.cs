using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Charts;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Listed;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.MasterDetail;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Paginated;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Pivoted;
using System.Diagnostics;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html;
internal static class HtmlReportGeneratorFactory
{
    public static ITypedHtmlReportGenerator Create(ReportType type)
    {
        return type switch
        {
            ReportType.Listed => new ListedHtmlReportGenerator(),
            ReportType.MasterDetail => new MasterDetailHtmlReportGenerator(),
            ReportType.Paginated => new PaginatedHtmlReportGenerator(),
            ReportType.Pivoted => new PivotedHtmlReportGenerator(),
            ReportType.Chart => new ChartHtmlReportGenerator(),
            _ => throw new UnreachableException(),
        };
    }
}
