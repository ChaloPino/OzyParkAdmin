using MudBlazor;
using OzyParkAdmin.Application.Ventas.Search;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Ventas;
using System.Diagnostics;

namespace OzyParkAdmin.Components.Admin.Ventas.Models;

internal static class VentaMappers
{
    public static SearchVentasOrden ToSearch(this GridState<VentaOrdenInfo> state, SearchVentaModel search) =>
        new(search.Fecha ?? SearchVentaModel.DateToSearch, search.NumeroOrden, search.VentaId, search.TicketId, search.Email, search.Telefono, search.Nombres, search.Apellidos, state.ToSortExpressions(), state.Page, state.PageSize);

    private static SortExpressionCollection<VentaOrdenInfo> ToSortExpressions(this GridState<VentaOrdenInfo> state)
    {
        SortExpressionCollection<VentaOrdenInfo> sortExpressions = new();

        foreach (SortDefinition<VentaOrdenInfo> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<VentaOrdenInfo> sortDefinition, SortExpressionCollection<VentaOrdenInfo> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(VentaOrdenInfo.VentaId) => sortExpressions.Add(x => x.VentaId, sortDefinition.Descending),
            nameof(VentaOrdenInfo.FechaVenta) => sortExpressions.Add(x => x.FechaVenta, sortDefinition.Descending),
            nameof(VentaOrdenInfo.Nombres) => sortExpressions.Add(x => x.Nombres, sortDefinition.Descending),
            nameof(VentaOrdenInfo.Apellidos) => sortExpressions.Add(x => x.Apellidos, sortDefinition.Descending),
            nameof(VentaOrdenInfo.Email) => sortExpressions.Add(x => x.Email, sortDefinition.Descending),
            nameof(VentaOrdenInfo.Telefono) => sortExpressions.Add(x => x.Telefono, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static GridData<VentaOrdenInfo> ToGridData(this PagedList<VentaOrdenInfo> source) =>
        new() { TotalItems = source.TotalCount, Items = source.Items };
}
