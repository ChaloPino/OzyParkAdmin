using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

internal static class ReportMappers
{
    public static GenerateHtmlReport ToGenerateHtml(this FilterViewModel filterViewModel, ClaimsPrincipal user) =>
        new(filterViewModel.ReportAka, filterViewModel.ToFilter(), user);

    public static GenerateHtmlReport ToGenerateHtml(this Report report, ClaimsPrincipal user) =>
        new(report.Aka, report.ToFilter(), user);

    public static GenerateOtherFormat ToGenerateOtherFormat(this FilterViewModel filterViewModel, ActionType format, ClaimsPrincipal user) =>
        new(filterViewModel.ReportAka, format, filterViewModel.ToFilter(), user);

    private static ReportFilter ToFilter(this FilterViewModel filterViewModel) =>
        new(filterViewModel.Filters.ToFilterValues(), new SortExpressionCollection<DataRow>(), 0, 0);

    private static ReportFilter ToFilter(this Report report) =>
        new(report.Filters.ToFilterValues(), new SortExpressionCollection<DataRow>(), 0, 0);

    private static ImmutableArray<FilterValue> ToFilterValues(this IEnumerable<IFilterModel> source) =>
        [.. source.Select(ToFilterValue)];

    private static ImmutableArray<FilterValue> ToFilterValues(this IEnumerable<Filter> source) =>
        [.. source.Select(ToFilterValue)];

    private static FilterValue ToFilterValue(IFilterModel filterModel) =>
        new(filterModel.Id, filterModel.GetValue());

    private static FilterValue ToFilterValue(Filter filter) =>
        new(filter.Id, filter.GetDefaultValue());
}
