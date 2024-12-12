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
    /// Sincroniza los detalles de un escenario de cupo, identificando cuáles deben crearse, actualizarse o eliminarse.
    /// </summary>
    public async Task<(IEnumerable<DetalleEscenarioCupo> nuevas, IEnumerable<DetalleEscenarioCupo> actualizar, IEnumerable<DetalleEscenarioCupo> eliminar)>
    SyncDetallesAsync(
    int escenarioCupoId,
    IEnumerable<DetalleEscenarioCupoInfo> nuevosDetallesInfo,
    CancellationToken cancellationToken)
    {
        var detallesExistentes = await _detalleEscenarioCupoRepository.FindByIdsAsync(escenarioCupoId, cancellationToken)
                                  ?? Enumerable.Empty<DetalleEscenarioCupo>();

        var nuevosDetalles = nuevosDetallesInfo?.Select(x => DetalleEscenarioCupo.Create(
            escenarioCupoId,
            x.ServicioId,
            x.TopeDiario,
            x.UsaSobreCupo,
            x.HoraMaximaVenta!.Value,
            x.HoraMaximaRevalidacion!.Value,
            x.UsaTopeEnCupo,
            x.TopeFlotante)).ToList() ?? new List<DetalleEscenarioCupo>();

        var detallesParaActualizar = detallesExistentes
            .Where(d => nuevosDetalles.Any(n => n.ServicioId == d.ServicioId))
            .ToList();

        var detallesParaEliminar = detallesExistentes
            .Where(d => !nuevosDetalles.Any(n => n.ServicioId == d.ServicioId))
            .ToList();

        var detallesParaAgregar = nuevosDetalles
            .Where(n => !detallesExistentes.Any(d => d.ServicioId == n.ServicioId))
            .ToList();

        foreach (var detalle in detallesParaActualizar)
        {
            var nuevoDetalle = nuevosDetalles.FirstOrDefault(n => n.ServicioId == detalle.ServicioId);
            if (nuevoDetalle != null)
            {
                detalle.Update(nuevoDetalle);
            }
        }

        return (detallesParaAgregar, detallesParaActualizar, detallesParaEliminar);
    }

}
