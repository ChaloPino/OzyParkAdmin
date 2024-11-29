using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
public sealed class DetalleEscenarioCupoExclusionManager : IBusinessLogic
{
    private readonly IDetalleEscenarioCupoExclusionRepository _detalleExclusionRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;

    public DetalleEscenarioCupoExclusionManager(
       IDetalleEscenarioCupoExclusionRepository detalleExclusionRepository,
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
    public async Task<ResultOf<IEnumerable<DetalleEscenarioCupoExclusion>>> UpdateExclusionesAsync(
        int escenarioCupoId,
        IEnumerable<DetalleEscenarioCupoExclusionFullInfo> nuevasExclusionesInfo,
        CancellationToken cancellationToken)
    {
        var exclusionesExistentes = await _detalleExclusionRepository.GetExclusionesByEscenarioCupoIdAsync(escenarioCupoId, cancellationToken);
        var nuevasExclusiones = nuevasExclusionesInfo.Select(d => DetalleEscenarioCupoExclusion.Create(
            escenarioCupoId,
            d.ServicioId,
            d.CanalVentaId,
            d.DiaSemanaId!,
            d.HoraInicio,
            d.HoraFin)).ToList();

        var errors = new List<ValidationError>();
        foreach (var exclusion in nuevasExclusiones)
        {
            await ValidateDuplicityAsync((exclusion.EscenarioCupoId, exclusion.ServicioId, exclusion.CanalVentaId, exclusion.DiaSemanaId, exclusion.HoraInicio), errors, cancellationToken);
        }

        if (errors.Any())
        {
            return errors;
        }

        foreach (var exclusion in exclusionesExistentes)
        {
            var exclusionActualizada = nuevasExclusiones.FirstOrDefault(nd => nd.ServicioId == exclusion.ServicioId && nd.CanalVentaId == exclusion.CanalVentaId && nd.DiaSemanaId == exclusion.DiaSemanaId && exclusion.HoraInicio == nd.HoraInicio);
            if (exclusionActualizada != null)
            {
                exclusion.Update(exclusionActualizada);
            }
        }

        return nuevasExclusiones;
    }

    /// <summary>
    /// Valida duplicidad al actualizar o crear exclusiones.
    /// </summary>
    private async Task ValidateDuplicityAsync(
        (int EscenarioCupoId, int ServicioId, int CanalVentaId, int diaSemanaId, TimeSpan horaInicio) exclusionToValidate,
        IList<ValidationError> errors,
        CancellationToken cancellationToken)
    {
        var exclusiones = await _detalleExclusionRepository.GetExclusionesByEscenarioCupoIdAsync(exclusionToValidate.EscenarioCupoId, cancellationToken);

        var existente = exclusiones.FirstOrDefault(x =>
            x.HoraInicio == exclusionToValidate.horaInicio &&
            x.ServicioId == exclusionToValidate.ServicioId &&
            x.CanalVentaId == exclusionToValidate.CanalVentaId &&
            x.DiaSemanaId == exclusionToValidate.diaSemanaId);

        if (existente is not null && existente.EscenarioCupoId != exclusionToValidate.EscenarioCupoId)
        {
            errors.Add(new ValidationError(
                nameof(DetalleEscenarioCupoExclusionFecha),
                $"La exclusión con ServicioId {exclusionToValidate.ServicioId}, CanalVentaId {exclusionToValidate.CanalVentaId}, DiaSemanaId {exclusionToValidate.diaSemanaId} y HoraInicio {exclusionToValidate.horaInicio} ya existe para el EscenarioCupoId {exclusionToValidate.EscenarioCupoId}."));
        }
    }
}
