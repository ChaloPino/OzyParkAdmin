using OzyParkAdmin.Domain.Reportes.Charts;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// El repositorio de <see cref="Report"/>.
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Busca el único reporte de tipo dashboard que esté publicado.
    /// </summary>
    /// <param name="user">El usuario que realiza la consulta.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de buscar el reporte.</returns>
    Task<ResultOf<ChartReport>> FindDashboardAsync(ClaimsPrincipal user, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un reporte por su aka.
    /// </summary>
    /// <param name="aka">El aka del reporte a buscar.</param>
    /// <param name="roles">Los roles para revisar si es que tiene acceso o no.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de buscar el reporte.</returns>
    Task<ResultOf<Report>> FindReportByAkaAsync(string aka, string[] roles, CancellationToken cancellationToken);

    /// <summary>
    /// Busca todas las agrupaciones de reportes.
    /// </summary>
    /// <param name="roles">Los roles que se revisarán para ver si se puede conseguir la lista de reportes.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Todas la agrupaciones de reportes.</returns>
    Task<List<ReportGroupInfo>> FindReportGroupsAsync(string[] roles, CancellationToken cancellationToken = default);

    /// <summary>
    /// Carga todos los datos del filtro de tipo lista.
    /// </summary>
    /// <param name="reportId">El id del reporte al que pertenece el filtro tipo lista.</param>
    /// <param name="filterId">El id del filtro tipo lista.</param>
    /// <param name="parameters">La lista de parámetros para ejecutar la carga.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La información cargada del filtro tipo lista.</returns>
    Task<List<ItemOption>> LoadFilterAsync(Guid reportId, int filterId, string?[] parameters, CancellationToken cancellationToken);

}
