using OzyParkAdmin.Application.Reportes.Charts;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Charts;
internal sealed class ChartHtmlFormattedReport : HtmlFormattedReport
{
    public override ReportType Type => ReportType.Chart;

    public IEnumerable<ChartMetaInfo> Charts { get; set; } = [];

    internal static ChartHtmlFormattedReport Create(IEnumerable<ChartMetaInfo> charts)
    {
        return new() { Charts = charts, };
    }

    public override ReportGenerated Generate() =>
        new()
        {
            Type = Type,
            Format = Format,
            Charts = Charts,
        };
}
