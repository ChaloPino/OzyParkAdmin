using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Application.Reportes.MasterDetails;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.MasterDetail;
internal sealed class MasterDetailHtmlFormattedReport : HtmlFormattedReport
{
    public override ReportType Type => ReportType.MasterDetail;

    public MasterTable? Master { get; set; }

    public IEnumerable<DetailTable> Details { get; set; } = [];

    internal static MasterDetailHtmlFormattedReport Create(MasterTable? master, IEnumerable<DetailTable> details)
    {
        return new()
        {
            Master = master,
            Details = details,
        };
    }

    public override ReportGenerated Generate() =>
        new()
        {
            Type= Type,
            Format = Format,
            MasterTable = Master,
            Details = Details
        };
}
