using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html;

internal abstract class HtmlFormattedReport : IFormattedReport
{
    public ActionType Format => ActionType.Html;

    public abstract ReportType Type { get; }

    public abstract ReportGenerated Generate();
}