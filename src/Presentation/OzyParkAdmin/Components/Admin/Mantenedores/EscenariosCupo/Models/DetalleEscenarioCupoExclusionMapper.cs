using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.DetalleEscenarioExclusion.Search;
using OzyParkAdmin.Application.DetalleEscenarioExclusion.Update;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

internal static class DetalleEscenarioCupoExclusionMapper
{
    public static SearchEscenarioCupoExclusion ToSearch(
     this GridState<DetalleEscenarioCupoExclusionModel> state,
     int escenarioCupoId,
     int[] serviciosIds,
     int[] canalesDeVentaIds,
     int[] diasSemanaIds,
     string? searchText)
    {
        return new(
            ServiciosIds: serviciosIds,
            CanalesDeVentaIds: canalesDeVentaIds,
            DiasDeSemanaIds: diasSemanaIds,
            EscenarioId: escenarioCupoId,
            SearchText: searchText,
            FilterExpressions: state.ToFilterExpressions(),
            SortExpressions: state.ToSortExpressions(),
            Page: state.Page,
            PageSize: state.PageSize
        );
    }

    public static ObservableGridData<DetalleEscenarioCupoExclusionModel> ToGridData(this PagedList<DetalleEscenarioCupoExclusionFullInfo> source, IMudStateHasChanged stateHasChanged) =>
       new(source.Items.Select(ToModel), source.TotalCount, stateHasChanged);

    private static FilterExpressionCollection<DetalleEscenarioCupoExclusion> ToFilterExpressions(this GridState<DetalleEscenarioCupoExclusionModel> state)
    {
        FilterExpressionCollection<DetalleEscenarioCupoExclusion> filterExpressions = new();

        foreach (IFilterDefinition<DetalleEscenarioCupoExclusionModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<DetalleEscenarioCupoExclusionModel> filterDefinition, FilterExpressionCollection<DetalleEscenarioCupoExclusion> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(DetalleEscenarioCupoExclusionModel.ServicioNombre) => filterExpressions.Add(x => x.Servicio.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(DetalleEscenarioCupoExclusionModel.CanalVentaNombre) => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(DetalleEscenarioCupoExclusionModel.DiaSemanaNombre) => filterExpressions.Add(x => x.DiaSemana.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(DetalleEscenarioCupoExclusionModel.HoraInicio) => filterExpressions.Add(x => x.HoraInicio, filterDefinition.Operator!, filterDefinition.Value),
            nameof(DetalleEscenarioCupoExclusionModel.HoraFin) => filterExpressions.Add(x => x.HoraFin, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<DetalleEscenarioCupoExclusion> ToSortExpressions(this GridState<DetalleEscenarioCupoExclusionModel> state)
    {
        SortExpressionCollection<DetalleEscenarioCupoExclusion> sortExpressions = new();

        foreach (SortDefinition<DetalleEscenarioCupoExclusionModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<DetalleEscenarioCupoExclusionModel> sortDefinition, SortExpressionCollection<DetalleEscenarioCupoExclusion> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(DetalleEscenarioCupoExclusionModel.ServicioNombre) => sortExpressions.Add(x => x.Servicio.Nombre, sortDefinition.Descending),
            nameof(DetalleEscenarioCupoExclusionModel.CanalVentaNombre) => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            nameof(DetalleEscenarioCupoExclusionModel.DiaSemanaNombre) => sortExpressions.Add(x => x.DiaSemana.Aka, sortDefinition.Descending),
            nameof(DetalleEscenarioCupoExclusionModel.HoraInicio) => sortExpressions.Add(x => x.HoraInicio, sortDefinition.Descending),
            nameof(DetalleEscenarioCupoExclusionModel.HoraFin) => sortExpressions.Add(x => x.HoraFin, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    private static DetalleEscenarioCupoExclusionModel ToModel(this DetalleEscenarioCupoExclusionFullInfo ex) =>
      new()
      {
          CanalVentaId = ex.CanalVentaId,
          CanalVentaNombre = ex.CanalVentaNombre,
          DiaSemanaId = ex.DiaSemanaId,
          DiaSemanaNombre = ex.DiaSemanaNombre,
          EscenarioCupoId = ex.EscenarioCupoId,
          ServicioId = ex.ServicioId,
          ServicioNombre = ex.ServicioNombre,
          HoraInicio = ex.HoraInicio!.Value,
          HoraFin = ex.HoraFin

      };

    public static UpdateDetalleEscenarioExclusion ToUpdateExclusions(this EscenarioCupoModel model) =>
      new(
            escenarioCupoId: model.Id,
            exclusiones: model.Exclusiones
          );
}
