using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Cupos.Create;
using OzyParkAdmin.Application.Cupos.Search;
using OzyParkAdmin.Application.Cupos.Update;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;

internal static class CupoMappers
{
    public static SearchCupos ToSearch(this GridState<CupoViewModel> state, ClaimsPrincipal user, string? searchText) =>
        new(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<Cupo> ToFilterExpressions(this GridState<CupoViewModel> state)
    {
        FilterExpressionCollection<Cupo> filterExpressions = new();

        foreach (IFilterDefinition<CupoViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<CupoViewModel> filterDefinition, FilterExpressionCollection<Cupo> filterExpressions) =>
        _ = filterDefinition.Column!.PropertyName switch
        {
            "EscenarioCupo.Nombre" => filterExpressions.Add(x => x.EscenarioCupo.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "CanalVenta.Nombre" => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "DiaSemana.Aka" => filterExpressions.Add(x => x.DiaSemana.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.HoraInicio) => filterExpressions.Add(x => x.HoraInicio, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.HoraFin) => filterExpressions.Add(x => x.HoraFin, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.Total) => filterExpressions.Add(x => x.Total, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.SobreCupo) => filterExpressions.Add(x => x.SobreCupo, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.TopeEnCupo) => filterExpressions.Add(x => x.TopeEnCupo, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.FechaEfectiva) => filterExpressions.Add(x => x.FechaEfectiva, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CupoViewModel.UltimaModificacion) => filterExpressions.Add(x => x.UltimaModificacion, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };

    private static SortExpressionCollection<Cupo> ToSortExpressions(this GridState<CupoViewModel> state)
    {
        SortExpressionCollection<Cupo> sortExpressions = new();

        foreach (SortDefinition<CupoViewModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<CupoViewModel> sortDefinition, SortExpressionCollection<Cupo> sortExpressions) =>
        _ = sortDefinition.SortBy switch
        {
            "EscenarioCupo.Nombre" => sortExpressions.Add(x => x.EscenarioCupo.Nombre, sortDefinition.Descending),
            "CanalVenta.Nombre" => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            "DiaSemana.Aka" => sortExpressions.Add(x => x.DiaSemana.Id, sortDefinition.Descending),
            nameof(CupoViewModel.HoraInicio) => sortExpressions.Add(x => x.HoraInicio, sortDefinition.Descending),
            nameof(CupoViewModel.HoraFin) => sortExpressions.Add(x => x.HoraFin, sortDefinition.Descending),
            nameof(CupoViewModel.Total) => sortExpressions.Add(x => x.Total, sortDefinition.Descending),
            nameof(CupoViewModel.SobreCupo) => sortExpressions.Add(x => x.SobreCupo, sortDefinition.Descending),
            nameof(CupoViewModel.TopeEnCupo) => sortExpressions.Add(x => x.TopeEnCupo, sortDefinition.Descending),
            nameof(CupoViewModel.FechaEfectiva) => sortExpressions.Add(x => x.FechaEfectiva, sortDefinition.Descending),
            nameof(CupoViewModel.UltimaModificacion) => sortExpressions.Add(x => x.UltimaModificacion, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };

    public static ObservableGridData<CupoViewModel> ToGridData(this PagedList<CupoFullInfo> source, IMudStateHasChanged stateHasChanged) =>
        new(source.Items.Select(ToViewModel), source.TotalCount, stateHasChanged);
    public static CupoViewModel ToViewModel(this CupoFullInfo cupo) =>
        new()
        {
            Id = cupo.Id,
            EscenarioCupo = cupo.EscenarioCupo,
            CanalVenta = cupo.CanalVenta,
            DiaSemana = cupo.DiaSemana,
            HoraInicio = cupo.HoraInicio,
            HoraFin = cupo.HoraFin,
            Total = cupo.Total,
            SobreCupo = cupo.SobreCupo,
            TopeEnCupo = cupo.TopeEnCupo,
            FechaEfectiva = cupo.FechaEfectiva,
            UltimaModificacion = cupo.UltimaModificacion,
        };

    public static CreateCupos ToCreate(this CuposModel model) =>
        new(
            model.FechaEfectiva,
            model.EscenarioCupo,
            [.. model.CanalesVenta],
            [.. model.DiasSemana],
            model.HoraInicio!.Value,
            model.HoraTermino!.Value,
            model.IntervaloMinutos,
            model.Total,
            model.SobreCupo,
            model.TopeEnCupo);

    public static UpdateCupo ToUpdate(this CupoViewModel model) =>
        new(
            model.Id,
            model.FechaEfectiva,
            model.EscenarioCupo,
            model.CanalVenta,
            model.DiaSemana,
            model.HoraInicio,
            model.HoraFin,
            model.Total,
            model.SobreCupo,
            model.TopeEnCupo);
}
