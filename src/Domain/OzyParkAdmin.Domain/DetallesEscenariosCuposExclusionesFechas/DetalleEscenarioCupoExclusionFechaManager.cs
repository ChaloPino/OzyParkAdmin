using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Repositories;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

/// <summary>
/// Administrador de la lógica de negocio para la entidad <see cref="DetalleEscenarioCupoExclusionFecha"/>.
/// </summary>
public sealed class DetalleEscenarioCupoExclusionFechaManager : IBusinessLogic
{
    private readonly IDetalleEscenarioCupoExclusionFechaRepository _detalleExclusionRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;

    public DetalleEscenarioCupoExclusionFechaManager(
        IDetalleEscenarioCupoExclusionFechaRepository detalleExclusionRepository,
        IEscenarioCupoRepository escenarioCupoRepository)
    {
        ArgumentNullException.ThrowIfNull(detalleExclusionRepository);
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);

        _detalleExclusionRepository = detalleExclusionRepository;
        _escenarioCupoRepository = escenarioCupoRepository;
    }

    /// <summary>
    /// Actualiza múltiples exclusiones para un escenario de cupo.
    /// </summary>
    public async Task<ResultOf<IEnumerable<DetalleEscenarioCupoExclusionFecha>>> UpdateExclusionesAsync(
        int escenarioCupoId,
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> nuevasExclusionesInfo,
        CancellationToken cancellationToken)
    {
        var exclusionesExistentes = await _detalleExclusionRepository.GetExclusionesByEscenarioCupoIdAsync(escenarioCupoId, cancellationToken);
        var nuevasExclusiones = nuevasExclusionesInfo.Select(d => DetalleEscenarioCupoExclusionFecha.Create(
            escenarioCupoId,
            d.ServicioId,
            d.CanalVentaId,
            d.FechaExclusion!,
            d.HoraInicio,
            d.HoraFin)).ToList();

        var errors = new List<ValidationError>();
        foreach (var exclusion in nuevasExclusiones)
        {
            await ValidateDuplicityAsync((exclusion.EscenarioCupoId, exclusion.ServicioId, exclusion.CanalVentaId, exclusion.FechaExclusion), errors, cancellationToken);
        }

        if (errors.Any())
        {
            return errors;
        }

        foreach (var exclusion in exclusionesExistentes)
        {
            var exclusionActualizada = nuevasExclusiones.FirstOrDefault(nd => nd.ServicioId == exclusion.ServicioId && nd.CanalVentaId == exclusion.CanalVentaId && nd.FechaExclusion == exclusion.FechaExclusion);
            if (exclusionActualizada != null)
            {
                exclusion.Update(exclusionActualizada);
            }
        }

        return nuevasExclusiones;
    }

    /// <summary>
    /// Crea múltiples exclusiones para un escenario de cupo.
    /// </summary>
    public async Task<ResultOf<IEnumerable<DetalleEscenarioCupoExclusionFecha>>> CreateExclusionesAsync(
        int escenarioCupoId,
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusionesInfo,
        CancellationToken cancellationToken)
    {
        var exclusiones = exclusionesInfo.Select(d => DetalleEscenarioCupoExclusionFecha.Create(
            escenarioCupoId,
            d.ServicioId,
            d.CanalVentaId,
            d.FechaExclusion,
            d.HoraInicio,
            d.HoraFin)).ToList();

        var errors = new List<ValidationError>();

        foreach (var exclusion in exclusiones)
        {
            await ValidateDuplicityAsync((exclusion.EscenarioCupoId, exclusion.ServicioId, exclusion.CanalVentaId, exclusion.FechaExclusion), errors, cancellationToken);
        }

        if (errors.Any())
        {
            return errors;
        }

        return exclusiones;
    }

    /// <summary>
    /// Valida duplicidad al actualizar o crear exclusiones.
    /// </summary>
    private async Task ValidateDuplicityAsync(
        (int EscenarioCupoId, int ServicioId, int CanalVentaId, DateTime FechaExclusion) exclusionToValidate,
        IList<ValidationError> errors,
        CancellationToken cancellationToken)
    {
        var exclusiones = await _detalleExclusionRepository.GetExclusionesByEscenarioCupoIdAsync(exclusionToValidate.EscenarioCupoId, cancellationToken);

        var existente = exclusiones.FirstOrDefault(x =>
            x.ServicioId == exclusionToValidate.ServicioId &&
            x.CanalVentaId == exclusionToValidate.CanalVentaId &&
            x.FechaExclusion.Date == exclusionToValidate.FechaExclusion.Date);

        if (existente is not null && existente.EscenarioCupoId != exclusionToValidate.EscenarioCupoId)
        {
            errors.Add(new ValidationError(
                nameof(DetalleEscenarioCupoExclusionFecha),
                $"La exclusión con ServicioId {exclusionToValidate.ServicioId}, CanalVentaId {exclusionToValidate.CanalVentaId}, y FechaExclusion {exclusionToValidate.FechaExclusion.ToShortDateString()} ya existe para el EscenarioCupoId {exclusionToValidate.EscenarioCupoId}."));
        }
    }
}
