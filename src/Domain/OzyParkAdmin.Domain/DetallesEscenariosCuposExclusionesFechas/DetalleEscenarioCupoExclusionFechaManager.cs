using OzyParkAdmin.Domain.Repositories;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

/// <summary>
/// Administrador de la lógica de negocio para la entidad <see cref="DetalleEscenarioCupoExclusionFecha"/>.
/// </summary>
public sealed class DetalleEscenarioCupoExclusionFechaManager : IBusinessLogic
{
    private readonly IDetalleEscenarioCupoExclusionFechaRepository _detalleExclusionRepository;

    public DetalleEscenarioCupoExclusionFechaManager(IDetalleEscenarioCupoExclusionFechaRepository detalleExclusionRepository)
    {
        ArgumentNullException.ThrowIfNull(detalleExclusionRepository);
        _detalleExclusionRepository = detalleExclusionRepository;
    }

    /// <summary>
    /// Sincroniza las exclusiones con la lista proporcionada. Si hay duplicados, se actualizan.
    /// </summary>
    public async Task<(IEnumerable<DetalleEscenarioCupoExclusionFecha> nuevas, IEnumerable<DetalleEscenarioCupoExclusionFecha> actualizar, IEnumerable<DetalleEscenarioCupoExclusionFecha> eliminar)>
    SyncExclusionesAsync(
    int escenarioCupoId,
    IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> nuevasExclusionesInfo,
    CancellationToken cancellationToken)
    {
        var exclusionesExistentes = await _detalleExclusionRepository.GetSimpleExclusionesByEscenarioCupoIdAsync(escenarioCupoId, cancellationToken)
                                    ?? Enumerable.Empty<DetalleEscenarioCupoExclusionFecha>();

        var nuevasExclusiones = nuevasExclusionesInfo?.Select(x => DetalleEscenarioCupoExclusionFecha.Create(
            escenarioCupoId: escenarioCupoId,
            servicioId: x.ServicioId,
            canalVentaId: x.CanalVentaId,
            fechaExclusion: x.FechaExclusion!.Value,
            horaInicio: x.HoraInicio,
            horaFin: x.HoraFin)).ToList() ?? new List<DetalleEscenarioCupoExclusionFecha>();

        var exclusionesParaActualizar = exclusionesExistentes
            .Where(e => nuevasExclusiones.Any(n => n.ServicioId == e.ServicioId && n.FechaExclusion == e.FechaExclusion && n.CanalVentaId == e.CanalVentaId))
            .ToList();

        var exclusionesParaEliminar = exclusionesExistentes
            .Where(e => !nuevasExclusiones.Any(n => n.ServicioId == e.ServicioId && n.FechaExclusion == e.FechaExclusion && n.CanalVentaId == e.CanalVentaId))
            .ToList();

        var exclusionesParaAgregar = nuevasExclusiones
            .Where(n => !exclusionesExistentes.Any(e => e.ServicioId == n.ServicioId && e.FechaExclusion == n.FechaExclusion && e.CanalVentaId == n.CanalVentaId))
            .ToList();

        foreach (var exclusion in exclusionesParaActualizar)
        {
            var nuevaExclusion = nuevasExclusiones.FirstOrDefault(n => n.ServicioId == exclusion.ServicioId && n.FechaExclusion == exclusion.FechaExclusion && n.CanalVentaId == exclusion.CanalVentaId);
            if (nuevaExclusion != null)
            {
                exclusion.Update(nuevaExclusion);
            }
        }

        return (exclusionesParaAgregar, exclusionesParaActualizar, exclusionesParaEliminar);
    }
}
