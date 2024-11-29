using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Domain.TarifasServicio;

/// <summary>
/// Contiene las lógicas de negocio de <see cref="TarifaServicio"/>.
/// </summary>
public sealed class TarifaServicioManager : IBusinessLogic
{
    private readonly ITarifaServicioRepository _tarifaServicioRepository;
    private readonly IServicioRepository _servicioRepository;
    private readonly ITramoRepository _tramoRepository;
    private readonly IGenericRepository<GrupoEtario> _grupoEtarioRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="TarifaServicioManager"/>.
    /// </summary>
    /// <param name="tarifaServicioRepository">El <see cref="ITarifaServicioRepository"/>.</param>
    /// <param name="servicioRepository">El <see cref="IServicioRepository"/>.</param>
    /// <param name="tramoRepository">El <see cref="ITramoRepository"/>.</param>
    /// <param name="grupoEtarioRepository">El repositorio de <see cref="GrupoEtario"/>.</param>
    public TarifaServicioManager(
        ITarifaServicioRepository tarifaServicioRepository,
        IServicioRepository servicioRepository,
        ITramoRepository tramoRepository,
        IGenericRepository<GrupoEtario> grupoEtarioRepository)
    {
        ArgumentNullException.ThrowIfNull(tarifaServicioRepository);
        ArgumentNullException.ThrowIfNull(servicioRepository);
        ArgumentNullException.ThrowIfNull(tramoRepository);
        ArgumentNullException.ThrowIfNull(grupoEtarioRepository);

        _tarifaServicioRepository = tarifaServicioRepository;
        _servicioRepository = servicioRepository;
        _tramoRepository = tramoRepository;
        _grupoEtarioRepository = grupoEtarioRepository;
    }

    /// <summary>
    /// Crea varias tarifas de servicio.
    /// </summary>
    /// <param name="inicioVigencia">El inicio de vigencia de la tarifa.</param>
    /// <param name="moneda">La moneda de la tarifa.</param>
    /// <param name="servicioInfo">El servicio de la tarifa.</param>
    /// <param name="tramosInfo">Lista de tramos del servicio.</param>
    /// <param name="gruposEtariosInfo">Lista de grupos etarios del servicio.</param>
    /// <param name="canalesVenta">Lista de canales de venta.</param>
    /// <param name="tiposDia">Lista de tipos de día.</param>
    /// <param name="tiposHorario">Lista de tipos de horario.</param>
    /// <param name="tiposSegmentacion">Lista de tipos de segmentación.</param>
    /// <param name="valorAfecto">El valor afecto de la tarifa.</param>
    /// <param name="valorExento">El valor exento de la tarifa.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// El resultado de la creación de las tarifas de servicio.
    /// </returns>
    public async Task<ResultOf<IEnumerable<TarifaServicio>>> CreateAsync(
        DateTime inicioVigencia,
        Moneda moneda,
        ServicioInfo servicioInfo,
        IEnumerable<TramoInfo> tramosInfo,
        IEnumerable<GrupoEtarioInfo> gruposEtariosInfo,
        IEnumerable<CanalVenta> canalesVenta,
        IEnumerable<TipoDia> tiposDia,
        IEnumerable<TipoHorario> tiposHorario,
        IEnumerable<TipoSegmentacion> tiposSegmentacion,
        decimal valorAfecto,
        decimal valorExento,
        CancellationToken cancellationToken)
    {
        Servicio? servicio = await _servicioRepository.FindByIdAsync(servicioInfo.Id, cancellationToken).ConfigureAwait(false);

        if (servicio is null)
        {
            return new ValidationError(nameof(TarifaServicio.Servicio), "El servicio no existe");
        }

        IEnumerable<Tramo> tramos = await _tramoRepository.FindByIdsAsync(tramosInfo.Select(x => x.Id).ToArray(), cancellationToken).ConfigureAwait(false);

        int[] gruposEtariosIds = gruposEtariosInfo.Select(x => x.Id).ToArray();
        IEnumerable<GrupoEtario> gruposEtarios = await _grupoEtarioRepository.ListAsync(
            predicate: x => Enumerable.Contains(gruposEtariosIds, x.Id), cancellationToken: cancellationToken).ConfigureAwait(false);

        var tarifasToCreate = (from tramo in tramos
                               from grupoEtario in gruposEtarios
                               from canalVenta in canalesVenta
                               from tipoDia in tiposDia
                               from tipoHorario in tiposHorario
                               from tipoSegmentacion in tiposSegmentacion
                               select (inicioVigencia, moneda, servicio, tramo, grupoEtario, canalVenta, tipoDia, tipoHorario, tipoSegmentacion))
                               .ToList();

        var existentes = await _tarifaServicioRepository.FindByPrimaryKeysAsync(tarifasToCreate, cancellationToken).ConfigureAwait(false);

        tarifasToCreate = tarifasToCreate.Except(existentes).ToList();
        List<TarifaServicio> newTarifas = [];
        foreach (var create in tarifasToCreate)
        {
            newTarifas.Add(TarifaServicio.Create(
                create.inicioVigencia,
                create.moneda,
                create.servicio,
                create.tramo,
                create.grupoEtario,
                create.canalVenta,
                create.tipoDia,
                create.tipoHorario,
                create.tipoSegmentacion,
                valorAfecto,
                valorExento));
        }

        return newTarifas;
    }

    /// <summary>
    /// Actualiza una tarifa de servicio.
    /// </summary>
    /// <param name="inicioVigencia">El inicio de vigencia de la tarifa a actualizar.</param>
    /// <param name="moneda">La momeda de la tarifa a actualizar.</param>
    /// <param name="servicio">El servicio de la tarifa a actualizar.</param>
    /// <param name="tramo">El tramo de la tarifa a actualizar.</param>
    /// <param name="grupoEtario">El grupo etario de la tarifa a actualizar.</param>
    /// <param name="canalVenta">El canal de venta de la tarifa a actualizar.</param>
    /// <param name="tipoDia">El tipo de día de la tarifa a actualizar.</param>
    /// <param name="tipoHorario">El tipo de horario de la tarifa a actualizar.</param>
    /// <param name="tipoSegmentacion">El tipo de segmentación de la tarifa a actualizar.</param>
    /// <param name="valorAfecto">El valor afecto de la tarifa.</param>
    /// <param name="valorExento">El valor exento de la tarifa.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>
    /// El resultado de la actualización de la tarifa de servicio.
    /// </returns>
    public async Task<ResultOf<TarifaServicio>> UpdateAsync(
        DateTime inicioVigencia,
        Moneda moneda,
        ServicioInfo servicio,
        TramoInfo tramo,
        GrupoEtarioInfo grupoEtario,
        CanalVenta canalVenta,
        TipoDia tipoDia,
        TipoHorario tipoHorario,
        TipoSegmentacion tipoSegmentacion,
        decimal valorAfecto,
        decimal valorExento,
        CancellationToken cancellationToken)
    {
        TarifaServicio? tarifa = await _tarifaServicioRepository.FindByPrimaryKeyAsync(
            inicioVigencia,
            moneda.Id,
            servicio.Id,
            tramo.Id,
            grupoEtario.Id,
            canalVenta.Id,
            tipoDia.Id,
            tipoHorario.Id,
            tipoSegmentacion.Id,
            cancellationToken);

        if (tarifa is null)
        {
            return new NotFound();
        }

        tarifa.Update(valorAfecto, valorExento);

        return tarifa;
    }
}
