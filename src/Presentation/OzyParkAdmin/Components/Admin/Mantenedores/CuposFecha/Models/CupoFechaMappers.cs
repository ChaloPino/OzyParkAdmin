using MudBlazor.Interfaces;
using MudBlazor;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;
using OzyParkAdmin.Application.CuposFecha.Search;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Application.CuposFecha.Create;
using OzyParkAdmin.Application.CuposFecha.Update;
using OzyParkAdmin.Application.CuposFecha.Delete;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;

internal static class CupoFechaMappers
{
    public static SearchCuposFecha ToSearch(this GridState<CupoFechaViewModel> state, ClaimsPrincipal user, string? searchText) =>
        new(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<CupoFecha> ToFilterExpressions(this GridState<CupoFechaViewModel> state)
    {
        FilterExpressionCollection<CupoFecha> filterExpressions = new();

        foreach (IFilterDefinition<CupoFechaViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<CupoFechaViewModel> filterDefinition, FilterExpressionCollection<CupoFecha> filterExpressions) =>
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(CupoFechaViewModel.Fecha) => filterExpressions.Add(x => x.Fecha, filterDefinition.Operator!, filterDefinition.Value),
            "EscenarioCupo.Nombre" => filterExpressions.Add(x => x.EscenarioCupo.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "CanalVenta.Nombre" => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "DiaSemana.Aka" => filterExpressions.Add(x => x.DiaSemana.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoFechaViewModel.HoraInicio) => filterExpressions.Add(x => x.HoraInicio, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoFechaViewModel.HoraFin) => filterExpressions.Add(x => x.HoraFin, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoFechaViewModel.Total) => filterExpressions.Add(x => x.Total, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoFechaViewModel.SobreCupo) => filterExpressions.Add(x => x.SobreCupo, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoFechaViewModel.TopeEnCupo) => filterExpressions.Add(x => x.TopeEnCupo, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };

    private static SortExpressionCollection<CupoFecha> ToSortExpressions(this GridState<CupoFechaViewModel> state)
    {
        SortExpressionCollection<CupoFecha> sortExpressions = state.SortDefinitions.Count == 0
            ? SortExpressionCollection<CupoFecha>.CreateDefault(x => x.Fecha, true)
            : new();

        foreach (SortDefinition<CupoFechaViewModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<CupoFechaViewModel> sortDefinition, SortExpressionCollection<CupoFecha> sortExpressions) =>
        _ = sortDefinition.SortBy switch
        {
            nameof(CupoFechaViewModel.Fecha) => sortExpressions.Add(x => x.Fecha, sortDefinition.Descending),
            "EscenarioCupo.Nombre" => sortExpressions.Add(x => x.EscenarioCupo.Nombre, sortDefinition.Descending),
            "CanalVenta.Nombre" => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            "DiaSemana.Aka" => sortExpressions.Add(x => x.DiaSemana.Id, sortDefinition.Descending),
            nameof(CupoFechaViewModel.HoraInicio) => sortExpressions.Add(x => x.HoraInicio, sortDefinition.Descending),
            nameof(CupoFechaViewModel.HoraFin) => sortExpressions.Add(x => x.HoraFin, sortDefinition.Descending),
            nameof(CupoFechaViewModel.Total) => sortExpressions.Add(x => x.Total, sortDefinition.Descending),
            nameof(CupoFechaViewModel.SobreCupo) => sortExpressions.Add(x => x.SobreCupo, sortDefinition.Descending),
            nameof(CupoFechaViewModel.TopeEnCupo) => sortExpressions.Add(x => x.TopeEnCupo, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };

    public static ObservableGridData<CupoFechaViewModel> ToGridData(this PagedList<CupoFechaFullInfo> source, IMudStateHasChanged stateHasChanged) =>
        new(source.Items.Select(ToViewModel), source.TotalCount, stateHasChanged);

    public static CupoFechaViewModel ToViewModel(this CupoFechaFullInfo cupo) =>
        new()
        {
            Id = cupo.Id,
            Fecha = cupo.Fecha,
            EscenarioCupo = cupo.EscenarioCupo,
            CanalVenta = cupo.CanalVenta,
            DiaSemana = cupo.DiaSemana,
            HoraInicio = cupo.HoraInicio,
            HoraFin = cupo.HoraFin,
            Total = cupo.Total,
            SobreCupo = cupo.SobreCupo,
            TopeEnCupo = cupo.TopeEnCupo,
        };

    public static CreateCuposFecha ToCreate(this CuposFechaModel model) =>
        new(
            model.RangoFechas.Start.ToDateOnly(),
            model.RangoFechas.End.ToDateOnly(),
            model.EscenarioCupo,
            [.. model.CanalesVenta],
            [.. model.DiasSemana],
            model.HoraInicio!.Value,
            model.HoraTermino!.Value,
            model.IntervaloMinutos,
            model.Total,
            model.SobreCupo,
            model.TopeEnCupo);

    public static UpdateCuposFecha ToUpdate(this CuposFechaEditModel model) =>
        new(
            model.Fecha,
            model.EscenarioCupo,
            model.CanalVenta,
            model.DiaSemana,
            model.Total,
            model.SobreCupo,
            model.TopeEnCupo);

    public static UpdateCupoFecha ToUpdate(this CupoFechaViewModel model) =>
        new(
            model.Id,
            model.Fecha,
            model.EscenarioCupo,
            model.CanalVenta,
            model.DiaSemana,
            model.HoraInicio,
            model.HoraFin,
            model.Total,
            model.SobreCupo,
            model.TopeEnCupo);

    public static DeleteCuposFecha ToDelete(this CuposFechaDeleteModel model) =>
       new(
           model.RangoFechas.Start.ToDateOnly(),
           model.RangoFechas.End.ToDateOnly(),
           model.EscenarioCupo,
           [.. model.CanalesVenta],
           [.. model.DiasSemana],
           model.HoraInicio!.Value,
           model.HoraTermino!.Value,
           model.IntervaloMinutos);

    private static DateOnly ToDateOnly(this DateTime? date) =>
        date is null ? DateOnly.FromDateTime(DateTime.Today) : DateOnly.FromDateTime(date.Value);
}
