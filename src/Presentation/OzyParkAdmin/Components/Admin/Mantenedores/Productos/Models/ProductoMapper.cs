using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Productos.Create;
using OzyParkAdmin.Application.Productos.Search;
using OzyParkAdmin.Application.Productos.Update;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

internal static class ProductoMapper
{
    public static SearchProductos ToSearch(this GridState<ProductoViewModel> state, ClaimsPrincipal user, string? searchText) =>
        new(user, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<Producto> ToFilterExpressions(this GridState<ProductoViewModel> state)
    {
        FilterExpressionCollection<Producto> filterExpressions = new();

        foreach (var filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<ProductoViewModel> filterDefinition, FilterExpressionCollection<Producto> filterExpressions)
    {
        _ = filterDefinition.Column?.PropertyName switch
        {
            nameof(ProductoViewModel.Sku) => filterExpressions.Add(x => x.Sku, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.Aka) => filterExpressions.Add(x => x.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.Nombre) => filterExpressions.Add(x => x.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "TipoProducto.Nombre" => filterExpressions.Add(x => x.TipoProducto.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "TipoProducto.EsParaVenta" => filterExpressions.Add(x => x.TipoProducto.EsParaVenta, filterDefinition.Operator!, filterDefinition.Value),
            "TipoProducto.ControlaStock" => filterExpressions.Add(x => x.TipoProducto.ControlaStock, filterDefinition.Operator!, filterDefinition.Value),
            "TipoProducto.ControlaInventario" => filterExpressions.Add(x => x.TipoProducto.ControlaInventario, filterDefinition.Operator!, filterDefinition.Value),
            "Categoria.Nombre" => filterExpressions.Add(x => x.Categoria.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            "Familia.Aka" => filterExpressions.Add(x => x.Familia.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.FechaAlta) => filterExpressions.Add(x => x.FechaAlta, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.EsComplemento) => filterExpressions.Add(x => x.EsComplemento, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.Orden) => filterExpressions.Add(x => x.Orden, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.EsActivo) => filterExpressions.Add(x => x.EsActivo, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.EnInventario) => filterExpressions.Add(x => x.EnInventario, filterDefinition.Operator!, filterDefinition.Value),
            "UsuarioCreacion.FriendlyName" => filterExpressions.Add(x => x.UsuarioCreacion.FriendlyName, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.FechaSistema) => filterExpressions.Add(x => x.FechaSistema, filterDefinition.Operator!, filterDefinition.Value),
            "UsuarioModificacion.FriendlyName" => filterExpressions.Add(x => x.UsuarioModificacion.FriendlyName, filterDefinition.Operator!, filterDefinition.Value),
            nameof(ProductoViewModel.UltimaModificacion) => filterExpressions.Add(x => x.UltimaModificacion, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<Producto> ToSortExpressions(this GridState<ProductoViewModel> state)
    {
        //se ordena por defecto por x.Categoria.Nombre dado que se agrupa por el nombre de la categoria,
        //sino aparece algunas categorias con menos productos a los que realmente tiene
        SortExpressionCollection<Producto> sortExpressions = state.SortDefinitions.Count == 0
             ? SortExpressionCollection<Producto>.CreateDefault(x => x.Categoria.Nombre, false)
             : new SortExpressionCollection<Producto>();

        foreach (var sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<ProductoViewModel> sortDefinition, SortExpressionCollection<Producto> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(ProductoViewModel.Sku) => sortExpressions.Add(x => x.Sku, sortDefinition.Descending),
            nameof(ProductoViewModel.Aka) => sortExpressions.Add(x => x.Aka, sortDefinition.Descending),
            nameof(ProductoViewModel.Nombre) => sortExpressions.Add(x => x.Nombre, sortDefinition.Descending),
            "TipoProducto.Nombre" => sortExpressions.Add(x => x.TipoProducto.Nombre, sortDefinition.Descending),
            "TipoProducto.EsParaVenta" => sortExpressions.Add(x => x.TipoProducto.EsParaVenta, sortDefinition.Descending),
            "TipoProducto.ControlaStock" => sortExpressions.Add(x => x.TipoProducto.ControlaStock, sortDefinition.Descending),
            "TipoProducto.ControlaInventario" => sortExpressions.Add(x => x.TipoProducto.ControlaInventario, sortDefinition.Descending),
            "Categoria.Nombre" => sortExpressions.Add(x => x.Categoria.Nombre, sortDefinition.Descending),
            "Familia.Aka" => sortExpressions.Add(x => x.Familia.Aka, sortDefinition.Descending),
            nameof(ProductoViewModel.FechaAlta) => sortExpressions.Add(x => x.FechaAlta, sortDefinition.Descending),
            nameof(ProductoViewModel.EsComplemento) => sortExpressions.Add(x => x.EsComplemento, sortDefinition.Descending),
            nameof(ProductoViewModel.Orden) => sortExpressions.Add(x => x.Orden, sortDefinition.Descending),
            nameof(ProductoViewModel.EsActivo) => sortExpressions.Add(x => x.EsActivo, sortDefinition.Descending),
            nameof(ProductoViewModel.EnInventario) => sortExpressions.Add(x => x.EnInventario, sortDefinition.Descending),
            "UsuarioCreacion.FriendlyName" => sortExpressions.Add(x => x.UsuarioCreacion.FriendlyName, sortDefinition.Descending),
            nameof(ProductoViewModel.FechaSistema) => sortExpressions.Add(x => x.FechaSistema, sortDefinition.Descending),
            "UsuarioModificacion.FriendlyName" => sortExpressions.Add(x => x.UsuarioModificacion.FriendlyName, sortDefinition.Descending),
            nameof(ProductoViewModel.UltimaModificacion) => sortExpressions.Add(x => x.UltimaModificacion, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<ProductoViewModel> ToGridData(this PagedList<ProductoFullInfo> source, IMudStateHasChanged stateHasChanged) =>
        new(source.Items.Select(ToViewModel), source.TotalCount, stateHasChanged);

    private static ProductoViewModel ToViewModel(ProductoFullInfo producto) =>
        new()
        {
            Id = producto.Id,
            Aka = producto.Aka,
            Sku = producto.Sku,
            Nombre = producto.Nombre,
            FranquiciaId = producto.FranquiciaId,
            CentroCosto = producto.CentroCosto,
            Categoria = producto.Categoria,
            CategoriaDespliegue = producto.CategoriaDespliegue,
            Imagen = producto.Imagen.ToModel(),
            TipoProducto = producto.TipoProducto,
            Orden = producto.Orden,
            Familia = producto.Familia,
            EsComplemento = producto.EsComplemento,
            EnInventario = producto.EnInventario,
            FechaAlta = producto.FechaAlta,
            UsuarioCreacion = producto.UsuarioCreacion,
            FechaSistema = producto.FechaSistema,
            UsuarioModificacion = producto.UsuarioModificacion,
            UltimaModificacion = producto.UltimaModificacion,
            EsActivo = producto.EsActivo,
            Complementos = producto.Complementos.ToModel(),
        };

    public static CatalogoImagenModel ToModel(this CatalogoImagenInfo imagen) =>
        new() { Aka = imagen.Aka, Base64 = imagen.Base64, MimeType = imagen.MimeType, Tipo = imagen.Tipo };

    public static List<ProductoComplementarioModel> ToModel(this IEnumerable<ProductoComplementarioInfo> source) =>
        [.. source.Select(ToModel)];

    private static ProductoComplementarioModel ToModel(ProductoComplementarioInfo complementario) =>
        new() { Complemento = complementario.Complemento, Orden = complementario.Orden };

    public static List<ProductoRelacionadoModel> ToModel(this IEnumerable<ProductoRelacionadoInfo> source) =>
        [.. source.Select(ToModel)];

    private static ProductoRelacionadoModel ToModel(ProductoRelacionadoInfo relacionado) =>
        new() { Relacionado = relacionado.Relacionado, Orden = relacionado.Orden };

    public static List<ProductoParteModel> ToModel(this IEnumerable<ProductoParteInfo> source) =>
        [.. source.Select(ToModel)];

    private static ProductoParteModel ToModel(ProductoParteInfo parte) =>
        new() { Parte = parte.Parte, Cantidad = parte.Cantidad, EsOpcional = parte.EsOpcional };

    public static CreateProducto ToCreate(this ProductoViewModel producto, ClaimsPrincipal user) =>
        new(
            producto.Aka,
            producto.Sku,
            producto.Nombre,
            producto.FranquiciaId,
            producto.CentroCosto,
            producto.Categoria,
            producto.CategoriaDespliegue,
            producto.Imagen.ToInfo(),
            producto.TipoProducto,
            producto.Orden,
            producto.Familia,
            producto.EsComplemento,
            producto.FechaAlta,
            user,
            producto.Complementos.ToInfo());

    public static UpdateProducto ToUpdate(this ProductoViewModel producto, ClaimsPrincipal user) =>
        new(
            producto.Id,
            producto.Aka,
            producto.Sku,
            producto.Nombre,
            producto.FranquiciaId,
            producto.CentroCosto,
            producto.Categoria,
            producto.CategoriaDespliegue,
            producto.Imagen.ToInfo(),
            producto.TipoProducto,
            producto.Orden,
            producto.Familia,
            producto.EsComplemento,
            producto.FechaAlta,
            user,
            producto.Complementos.ToInfo());

    private static CatalogoImagenInfo ToInfo(this CatalogoImagenModel imagen) =>
        new() {  Aka = imagen.Aka, Base64 = imagen.Base64, MimeType = imagen.MimeType, Tipo = imagen.Tipo };

    private static ImmutableArray<ProductoComplementarioInfo> ToInfo(this IEnumerable<ProductoComplementarioModel> source) =>
        [.. source.Select(ToInfo)];

    private static ProductoComplementarioInfo ToInfo(ProductoComplementarioModel complementario) =>
        new() { Complemento = complementario.Complemento, Orden = complementario.Orden };

    public static ImmutableArray<ProductoParteInfo> ToInfo(this IEnumerable<ProductoParteModel> source) =>
        [.. source.Select(ToInfo)];

    private static ProductoParteInfo ToInfo(ProductoParteModel productoParte) =>
        new() {  Parte = productoParte.Parte, Cantidad = productoParte.Cantidad, EsOpcional = productoParte.EsOpcional };
}
