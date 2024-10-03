using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// El repositorio de <see cref="Cupo"/>.
/// </summary>
public interface ICupoRepository
{
    /// <summary>
    /// Busca un cupo por su id.
    /// </summary>
    /// <param name="id">El id del cupo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El cupo si es que existe.</returns>
    Task<Cupo?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un cupo por su clave única.
    /// </summary>
    /// <param name="fechaEfectiva">La fecha efectiva a buscar.</param>
    /// <param name="escenarioCupo">El escenario de cupo a buscar.</param>
    /// <param name="canalVenta">El canal de venta a buscar.</param>
    /// <param name="diaSemana">El día de semana a buscar.</param>
    /// <param name="horaInicio">La hora de inicio a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El cupo si es que existe.</returns>
    Task<Cupo?> FindByUniqueKey(DateOnly fechaEfectiva, EscenarioCupo escenarioCupo, CanalVenta canalVenta, DiaSemana diaSemana, TimeSpan horaInicio, CancellationToken cancellationToken);

    /// <summary>
    /// Consigue el id máximo de cupos.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El id máximo de cupos.</returns>
    Task<int> MaxIdAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Busca cupos que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de <see cref="CupoFullInfo"/> que coinciden con los criterios de búsqueda.</returns>
    Task<PagedList<CupoFullInfo>> SearchAsync(int[]? centroCostoIds, string? searchText, FilterExpressionCollection<Cupo> filterExpressions, SortExpressionCollection<Cupo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Busca cupos para que se muestren en un calendario.
    /// </summary>
    /// <param name="canalVentaId">El id del canal de venta.</param>
    /// <param name="alcance">El alcance.</param>
    /// <param name="servicioId">El id del servicio.</param>
    /// <param name="zonaOrigenId">El id de la zona de origen.</param>
    /// <param name="inicio">La fecha de inicio.</param>
    /// <param name="dias">La cantidad de días.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de cupos para que se muestren en un calendario.</returns>
    Task<List<CupoFechaInfo>> SearchCuposParaCalendarioAsync(int canalVentaId, string alcance, int servicioId, int? zonaOrigenId, DateTime inicio, int dias, CancellationToken cancellationToken);
}
