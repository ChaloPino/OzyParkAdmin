using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.CuposFecha;

/// <summary>
/// El repositorio de <see cref="CupoFecha"/>.
/// </summary>
public interface ICupoFechaRepository
{
    /// <summary>
    /// Busca un cupo por fecha dado su id.
    /// </summary>
    /// <param name="id">El id del cupo por fecha a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El cupo por fecha si es que existe.</returns>
    Task<CupoFecha?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Busca cupos por fecha dados los ids.
    /// </summary>
    /// <param name="ids">Los ids de cupos de fecha a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Lista de cupos por fecha.</returns>
    Task<IEnumerable<CupoFecha>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un cupo por fecha dado su clave única.
    /// </summary>
    /// <param name="fecha">La fecha a buscar.</param>
    /// <param name="escenarioCupo">El escenario de cupo por fecha a buscar.</param>
    /// <param name="canalVenta">El canal de venta a buscar.</param>
    /// <param name="diaSemana">El día de semana a buscar.</param>
    /// <param name="horaInicio">La hora de inicio a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El cupo por fecha si es que existe.</returns>
    Task<CupoFecha?> FindByUniqueKeyAsync(DateOnly fecha, EscenarioCupo escenarioCupo, CanalVenta canalVenta, DiaSemana diaSemana, TimeSpan horaInicio, CancellationToken cancellationToken);

    /// <summary>
    /// Busca varios cupos por fecha dado parte de la clave única.
    /// </summary>
    /// <param name="fecha">La fecha del cupo por fecha..</param>
    /// <param name="escenarioCupo">El escenario cupo.</param>
    /// <param name="canalVenta">El canal de venta del cupo por fecha.</param>
    /// <param name="diaSemana">El día de semana del cupo por fecha.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Los cupos por fecha si es que existe.</returns>
    Task<IEnumerable<CupoFecha>> FindByUniqueKeyAsync(
        DateOnly fecha,
        EscenarioCupoInfo escenarioCupo,
        CanalVenta canalVenta,
        DiaSemana diaSemana,
        CancellationToken cancellationToken);

    /// <summary>
    /// Busca varios cupos por fecha dado sus claves únicas.
    /// </summary>
    /// <param name="uniqueKey">Las clave únicas a  buscar</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Los cupos por fecha si es que existe.</returns>
    Task<IEnumerable<CupoFecha>> FindByUniqueKeysAsync((DateOnly Fecha, EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio)[] uniqueKey, CancellationToken cancellationToken);

    /// <summary>
    /// Busca varios cupos por fecha dado sus claves únicas que se generarán con la información entregada.
    /// </summary>
    /// <param name="fechaDesde">La fecha desde.</param>
    /// <param name="fechaHasta">La fecha hasta.</param>
    /// <param name="escenarioCupo">El escenario de cupo.</param>
    /// <param name="canalesVenta">Lista de canales de venta.</param>
    /// <param name="diasSemana">Lista de días de semana.</param>
    /// <param name="horaInicio">Hora de inicio.</param>
    /// <param name="horaTermino">Hora de término.</param>
    /// <param name="intervaloMinutos">Intérvalo en minutos para calcular la hora de inicio real.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Los cupos por fecha si es que existe.</returns>
    Task<IEnumerable<CupoFecha>> FindByUniqueKeysAsync(
        DateOnly fechaDesde,
        DateOnly fechaHasta,
        EscenarioCupoInfo escenarioCupo,
        ImmutableArray<CanalVenta> canalesVenta,
        ImmutableArray<DiaSemana> diasSemana,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        int intervaloMinutos,
        CancellationToken cancellationToken);

    /// <summary>
    /// Consigue el id máximo de cupos por fecha.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El id máximo de cupos por fecha.</returns>
    Task<int> MaxIdAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Busca cupos por fecha que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de <see cref="CupoFechaFullInfo"/> que coinciden con los criterios de búsqueda.</returns>
    Task<PagedList<CupoFechaFullInfo>> SearchAsync(int[]? centroCostoIds, string? searchText, FilterExpressionCollection<CupoFecha> filterExpressions, SortExpressionCollection<CupoFecha> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
