using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;

internal abstract class FormatReportGenerator : IFormatReportGenerator
{
    public IFormattedReport GenerateReport(Report report, ReportFilter filter, ClaimsPrincipal user)
    {
        DataSet dataSet = DataSetExecutor.ExecuteDataSet(report.DataSource, report, filter);
        return GenerateReport(report, filter, dataSet, user);
    }

    protected abstract IFormattedReport GenerateReport(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user);
}