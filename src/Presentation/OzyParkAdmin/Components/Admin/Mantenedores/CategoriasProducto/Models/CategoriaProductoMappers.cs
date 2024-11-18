using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.CategoriasProducto.Create;
using OzyParkAdmin.Application.CategoriasProducto.Search;
using OzyParkAdmin.Application.CategoriasProducto.Update;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
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
            nameof(CategoriaProductoViewModel.Nombre) => filterExpressions.Add(x => x.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(CategoriaProductoViewModel.Aka) => filterExpressions.Add(x => x.Aka, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.Orden) => filterExpressions.Add(x => x.Orden, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.Nivel) => filterExpressions.Add(x => x.Nivel, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.EsFinal) => filterExpressions.Add(x => x.EsFinal, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.EsActivo) => filterExpressions.Add(x => x.EsActivo, filterDefinition.Operator!, filterDefinition.Value),
            "UsuarioCreacion.UserName" => filterExpressions.Add(x => x.UsuarioCreacion.UserName, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.FechaCreacion) => filterExpressions.Add(x => x.FechaCreacion, filterDefinition.Operator!, filterDefinition.Value),
            "UsuarioModificacion.UserName" => filterExpressions.Add(x => x.UsuarioModificacion.UserName, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.UltimaModificacion) => filterExpressions.Add(x => x.UltimaModificacion, filterDefinition.Operator!, filterDefinition.Value),
            //"Padre.Nombre" => filterExpressions.Add(x => x.Padre.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.EsTop) => filterExpressions.Add(x => x.EsTop, filterDefinition.Operator!, filterDefinition.Value),
            //nameof(CategoriaProductoViewModel.PrimeroProductos) => filterExpressions.Add(x => x.PrimeroProductos, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<CategoriaProducto> ToSortExpressions(this GridState<CategoriaProductoViewModel> state)
    {
        SortExpressionCollection<CategoriaProducto> sortExpressions = new();

        //Esto para evitar el Warning
        //[WRN] The query uses a row limiting operator ('Skip'/'Take') without an 'OrderBy' operator. This may lead to unpredictable results. If the 'Distinct' operator is used after 'OrderBy',
        //then make sure to use the 'OrderBy' operator after 'Distinct' as the ordering would otherwise get erased.
        //Esto hace que vaya un ordenamiento por defecto
        if (state.SortDefinitions.Count == 0)
        {
            //Para que las categorias aparezcan en el mismo grupo ordenados por Nombre de la categoria padre
            sortExpressions.Add(x => x.Padre.Nombre, false);
        }

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
            nameof(CategoriaProductoViewModel.Aka) => sortExpressions.Add(x => x.Aka, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.Nombre) => sortExpressions.Add(x => x.Nombre, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.Orden) => sortExpressions.Add(x => x.Orden, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.Nivel) => sortExpressions.Add(x => x.Nivel, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.EsFinal) => sortExpressions.Add(x => x.EsFinal, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.EsActivo) => sortExpressions.Add(x => x.EsActivo, sortDefinition.Descending),
            "UsuarioCreacion.UserName" => sortExpressions.Add(x => x.UsuarioCreacion.UserName, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.FechaCreacion) => sortExpressions.Add(x => x.FechaCreacion, sortDefinition.Descending),
            "UsuarioModificacion.UserName" => sortExpressions.Add(x => x.UsuarioModificacion.UserName, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.UltimaModificacion) => sortExpressions.Add(x => x.UltimaModificacion, sortDefinition.Descending),
            "Padre.Nombre" => sortExpressions.Add(x => x.Padre.Nombre, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.EsTop) => sortExpressions.Add(x => x.EsTop, sortDefinition.Descending),
            nameof(CategoriaProductoViewModel.PrimeroProductos) => sortExpressions.Add(x => x.PrimeroProductos, sortDefinition.Descending),
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
        Imagen = categoria.Imagen.ToModel(),
        Nivel = categoria.Nivel,
        Nombre = categoria.Nombre,
        NombreCompleto = categoria.NombreCompleto,
        Orden = categoria.Orden,
        Padre = categoria.Padre.ToModelPadre(),
        PrimeroProductos = categoria.PrimeroProductos,
        UltimaModificacion = categoria.UltimaModificacion,
        UsuarioCreacion = categoria.UsuarioCreacion,
        UsuarioModificacion = categoria.UsuarioModificacion
    };
    public static CatalogoImagenModel ToModel(this CatalogoImagenInfo imagen)
    {
        return new() { Aka = imagen.Aka, Base64 = imagen.Base64, MimeType = imagen.MimeType, Tipo = imagen.Tipo };
    }

    public static CategoriaProductoInfo ToModelPadre(this CategoriaProductoInfo? padre)
    {
        //Para no dejar en null el Padre y tengo por defecto "Raíz"
        if (padre is null)
        {
            return new CategoriaProductoInfo
            {
                Id = 0,
                Aka = "Raíz",
                EsActivo = true,
                Nombre = "Raíz",
                NombreCompleto = "Raíz"
            };
        }
        return padre;
    }

    public static CreateCategoriaProducto ToCreate(this CategoriaProductoViewModel categoriaProducto, ClaimsPrincipal user) =>
    new(
        categoriaProducto.FranquiciaId,
        categoriaProducto.Aka,
        categoriaProducto.Nombre,
        categoriaProducto.Padre,
        categoriaProducto.EsFinal,
        categoriaProducto.Imagen.ToInfo(),
        categoriaProducto.Orden,
        categoriaProducto.EsTop,
        categoriaProducto.Nivel,
        categoriaProducto.PrimeroProductos,
        user,
        DateTime.Now,
        user,
        DateTime.Now);

    public static UpdateCategoriaProducto ToUpdate(this CategoriaProductoViewModel categoriaProducto, ClaimsPrincipal user) =>
    new(
        categoriaProducto.Id,
        categoriaProducto.FranquiciaId,
        categoriaProducto.Aka,
        categoriaProducto.Nombre,
        categoriaProducto.Padre,
        categoriaProducto.EsFinal,
        categoriaProducto.Imagen.ToInfo(),
        categoriaProducto.Orden,
        categoriaProducto.EsTop,
        categoriaProducto.Nivel,
        categoriaProducto.PrimeroProductos,
        user,
        DateTime.Now);

    private static CatalogoImagenInfo ToInfo(this CatalogoImagenModel imagen) =>
        new() { Aka = imagen.Aka, Base64 = imagen.Base64, MimeType = imagen.MimeType, Tipo = imagen.Tipo };
}
