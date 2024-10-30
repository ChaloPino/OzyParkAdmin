using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.CategoriasProducto.Search;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;

internal static class CategoriaProductoMappers
{
    public static SearchCategoriaProducto ToSearch(this GridState<CategoriaProductoViewModel> state, ClaimsPrincipal user, string? searchText) =>
    new(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<CategoriaProducto> ToFilterExpressions(this GridState<CategoriaProductoViewModel> state)
    {
        FilterExpressionCollection<CategoriaProducto> filterExpressions = new();

        foreach (var filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<CategoriaProductoViewModel> filterDefinition, FilterExpressionCollection<CategoriaProducto> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(ServicioViewModel.Aka) => filterExpressions.Add(x => x.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ServicioViewModel.Nombre) => filterExpressions.Add(x => x.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<CategoriaProducto> ToSortExpressions(this GridState<CategoriaProductoViewModel> state)
    {
        SortExpressionCollection<CategoriaProducto> sortExpressions = new();

        foreach (var sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<CategoriaProductoViewModel> sortDefinition, SortExpressionCollection<CategoriaProducto> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(ServicioViewModel.Aka) => sortExpressions.Add(x => x.Aka, sortDefinition.Descending),
            nameof(ServicioViewModel.Nombre) => sortExpressions.Add(x => x.Nombre, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<CategoriaProductoViewModel> ToGridData(this PagedList<CategoriaProductoFullInfo> servicios, IMudStateHasChanged mudStateHasChanged) =>
    new ObservableGridData<CategoriaProductoViewModel, int>(servicios.Items.Select(ToViewModel), servicios.TotalCount, mudStateHasChanged, x => x.Id);

    private static CategoriaProductoViewModel ToViewModel(CategoriaProductoFullInfo categoria) =>
    new()
    {
        Aka = categoria.Aka,
        CajasAsignadas = categoria.CajasAsignadas,
        CanalesVenta = categoria.CanalesVenta,
        EsActivo = categoria.EsActivo,
        EsFinal = categoria.EsFinal,
        EsTop = categoria.EsTop,
        FechaCreacion = categoria.FechaCreacion,
        FranquiciaId = categoria.FranquiciaId,
        Hijos = categoria.Hijos,
        Id = categoria.Id,
        Imagen = categoria.Imagen,
        Nivel = categoria.Nivel,
        Nombre = categoria.Nombre,
        Orden = categoria.Orden,
        Padre = categoria.Padre,
        PrimeroProductos = categoria.PrimeroProductos,
        UltimaModificacion = categoria.UltimaModificacion,
        UsuarioCreacion = categoria.UsuarioCreacion,
        UsuarioModificacion = categoria.UsuarioModificacion
    };
}
