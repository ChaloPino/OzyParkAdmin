using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.DetallesEscenariosCupos;

/// <summary>
/// Administrador de la lógica de negocio para la entidad <see cref="DetalleEscenarioCupo"/>.
/// </summary>
public sealed class DetalleEscenarioCupoManager : IBusinessLogic
{
    private readonly IDetalleEscenarioCupoRepository _detalleEscenarioCupoRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;

    public DetalleEscenarioCupoManager(
        IDetalleEscenarioCupoRepository detalleEscenarioCupoRepository,
        IEscenarioCupoRepository escenarioCupoRepository)
    {
        ArgumentNullException.ThrowIfNull(detalleEscenarioCupoRepository);
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);

        _detalleEscenarioCupoRepository = detalleEscenarioCupoRepository;
        _escenarioCupoRepository = escenarioCupoRepository;
    }

    /// <summary>
    /// Actualiza múltiples detalles para un escenario de cupo.
    /// </summary>
    public async Task<ResultOf<IEnumerable<DetalleEscenarioCupo>>> UpdateDetallesAsync(
        int escenarioCupoId,
        IEnumerable<DetalleEscenarioCupoInfo> nuevosDetallesInfo,
        CancellationToken cancellationToken)
    {
        var detallesExistentes = await _detalleEscenarioCupoRepository.FindByIdsAsync(escenarioCupoId, cancellationToken);
        var nuevosDetalles = nuevosDetallesInfo.Select(d => DetalleEscenarioCupo.Create(
            escenarioCupoId,
            d.ServicioId,
            d.TopeDiario,
            d.UsaSobreCupo,
            d.HoraMaximaVenta!.Value,
            d.HoraMaximaRevalidacion!.Value,
            d.UsaTopeEnCupo,
            d.TopeFlotante)).ToList();

        var errors = new List<ValidationError>();
        foreach (var detalle in nuevosDetalles)
        {
            await ValidateDuplicityAsync((detalle.EscenarioCupoId, detalle.ServicioId), errors, cancellationToken);
        }

        if (errors.Any())
        {
            return errors;
        }

        foreach (var detalle in detallesExistentes)
        {
            var detalleActualizado = nuevosDetalles.FirstOrDefault(nd => nd.ServicioId == detalle.ServicioId);
            if (detalleActualizado != null)
            {
                detalle.Update(detalleActualizado);
            }
        }

        return nuevosDetalles;
    }

    /// <summary>
    /// Crea múltiples detalles para un escenario de cupo.
    /// </summary>
    public async Task<ResultOf<IEnumerable<DetalleEscenarioCupo>>> CreateDetallesAsync(
        int escenarioCupoId,
        IEnumerable<DetalleEscenarioCupoInfo> detallesInfo,
        CancellationToken cancellationToken)
    {
        var detalles = detallesInfo.Select(d => DetalleEscenarioCupo.Create(
            escenarioCupoId,
            d.ServicioId,
            d.TopeDiario,
            d.UsaSobreCupo,
            d.HoraMaximaVenta!.Value,
            d.HoraMaximaRevalidacion!.Value,
            d.UsaTopeEnCupo,
            d.TopeFlotante)).ToList();

        var errors = new List<ValidationError>();

        foreach (var detalle in detalles)
        {
            await ValidateDuplicityAsync((detalle.EscenarioCupoId, detalle.ServicioId), errors, cancellationToken);
        }

        if (errors.Any())
        {
            return errors;
        }

        return detalles;
    }

    /// <summary>
    /// Valida duplicidad al actualizar detalles.
    /// </summary>
    private async Task ValidateDuplicityAsync(
        (int EscenarioCupoId, int ServicioId) detalleToValidate,
        IList<ValidationError> errors,
        CancellationToken cancellationToken)
    {
        var detalles = await _detalleEscenarioCupoRepository.FindByIdsAsync(detalleToValidate.EscenarioCupoId, cancellationToken);

        var existente = detalles.FirstOrDefault(x => x.ServicioId == detalleToValidate.ServicioId);
        if (existente is not null && existente.EscenarioCupoId != detalleToValidate.EscenarioCupoId)
        {
            errors.Add(new ValidationError(
                nameof(DetalleEscenarioCupo),
                $"El detalle con ServicioId {detalleToValidate.ServicioId} ya existe para el EscenarioCupoId {detalleToValidate.EscenarioCupoId}."));
        }
    }
}
