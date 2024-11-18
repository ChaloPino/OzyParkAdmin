using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.ExclusionesCupo.Create;
using OzyParkAdmin.Application.ExclusionesCupo.Delete;
using OzyParkAdmin.Application.ExclusionesCupo.Search;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;

internal static class FechaExcluidaCupoMappers
{
    public static SearchFechasExcluidasCupo ToSearch(this GridState<FechaExcluidaCupoViewModel> state, ClaimsPrincipal user, string? searchText) =>
        new(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<FechaExcluidaCupo> ToFilterExpressions(this GridState<FechaExcluidaCupoViewModel> state)
    {
        FilterExpressionCollection<FechaExcluidaCupo> filterExpressions = new();

        foreach (IFilterDefinition<FechaExcluidaCupoViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<FechaExcluidaCupoViewModel> filterDefinition, FilterExpressionCollection<FechaExcluidaCupo> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            "CentroCosto.Descripcion" => filterExpressions.Add(x => x.CentroCosto.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "CanalVenta.Nombre" => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(FechaExcluidaCupoViewModel.Fecha) => filterExpressions.Add(x => x.Fecha, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<FechaExcluidaCupo> ToSortExpressions(this GridState<FechaExcluidaCupoViewModel> state)
    {
        SortExpressionCollection<FechaExcluidaCupo> sortExpressions = state.SortDefinitions.Count == 0
            ? SortExpressionCollection<FechaExcluidaCupo>.CreateDefault(x => x.CentroCosto.Descripcion, false)
            : new();

        foreach (SortDefinition<FechaExcluidaCupoViewModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<FechaExcluidaCupoViewModel> sortDefinition, SortExpressionCollection<FechaExcluidaCupo> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            "CentroCosto.Descripcion" => sortExpressions.Add(x => x.CentroCosto.Descripcion, sortDefinition.Descending),
            "CanalVenta.Nombre" => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            nameof(FechaExcluidaCupoViewModel.Fecha) => sortExpressions.Add(x => x.Fecha, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<FechaExcluidaCupoViewModel> ToGridData(this PagedList<FechaExcluidaCupoFullInfo> source, IMudStateHasChanged stateHasChanged) =>
        new([.. source.Items.Select(ToViewModel)], source.TotalCount, stateHasChanged);

    private static FechaExcluidaCupoViewModel ToViewModel(this FechaExcluidaCupoFullInfo fechaExcluida) =>
        new() { CentroCosto = fechaExcluida.CentroCosto, CanalVenta = fechaExcluida.CanalVenta, Fecha = fechaExcluida.Fecha, };

    public static CreateFechasExcluidasCupo ToCreate(this FechasExcluidasCupoModel model) =>
        new(model.CentroCosto, [.. model.CanalesVenta], model.RangoFechas.Start.ToDateOnly(), model.RangoFechas.End.ToDateOnly());

    public static DeleteFechasExcluidasCupo ToDelete(this IEnumerable<FechaExcluidaCupoViewModel> source) =>
        new([.. source.Select(ToInfo)]);

    private static FechaExcluidaCupoFullInfo ToInfo(this FechaExcluidaCupoViewModel fechaExcluida) =>
        new() { CentroCosto = fechaExcluida.CentroCosto, CanalVenta = fechaExcluida.CanalVenta, Fecha = fechaExcluida.Fecha };

    private static DateOnly ToDateOnly(this DateTime? date) =>
        date is null ? DateOnly.FromDateTime(DateTime.Today) : DateOnly.FromDateTime(date.Value);
}
