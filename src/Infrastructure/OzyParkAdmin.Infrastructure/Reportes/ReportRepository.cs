using Dapper;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.DataSources;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Reportes.Internals;
using OzyParkAdmin.Infrastructure.Shared;
using System.Collections.Immutable;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes;

/// <summary>
/// El repositorio de <see cref="Report"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class ReportRepository(OzyParkAdminContext context) : Repository<Report>(context), IReportRepository
{
    /// <inheritdoc/>
    public async Task<ResultOf<Report>> FindReportByAkaAsync(string aka, string[] roles, CancellationToken cancellationToken)
    {
        Report? report = await EntitySet
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Aka == aka, cancellationToken).ConfigureAwait(false);

        if (report is null)
        {
            return new NotFound();
        }

        if (!report.IsInRole(roles))
        {
            return new Unauthorized();
        }

        return report;
    }

    /// <inheritdoc/>
    public async Task<List<ReportGroupInfo>> FindReportGroupsAsync(string[] roles, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(roles);

        List<Report> reportes = await EntitySet
            .IgnoreAutoIncludes()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.ReportGroup)
            .Where(x => x.Published)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        return reportes
            .GroupBy(x => (x.ReportGroup.Name, x.ReportGroup.Order))
            .Select(x => new ReportGroupInfo(x.Key.Name, x.Key.Order ?? 0, ToReportInfo(x, roles)))
            .Where(x => x.Reports.Length > 0)
            .ToList();
    }

    private static ImmutableArray<ReportInfo> ToReportInfo(IEnumerable<Report> source, string[] roles) =>
        [.. source.Where(x => x.IsInRole(roles)).Select(ToReportInfo)];

    private static ReportInfo ToReportInfo(Report report) =>
        new(report.Aka, report.Title, report.Order, [.. report.Roles]);

    /// <inheritdoc/>
    public async Task<List<ItemOption>> LoadFilterAsync(Guid reportId, int filterId, string?[] parameters, CancellationToken cancellationToken)
    {
        ListFilter? filter = await Context.Set<ListFilter>().FirstOrDefaultAsync(x => x.ReportId == reportId && x.Id == filterId, cancellationToken).ConfigureAwait(false);

        if (filter is null)
        {
            return [];
        }

        if (!filter.IsRemote)
        {
            return filter.List.ToList();
        }

        if (filter.RemoteDataSource is null)
        {
            return [];
        }

        if (parameters.Length != filter.RemoteDataSource.Parameters.Count())
        {
            return [];
        }

        return await ExecuteFilterAsync<ItemOption>(filter.RemoteDataSource, filter, parameters, cancellationToken).ConfigureAwait(false);
    }

    private async Task<List<T>> ExecuteFilterAsync<T>(DataSource dataSource, ListFilter filter, string?[] parameters, CancellationToken cancellationToken)
    {
        CommandDefinition command = new(
            commandText: dataSource.Script,
            parameters: PrepareParameters(dataSource.Parameters, filter, parameters),
            commandType: dataSource.IsStoredProcedure ? CommandType.StoredProcedure : CommandType.Text,
            cancellationToken: cancellationToken);

        IEnumerable<T> results = await Context.Database.GetDbConnection().QueryAsync<T>(command);
        return [..results];
    }

    private static DynamicParameters? PrepareParameters(IEnumerable<Parameter> parameters, Filter filter, string?[] parameterValues)
    {
        DynamicParameters dynamicParameters = new();

        for (int i = 0; i < parameterValues.Length; i++)
        {
            Parameter parameter = parameters.ElementAt(i);
            dynamicParameters.Add(
                name: parameter.Name,
                value: ParameterUtils.ConvertValue(parameterValues[i], parameter.Type, filter, false),
                dbType: parameter.Type);
        }

        return dynamicParameters;
    }

    
}
