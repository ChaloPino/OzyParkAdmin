using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// El repositorio de <see cref="EscenarioCupo"/>.
/// </summary>
public interface IEscenarioCupoRepository
{
    /// <summary>
    /// Busca un escenario de cupo por su id.
    /// </summary>
    Task<EscenarioCupo?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Busca varios escenarios de cupo por sus ids.
    /// </summary>
    /// <param name="ids">Los ids de escenarios de cupos a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de escenarios de cupo.</returns>
    Task<IEnumerable<EscenarioCupo>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Busca escenarios cupos por centro de costo y zona.
    /// </summary>
    Task<IEnumerable<EscenarioCupo>> FindEscenariosAsync(int centroCostoId, int? zonaId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca escenarios cupos por una lista de información completa.
    /// </summary>
    Task<IEnumerable<EscenarioCupo>> FindEscenariosAsync(IEnumerable<EscenarioCupoFullInfo> escenariosCupos, CancellationToken cancellationToken);

    /// <summary>
    /// Busca un escenario cupo por centro de costo, zona y nombre.
    /// </summary>
    Task<EscenarioCupo?> FindEscenarioAsync(int centroCostoId, int? zonaId, string nombre, CancellationToken cancellationToken);

    /// <summary>
    /// Busca escenarios cupo que coincidan con los criterios de búsqueda.
    /// </summary>
    Task<PagedList<EscenarioCupoFullInfo>> SearchAsync(
        int[]? centrosCostoId,
        int[]? zonasId,
        string? searchText,
        FilterExpressionCollection<EscenarioCupo> filterExpressions,
        SortExpressionCollection<EscenarioCupo> sortExpressions,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los escenarios de cupo.
    /// </summary>
    Task<List<EscenarioCupoInfo>> ListAsync(int[]? centroCostoIds, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene el último id del escenario cupo.
    /// </summary>
    Task<int> GetLastIdAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Agrega un nuevo escenario cupo.
    /// </summary>
    Task AddAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken);

    /// <summary>
    /// Agrega múltiples escenarios cupos.
    /// </summary>
    Task AddRangeAsync(IEnumerable<EscenarioCupo> escenariosCupos, CancellationToken cancellationToken);

    /// <summary>
    /// Agrega detalles asociados a un escenario cupo.
    /// </summary>
    Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupoInfo> detalles, CancellationToken cancellationToken);

    /// <summary>
    /// Elimina un escenario cupo.
    /// </summary>
    Task RemoveAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken);

    /// <summary>
    /// Guarda los cambios pendientes en la base de datos.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza un escenario cupo existente.
    /// </summary>
    Task UpdateAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica si existe un escenario de cupo con un nombre similar.
    /// </summary>
    /// <param name="nombre">El nombre del escenario a buscar.</param>
    /// <param name="excludeId">El ID del escenario a excluir (útil para evitar conflictos al actualizar).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si existe un escenario con el nombre dado, excluyendo el ID especificado.</returns>
    Task<bool> ExistsWithSimilarNameAsync(string nombre, int? excludeId, CancellationToken cancellationToken);


}
