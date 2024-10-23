using Microsoft.CodeAnalysis.CSharp.Syntax;
using MudBlazor;
using OzyParkAdmin.Application.OmisionesCupo.Create;
using OzyParkAdmin.Application.OmisionesCupo.Delete;
using OzyParkAdmin.Application.OmisionesCupo.Search;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;
using System.Diagnostics;

namespace OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Models;

internal static class IgnoraEscenarioCupoExclusionMappers
{
    public static SearchOmisionesEscenarioCupoExlusion ToSearch(this GridState<IgnoraEscenarioCupoExclusionViewModel> state, string? searchText) =>
        new(searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<IgnoraEscenarioCupoExclusion> ToFilterExpressions(this GridState<IgnoraEscenarioCupoExclusionViewModel> state)
    {
        FilterExpressionCollection<IgnoraEscenarioCupoExclusion> filterExpressions = new();

        foreach (IFilterDefinition<IgnoraEscenarioCupoExclusionViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<IgnoraEscenarioCupoExclusionViewModel> filterDefinition, FilterExpressionCollection<IgnoraEscenarioCupoExclusion> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            "EscenarioCupo.Nombre" => filterExpressions.Add(x => x.EscenarioCupo.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "CanalVenta.Nombre" => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(IgnoraEscenarioCupoExclusionViewModel.FechaIgnorada) => filterExpressions.Add(x => x.FechaIgnorada, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<IgnoraEscenarioCupoExclusion> ToSortExpressions(this GridState<IgnoraEscenarioCupoExclusionViewModel> state)
    {
        SortExpressionCollection<IgnoraEscenarioCupoExclusion> sortExpressions = new();

        foreach (SortDefinition<IgnoraEscenarioCupoExclusionViewModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<IgnoraEscenarioCupoExclusionViewModel> sortDefinition, SortExpressionCollection<IgnoraEscenarioCupoExclusion> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            "EscenarioCupo.Nombre" => sortExpressions.Add(x => x.EscenarioCupo.Nombre, sortDefinition.Descending),
            "CanalVenta.Nombre" => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            nameof(IgnoraEscenarioCupoExclusionViewModel.FechaIgnorada) => sortExpressions.Add(x => x.FechaIgnorada, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static GridData<IgnoraEscenarioCupoExclusionViewModel> ToGridData(this PagedList<IgnoraEscenarioCupoExclusionFullInfo> source) =>
        new()
        {
            TotalItems = source.TotalCount,
            Items = [..source.Items.Select(ToViewModel)],
        };

    private static IgnoraEscenarioCupoExclusionViewModel ToViewModel(this IgnoraEscenarioCupoExclusionFullInfo omision) =>
        new() { EscenarioCupo = omision.EscenarioCupo, CanalVenta = omision.CanalVenta, FechaIgnorada = omision.FechaIgnorada, };

    public static CreateOmisionesEscenarioCupoExclusion ToCreate(this OmisionesCupoExclusionModel omisiones) =>
        new([.. omisiones.EscenariosCupo], [.. omisiones.CanalesVenta], omisiones.RangoFechas.ToStartDate(), omisiones.RangoFechas.ToEndDate());

    private static DateOnly ToStartDate(this DateRange dateRange) =>
        dateRange.Start.ToDateOnly();

    private static DateOnly ToEndDate(this DateRange dateRange) =>
        dateRange.End.ToDateOnly();

    private static DateOnly ToDateOnly(this DateTime? date) =>
        date is null ? DateOnly.FromDateTime(DateTime.Today) : DateOnly.FromDateTime(date.Value);

    public static DeleteOmisionesEscenarioCupoExclusion ToDelete(this IEnumerable<IgnoraEscenarioCupoExclusionViewModel> source) =>
        new([.. source.Select(ToFullInfo)]);

    private static IgnoraEscenarioCupoExclusionFullInfo ToFullInfo(this IgnoraEscenarioCupoExclusionViewModel model) =>
        new() { EscenarioCupo = model.EscenarioCupo, CanalVenta = model.CanalVenta, FechaIgnorada = model.FechaIgnorada };
}
