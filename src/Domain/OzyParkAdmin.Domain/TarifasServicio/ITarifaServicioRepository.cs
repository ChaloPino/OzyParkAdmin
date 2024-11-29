using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.TarifasServicio;

/// <summary>
/// El repositorio de <see cref="TarifaServicio"/>.
/// </summary>
public interface ITarifaServicioRepository
{
    /// <summary>
    /// Consigue una tarifa de servicio por su clave primaria.
    /// </summary>
    /// <param name="inicioVigencia">El inicio de vigencia del tramo.</param>
    /// <param name="monedaId">El id de la moneda.</param>
    /// <param name="servicioId">El id del servicio.</param>
    /// <param name="tramoId">El id del tramo.</param>
    /// <param name="grupoEtarioId">El id del grupo etario.</param>
    /// <param name="canalVentaId">El id del canal de venta.</param>
    /// <param name="tipoDiaId">El id del tipo de día.</param>
    /// <param name="tipoHorarioId">El id del tipo de horario.</param>
    /// <param name="tipoSegmentacionId">El id del tipo de segmentación.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La tarifa de servicio si existe.</returns>
    Task<TarifaServicio?> FindByPrimaryKeyAsync(DateTime inicioVigencia, int monedaId, int servicioId, int tramoId, int grupoEtarioId, int canalVentaId, int tipoDiaId, int tipoHorarioId, int tipoSegmentacionId, CancellationToken cancellationToken);

    /// <summary>
    /// Consigue todas las tarifas que coincidan con las claves primarias.
    /// </summary>
    /// <param name="primaryKeys">Las claves primarias a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de tarifas que coinciden con las claves primarias.</returns>
    Task<IEnumerable<(DateTime InicioVigencia, Moneda Moneda, Servicio Servicio, Tramo Tramo, GrupoEtario GrupoEtario, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario, TipoSegmentacion TipoSegmentacion)>> FindByPrimaryKeysAsync(List<(DateTime InicioVigencia, Moneda Moneda, Servicio Servicio, Tramo Tramo, GrupoEtario GrupoEtario, CanalVenta CanalVenta, TipoDia TipoDia, TipoHorario TipoHorario, TipoSegmentacion TipoSegmentacion)> primaryKeys, CancellationToken cancellationToken);

    /// <summary>
    /// Busca tarifas de servicio que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centroCostoId">El id del centro de costo al que pertenecen los servicios.</param>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de tarifas de servicio.</returns>
    Task<PagedList<TarifaServicio>> SearchTarifasServiciosAsync(int centroCostoId, string? searchText, FilterExpressionCollection<TarifaServicio> filterExpressions, SortExpressionCollection<TarifaServicio> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
