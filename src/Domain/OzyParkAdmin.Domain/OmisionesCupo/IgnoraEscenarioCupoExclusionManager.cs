using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Domain.OmisionesCupo;

/// <summary>
/// Contiene la lógica de negocio de <see cref="IgnoraEscenarioCupoExclusion"/>.
/// </summary>
/// <remarks>
/// Implementa <see cref="IBusinessLogic"/> para que en la infraestructura registre en el DI automáticamente.
/// </remarks>
public sealed class IgnoraEscenarioCupoExclusionManager : IBusinessLogic
{
    private readonly IIgnoraEscenarioCupoExclusionRepository _ignoraEscenarioCupoExclusionRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="IgnoraEscenarioCupoExclusionManager"/>.
    /// </summary>
    /// <param name="ignoraEscenarioCupoExclusionRepository">El <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.</param>
    /// <param name="escenarioCupoRepository">El <see cref="IEscenarioCupoRepository"/>.</param>
    public IgnoraEscenarioCupoExclusionManager(
        IIgnoraEscenarioCupoExclusionRepository ignoraEscenarioCupoExclusionRepository,
        IEscenarioCupoRepository escenarioCupoRepository)
    {
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        _ignoraEscenarioCupoExclusionRepository = ignoraEscenarioCupoExclusionRepository;
        _escenarioCupoRepository = escenarioCupoRepository;
    }

    /// <summary>
    /// Crea una nueva <see cref="IgnoraEscenarioCupoExclusion"/>.
    /// </summary>
    /// <param name="escenariosCupoInfo">Los escenarios de cupo.</param>
    /// <param name="canalesVenta">Los canales de venta.</param>
    /// <param name="fechaDesde">La fecha desde para crear</param>
    /// <param name="fechaHasta">La fecha hasta para crear</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de crear varios <see cref="IgnoraEscenarioCupoExclusion"/>.</returns>
    public async Task<ResultOf<IEnumerable<IgnoraEscenarioCupoExclusion>>> CreateAsync(
        IEnumerable<EscenarioCupoInfo> escenariosCupoInfo,
        IEnumerable<CanalVenta> canalesVenta,
        DateOnly fechaDesde,
        DateOnly fechaHasta,
        CancellationToken cancellationToken)
    {
        fechaDesde = AsegurarFechaDesde(fechaDesde);

        IEnumerable<EscenarioCupo> escenariosCupo = await _escenarioCupoRepository.FindByIdsAsync(escenariosCupoInfo.Select(x => x.Id).ToArray(), cancellationToken);
        IEnumerable<DateOnly> fechas = DateTimeUitls.CreateDates(fechaDesde, fechaHasta);
        IEnumerable<(EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DateOnly Fecha)> keys = ProyectarClaves(canalesVenta, escenariosCupo, fechas);

        var existentes = await _ignoraEscenarioCupoExclusionRepository.FindByKeysAsync(keys.Select(x => (x.EscenarioCupo.Id, x.CanalVenta.Id, x.Fecha)), cancellationToken);
        var paraCrear = keys.ExceptBy(existentes.Select(x => (x.EscenarioCupo, x.CanalVenta, x.FechaIgnorada)), x => (x.EscenarioCupo, x.CanalVenta, x.Fecha));

        List<IgnoraEscenarioCupoExclusion> creados = [];

        foreach (var key in paraCrear)
        {
            var result = IgnoraEscenarioCupoExclusion.Create(key.EscenarioCupo, key.CanalVenta, key.Fecha);

            if (result.IsFailure(out Failure failure))
            {
                return failure;
            }

            if (result.IsSuccess(out IgnoraEscenarioCupoExclusion? creado))
            {
                creados.Add(creado);
            }
        }

        return creados;
    }

    private static DateOnly AsegurarFechaDesde(DateOnly fechaDesde)
    {
        DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
        return fechaDesde < hoy ? hoy : fechaDesde;
    }

    private static IEnumerable<(EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DateOnly Fecha)> ProyectarClaves(IEnumerable<CanalVenta> canalesVenta, IEnumerable<EscenarioCupo> escenariosCupo, IEnumerable<DateOnly> fechas)
    {
        return [..from escenarioCupo in escenariosCupo
                  from canalVenta in canalesVenta
                  from fecha in fechas
                  select (escenarioCupo, canalVenta, fecha)];
    }
}
