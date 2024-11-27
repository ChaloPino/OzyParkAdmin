using Microsoft.CodeAnalysis.CSharp.Syntax;
using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.TarfiasServicio.Create;
using OzyParkAdmin.Application.TarfiasServicio.Search;
using OzyParkAdmin.Application.TarfiasServicio.Update;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasServicio;
using OzyParkAdmin.Shared;
using System.Diagnostics;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;

internal static class TarifaServicioMappers
{
    public static SearchTarifasServicio ToSearch(this GridState<TarifaServicioViewModel> state, CentroCostoInfo centroCosto, string? searchText) =>
        new(centroCosto.Id, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<TarifaServicio> ToFilterExpressions(this GridState<TarifaServicioViewModel> state)
    {
        FilterExpressionCollection<TarifaServicio> filterExpressions = new();

        foreach (IFilterDefinition<TarifaServicioViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<TarifaServicioViewModel> filterDefinition, FilterExpressionCollection<TarifaServicio> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(TarifaServicioViewModel.InicioVigencia) => filterExpressions.Add(x => x.InicioVigencia, filterDefinition.Operator!, filterDefinition.Value),
            "Moneda.Descripcion" => filterExpressions.Add(x => x.Moneda.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "Servicio.Nombre" => filterExpressions.Add(x => x.Servicio.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "Tramo.Descripcion" => filterExpressions.Add(x => x.Tramo.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "GrupoEtario.Descripcion" => filterExpressions.Add(x => x.GrupoEtario.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "TipoDia.Descripcion" => filterExpressions.Add(x => x.TipoDia.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "TipoHorario.Descripcion" => filterExpressions.Add(x => x.TipoHorario.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "CanalVenta.Nombre" => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "TipoSegmentacion.Descripcion" => filterExpressions.Add(x => x.TipoSegmentacion.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            nameof(TarifaServicioViewModel.ValorAfecto) => filterExpressions.Add(x => x.ValorAfecto, filterDefinition.Operator!, filterDefinition.Value),
            nameof(TarifaServicioViewModel.ValorExento) => filterExpressions.Add(x => x.ValorExento, filterDefinition.Operator!, filterDefinition.Value),
            nameof(TarifaServicioViewModel.Valor) => filterExpressions.Add(x => x.Valor, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<TarifaServicio> ToSortExpressions(this GridState<TarifaServicioViewModel> state)
    {
        SortExpressionCollection<TarifaServicio> sortExpressions = state.SortDefinitions.Count == 0
            ? SortExpressionCollection<TarifaServicio>.CreateDefault(x => x.InicioVigencia, true)
            : new();

        foreach (var sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<TarifaServicioViewModel> sortDefinition, SortExpressionCollection<TarifaServicio> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(TarifaServicioViewModel.InicioVigencia) => sortExpressions.Add(x => x.InicioVigencia, sortDefinition.Descending),
            "Moneda.Nombre" => sortExpressions.Add(x => x.Moneda.Nombre, sortDefinition.Descending),
            "Servicio.Nombre" => sortExpressions.Add(x => x.Servicio.Nombre, sortDefinition.Descending),
            "Tramo.Descripcion" => sortExpressions.Add(x => x.Tramo.Descripcion, sortDefinition.Descending),
            "GrupoEtario.Descripcion" => sortExpressions.Add(x => x.GrupoEtario.Descripcion, sortDefinition.Descending),
            "TipoDia.Descripcion" => sortExpressions.Add(x => x.TipoDia.Descripcion, sortDefinition.Descending),
            "TipoHorario.Descripcion" => sortExpressions.Add(x => x.TipoHorario.Descripcion, sortDefinition.Descending),
            "CanalVenta.Nombre" => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            "TipoSegmentacion.Descripcion" => sortExpressions.Add(x => x.TipoSegmentacion.Descripcion, sortDefinition.Descending),
            nameof(TarifaServicioViewModel.ValorAfecto) => sortExpressions.Add(x => x.ValorAfecto, sortDefinition.Descending),
            nameof(TarifaServicioViewModel.ValorExento) => sortExpressions.Add(x => x.ValorExento, sortDefinition.Descending),
            nameof(TarifaServicioViewModel.Valor) => sortExpressions.Add(x => x.Valor, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<TarifaServicioViewModel> ToGridData(this PagedList<TarifaServicioFullInfo> source, IMudStateHasChanged mudStateHasChanged) =>
        new([.. source.Items.Select(ToViewModel)], source.TotalCount, mudStateHasChanged);

    private static TarifaServicioViewModel ToViewModel(this TarifaServicioFullInfo tarifa) =>
        new()
        {
            InicioVigencia = tarifa.InicioVigencia,
            Moneda = tarifa.Moneda,
            Servicio = tarifa.Servicio,
            Tramo = tarifa.Tramo,
            GrupoEtario = tarifa.GrupoEtario,
            TipoDia = tarifa.TipoDia,
            TipoHorario = tarifa.TipoHorario,
            CanalVenta = tarifa.CanalVenta,
            TipoSegmenetacion = tarifa.TipoSegmentacion,
            ValorAfecto = tarifa.ValorAfecto,
            ValorExento = tarifa.ValorExento,
        };

    public static CreateTarifasServicio ToCreate(this TarifaServicioCreateModel model) =>
        new(
            model.InicioVigenciaFinal,
            model.Moneda,
            model.Servicio,
            [.. model.Tramos],
            [.. model.GruposEtarios],
            [.. model.CanalesVenta],
            [.. model.TiposDia],
            [.. model.TiposHorario],
            [.. model.TiposSegmentacion],
            model.ValorAfecto,
            model.ValorExento);

    public static UpdateTarifaServicio ToUpdate(this TarifaServicioViewModel tarifa) =>
        new(
            tarifa.InicioVigencia,
            tarifa.Moneda,
            tarifa.Servicio,
            tarifa.Tramo,
            tarifa.GrupoEtario,
            tarifa.CanalVenta,
            tarifa.TipoDia,
            tarifa.TipoHorario,
            tarifa.TipoSegmenetacion,
            tarifa.ValorAfecto,
            tarifa.ValorExento);
}
