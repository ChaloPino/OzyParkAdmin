using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.EscenariosCupo.Create;
using OzyParkAdmin.Application.EscenariosCupo.Search;
using OzyParkAdmin.Application.EscenariosCupo.Update;
using OzyParkAdmin.Application.ExclusionesCupo.Delete;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

internal static class EscenarioCupoMappers
{
    public static SearchEscenariosCupo ToSearch(this GridState<EscenarioCupoModel> state, ClaimsPrincipal user, int[] zonasIds, string? searchText) =>
        new(user, zonasIds, searchText, state.ToFilterExpressions(), state.ToSortExpressions(), state.Page, state.PageSize);

    private static FilterExpressionCollection<EscenarioCupo> ToFilterExpressions(this GridState<EscenarioCupoModel> state)
    {
        FilterExpressionCollection<EscenarioCupo> filterExpressions = new();

        foreach (IFilterDefinition<EscenarioCupoModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<EscenarioCupoModel> filterDefinition, FilterExpressionCollection<EscenarioCupo> filterExpressions)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(EscenarioCupoModel.Id) => filterExpressions.Add(x => x.Id, filterDefinition.Operator!, filterDefinition.Value),
            nameof(EscenarioCupoModel.Nombre) => filterExpressions.Add(x => x.Nombre, filterDefinition.Operator!, filterDefinition.Value),
            nameof(EscenarioCupoModel.ZonaDescripcion) => filterExpressions.Add(x => x.Zona != null ? x.Zona.Descripcion : "Sin Zona", filterDefinition.Operator!, filterDefinition.Value),
            nameof(EscenarioCupoModel.EsHoraInicio) => filterExpressions.Add(x => x.EsHoraInicio, filterDefinition.Operator!, filterDefinition.Value),
            nameof(EscenarioCupoModel.MinutosAntes) => filterExpressions.Add(x => x.MinutosAntes, filterDefinition.Operator!, filterDefinition.Value),
            nameof(EscenarioCupoModel.EsActivo) => filterExpressions.Add(x => x.EsActivo, filterDefinition.Operator!, filterDefinition.Value),
            "CentroCosto.Descripcion" => filterExpressions.Add(x => x.CentroCosto.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static SortExpressionCollection<EscenarioCupo> ToSortExpressions(this GridState<EscenarioCupoModel> state)
    {
        SortExpressionCollection<EscenarioCupo> sortExpressions = new();

        foreach (SortDefinition<EscenarioCupoModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<EscenarioCupoModel> sortDefinition, SortExpressionCollection<EscenarioCupo> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(EscenarioCupoModel.Id) => sortExpressions.Add(x => x.Id, sortDefinition.Descending),
            nameof(EscenarioCupoModel.Nombre) => sortExpressions.Add(x => x.Nombre, sortDefinition.Descending),
            nameof(EscenarioCupoModel.ZonaDescripcion) => sortExpressions.Add(x => x.Zona != null ? x.Zona.Descripcion : "Sin Zona", sortDefinition.Descending),
            nameof(EscenarioCupoModel.EsHoraInicio) => sortExpressions.Add(x => x.EsHoraInicio, sortDefinition.Descending),
            nameof(EscenarioCupoModel.MinutosAntes) => sortExpressions.Add(x => x.MinutosAntes, sortDefinition.Descending),
            nameof(EscenarioCupoModel.EsActivo) => sortExpressions.Add(x => x.EsActivo, sortDefinition.Descending),
            "CentroCosto.Descripcion" => sortExpressions.Add(x => x.CentroCosto.Descripcion, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }


    public static ObservableGridData<EscenarioCupoModel> ToGridData(this PagedList<EscenarioCupoFullInfo> source, IMudStateHasChanged stateHasChanged) =>
        new([.. source.Items.Select(ToModel)], source.TotalCount, stateHasChanged);

    private static EscenarioCupoModel ToModel(this EscenarioCupoFullInfo escenarioCupo) =>
        new()
        {
            CentroCosto = escenarioCupo.CentroCosto,
            Zona = escenarioCupo.Zona,
            Id = escenarioCupo.Id,
            EsHoraInicio = escenarioCupo.EsHoraInicio,
            MinutosAntes = escenarioCupo.MinutosAntes,
            Nombre = escenarioCupo.Nombre,
            EsActivo = escenarioCupo.EsActivo,
            Detalles = escenarioCupo.Detalles.ToList()
        };

    public static CreateEscenarioCupo ToCreate(this EscenarioCupoModel model) =>
        new(model.CentroCosto, model.Zona, model.Detalles, model.Nombre, model.EsHoraInicio, model.MinutosAntes, model.EsActivo);

    public static DeleteEscenarioCupo ToDelete(this IEnumerable<EscenarioCupoModel> source) =>
        new([.. source.Select(ToInfo)]);

    public static UpdateEscenarioCupo ToUpdate(this EscenarioCupoModel model) =>
       new(
           model.Id,
           new CentroCostoInfo { Id = model.CentroCosto.Id, Descripcion = model.CentroCosto.Descripcion },
           model.Zona is not null ? new ZonaInfo { Id = model.Zona.Id, Descripcion = model.Zona.Descripcion } : null,
           model.Detalles,
           model.Nombre,
           model.EsHoraInicio,
           model.MinutosAntes,
           model.EsActivo);

    private static EscenarioCupoFullInfo ToInfo(this EscenarioCupoModel escenarioCupo) =>
        new()
        {
            CentroCosto = escenarioCupo.CentroCosto,
            Zona = escenarioCupo.Zona,
            Id = escenarioCupo.Id,
            EsHoraInicio = escenarioCupo.EsHoraInicio,
            MinutosAntes = escenarioCupo.MinutosAntes,
            Nombre = escenarioCupo.Nombre,
            EsActivo = escenarioCupo.EsActivo,
        };
}
