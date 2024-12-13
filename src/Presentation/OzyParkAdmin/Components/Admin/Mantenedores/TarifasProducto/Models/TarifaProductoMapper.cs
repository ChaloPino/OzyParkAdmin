using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.TarfiasProducto.Create;
using OzyParkAdmin.Application.TarfiasProducto.Search;
using OzyParkAdmin.Application.TarfiasProducto.Update;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Models;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;
using OzyParkAdmin.Shared;
using System.Diagnostics;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProductos.Models;

internal static class TarifaProductoMappers
{
    public static SearchTarifasProducto ToSearch(this GridState<TarifaProductoViewModel> state, CentroCostoInfo centroCosto, string? searchText) =>
        new(centroCosto.Id, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<TarifaProducto> ToFilterExpressions(this GridState<TarifaProductoViewModel> state)
    {
        FilterExpressionCollection<TarifaProducto> filterExpressions = new();

        foreach (IFilterDefinition<TarifaProductoViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<TarifaProductoViewModel> filterDefinition, FilterExpressionCollection<TarifaProducto> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(TarifaProductoViewModel.InicioVigencia) => filterExpressions.Add(x => x.InicioVigencia, filterDefinition.Operator!, filterDefinition.Value),
            "Moneda.Descripcion" => filterExpressions.Add(x => x.Moneda.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "Producto.Nombre" => filterExpressions.Add(x => x.Producto.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "TipoDia.Descripcion" => filterExpressions.Add(x => x.TipoDia.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "TipoHorario.Descripcion" => filterExpressions.Add(x => x.TipoHorario.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            "CanalVenta.Nombre" => filterExpressions.Add(x => x.CanalVenta.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(TarifaProductoViewModel.ValorAfecto) => filterExpressions.Add(x => x.ValorAfecto, filterDefinition.Operator!, filterDefinition.Value),
            nameof(TarifaProductoViewModel.ValorExento) => filterExpressions.Add(x => x.ValorExento, filterDefinition.Operator!, filterDefinition.Value),
            nameof(TarifaProductoViewModel.Valor) => filterExpressions.Add(x => x.Valor, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<TarifaProducto> ToSortExpressions(this GridState<TarifaProductoViewModel> state)
    {
        SortExpressionCollection<TarifaProducto> sortExpressions = state.SortDefinitions.Count == 0
            ? SortExpressionCollection<TarifaProducto>.CreateDefault(x => x.InicioVigencia, true)
            : new();

        foreach (var sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<TarifaProductoViewModel> sortDefinition, SortExpressionCollection<TarifaProducto> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(TarifaProductoViewModel.InicioVigencia) => sortExpressions.Add(x => x.InicioVigencia, sortDefinition.Descending),
            "Moneda.Nombre" => sortExpressions.Add(x => x.Moneda.Nombre, sortDefinition.Descending),
            "Producto.Nombre" => sortExpressions.Add(x => x.Producto.Nombre, sortDefinition.Descending),
            "TipoDia.Descripcion" => sortExpressions.Add(x => x.TipoDia.Descripcion, sortDefinition.Descending),
            "TipoHorario.Descripcion" => sortExpressions.Add(x => x.TipoHorario.Descripcion, sortDefinition.Descending),
            "CanalVenta.Nombre" => sortExpressions.Add(x => x.CanalVenta.Nombre, sortDefinition.Descending),
            nameof(TarifaProductoViewModel.ValorAfecto) => sortExpressions.Add(x => x.ValorAfecto, sortDefinition.Descending),
            nameof(TarifaProductoViewModel.ValorExento) => sortExpressions.Add(x => x.ValorExento, sortDefinition.Descending),
            nameof(TarifaProductoViewModel.Valor) => sortExpressions.Add(x => x.Valor, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<TarifaProductoViewModel> ToGridData(this PagedList<TarifaProductoFullInfo> source, IMudStateHasChanged mudStateHasChanged) =>
        new([.. source.Items.Select(ToViewModel)], source.TotalCount, mudStateHasChanged);

    private static TarifaProductoViewModel ToViewModel(this TarifaProductoFullInfo tarifa) =>
        new()
        {
            InicioVigencia = tarifa.InicioVigencia,
            Moneda = tarifa.Moneda,
            Producto = tarifa.Producto,
            TipoDia = tarifa.TipoDia,
            TipoHorario = tarifa.TipoHorario,
            CanalVenta = tarifa.CanalVenta,
            ValorAfecto = tarifa.ValorAfecto,
            ValorExento = tarifa.ValorExento,
        };

    public static CreateTarifasProducto ToCreate(this TarifaProductoCreateModel model) =>
        new(
            model.InicioVigenciaFinal,
            model.Moneda,
            model.Producto,
            [.. model.CanalesVenta],
            [.. model.TiposDia],
            [.. model.TiposHorario],
            model.ValorAfecto,
            model.ValorExento);

    public static UpdateTarifaProducto ToUpdate(this TarifaProductoViewModel tarifa) =>
        new(
            tarifa.InicioVigencia,
            tarifa.Moneda,
            tarifa.Producto,
            tarifa.CanalVenta,
            tarifa.TipoDia,
            tarifa.TipoHorario,
            tarifa.ValorAfecto,
            tarifa.ValorExento);
}
