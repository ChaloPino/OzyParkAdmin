using Dapper;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Ventas;
using OzyParkAdmin.Infrastructure.Shared;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Ventas;

/// <summary>
/// El repositorio de <see cref="Venta"/>
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class VentaRepository(OzyParkAdminContext context) : Repository<Venta>(context), IVentaRepository
{
    /// <inheritdoc/>
    public async Task<Venta?> FindByIdAsync(string ventaId, CancellationToken cancellationToken) =>
        await EntitySet.FirstOrDefaultAsync(x => x.Id == ventaId, cancellationToken);

    /// <inheritdoc/>
    public async Task<PagedList<VentaOrdenInfo>> SearchVentasOrdenAsync(DateTime fecha, string? numeroOrden, string? ventaId, string? ticketId, string? email, string? telefono, string? nombres, string? apellidos, SortExpressionCollection<VentaOrdenInfo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sortExpressions);

        DynamicParameters parameters = new();
        parameters.Add("Fecha", fecha);
        parameters.Add("NroOrden", numeroOrden);
        parameters.Add("VentaId", ventaId);
        parameters.Add("TicketId", ticketId);
        parameters.Add("Email", email);
        parameters.Add("Telefono", telefono);
        parameters.Add("Nombres", nombres);
        parameters.Add("Apellidos", apellidos);
        parameters.Add("Page", page + 1);
        parameters.Add("PageSize", pageSize);
        parameters.Add("RowsNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);

        var definition = new CommandDefinition(
            commandText: "qsp_buscaVentaEcommerce_prc",
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var reader = await Context.Database.GetDbConnection().QueryMultipleAsync(definition);


        IEnumerable<VentaOrdenInfo> ventas = await reader.ReadAsync<VentaOrdenInfo>();
        IEnumerable<TicketOrdenInfo> tickets = await reader.ReadAsync<TicketOrdenInfo>();

        int count = parameters.Get<int>("RowsNumber");

        foreach (var venta in ventas)
        {
            venta.Tickets = tickets.Where(x => x.VentaId == venta.VentaId).ToList();
        }

        var query = sortExpressions.Sort(ventas.AsQueryable());

        return new()
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = [.. query],
        };
    }
}
