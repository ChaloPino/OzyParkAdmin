using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El administrador de servicios.
/// </summary>
public sealed class ServicioManager
{
    private readonly IServicioRepository _repository;
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly ICajaRepository _cajaRepository;
    private readonly ITramoRepository _tramoRepository;
    private readonly IZonaRepository _zonaRepository;
    private readonly IGenericRepository<GrupoEtario> _grupoEtarioRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ServicioManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    /// <param name="centroCostoRepository">El <see cref="ICentroCostoRepository"/>.</param>
    /// <param name="cajaRepository">El <see cref="ICajaRepository"/>.</param>
    /// <param name="tramoRepository">El <see cref="ITramoRepository"/>.</param>
    /// <param name="zonaRepository">El <see cref="IZonaRepository"/>.</param>
    /// <param name="grupoEtarioRepository">El <see cref="IGenericRepository{TEntity}"/> para grupos etarios.</param>
    public ServicioManager(
        IServicioRepository repository,
        ICentroCostoRepository centroCostoRepository,
        ICajaRepository cajaRepository,
        ITramoRepository tramoRepository,
        IZonaRepository zonaRepository,
        IGenericRepository<GrupoEtario> grupoEtarioRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(centroCostoRepository);
        ArgumentNullException.ThrowIfNull(cajaRepository);
        ArgumentNullException.ThrowIfNull(tramoRepository);
        ArgumentNullException.ThrowIfNull(zonaRepository);
        ArgumentNullException.ThrowIfNull(grupoEtarioRepository);
        _repository = repository;
        _centroCostoRepository = centroCostoRepository;
        _cajaRepository = cajaRepository;
        _tramoRepository = tramoRepository;
        _zonaRepository = zonaRepository;
        _grupoEtarioRepository = grupoEtarioRepository;

    }

    /// <summary>
    /// Asigna o desasigna cajas de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a asignar las cajas.</param>
    /// <param name="cajasAsociar">Las cajas a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar las cajas.</returns>
    public async Task<ResultOf<Servicio>> AssignCajasAsync(int servicioId, ImmutableArray<ServicioPorCajaInfo> cajasAsociar, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        IEnumerable<Caja> cajas = await _cajaRepository.FindByIdsAsync(cajasAsociar.Select(x => x.Caja.Id).ToArray(), cancellationToken);

        IEnumerable<(ServicioPorCajaInfo Info, Caja Caja)> cajasInfo = from info in cajasAsociar
                                                                        join caja in cajas on info.Caja.Id equals caja.Id
                                                                        select (info, caja);

        return servicio.AssignCajas(cajasInfo);
    }

    /// <summary>
    /// Asigna o desasigna centros de costo de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a asignar los centros de costo.</param>
    /// <param name="centrosCostoAsociar">Los centros de costo a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar los centros de costo.</returns>
    public async Task<ResultOf<Servicio>> AssignCentrosCostoAsync(int servicioId, ImmutableArray<CentroCostoServicioInfo> centrosCostoAsociar, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        IEnumerable<CentroCosto> centrosCosto = await _centroCostoRepository.FindByIdsAsync(centrosCostoAsociar.Select(x => x.CentroCosto.Id).ToArray(), cancellationToken);

        IEnumerable<(CentroCostoServicioInfo Info, CentroCosto CentroCosto)> centrosCostoInfo = from info in centrosCostoAsociar
                                                                                         join centroCosto in centrosCosto on info.CentroCosto.Id equals centroCosto.Id
                                                                                         select (info, centroCosto);

        return servicio.AssignCentrosCosto(centrosCostoInfo);
    }

    /// <summary>
    /// Asigna o desasigna tramos de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a asignar los tramos.</param>
    /// <param name="tramosAsignar">Los tramos a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar los tramos.</returns>
    public async Task<ResultOf<Servicio>> AssignTramosAsync(int servicioId, ImmutableArray<TramoServicioInfo> tramosAsignar, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        IEnumerable<CentroCosto> centrosCosto = await _centroCostoRepository.FindByIdsAsync(tramosAsignar.Select(x => x.CentroCosto.Id).ToArray(), cancellationToken);
        IEnumerable<Tramo> tramos = await _tramoRepository.FindByIdsAsync(tramosAsignar.Select(x => x.Tramo.Id).ToArray(), cancellationToken);

        IEnumerable<(TramoServicioInfo Info, CentroCosto CentroCosto, Tramo Tramo)> tramosInfo = from info in tramosAsignar
                                                                                                 join centroCosto in centrosCosto on info.CentroCosto.Id equals centroCosto.Id
                                                                                                 join tramo in tramos on info.Tramo.Id equals tramo.Id
                                                                                                 select (info, centroCosto, tramo);

        return servicio.AssignTramos(tramosInfo);
    }

    /// <summary>
    /// Asigna o desasigna permisos de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a asignar los permisos.</param>
    /// <param name="permisosAsignar">Los permisos a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar los permisos.</returns>
    public async Task<ResultOf<Servicio>> AssignPermisosAsync(int servicioId, ImmutableArray<PermisoServicioInfo> permisosAsignar, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        IEnumerable<Tramo> tramos = await _tramoRepository.FindByIdsAsync(permisosAsignar.Select(x => x.Tramo.Id).ToArray(), cancellationToken);
        IEnumerable<CentroCosto> centrosCosto = await _centroCostoRepository.FindByIdsAsync(permisosAsignar.Select(x => x.CentroCosto.Id).ToArray(), cancellationToken);

        IEnumerable<(PermisoServicioInfo Info, Tramo Tramo, CentroCosto CentroCosto)> permisosInfo = from info in permisosAsignar
                                                                                                     join centroCosto in centrosCosto on info.CentroCosto.Id equals centroCosto.Id
                                                                                                     join tramo in tramos on info.Tramo.Id equals tramo.Id
                                                                                                     select (info, tramo, centroCosto);

        return servicio.AssignPermisos(permisosInfo);
    }

    /// <summary>
    /// Asigna o desasigna zonas de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a asignar las zonas.</param>
    /// <param name="zonasAsignar">Las zonas a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar las zonas.</returns>
    public async Task<ResultOf<Servicio>> AssignZonasAsync(int servicioId, ImmutableArray<ZonaPorTramoInfo> zonasAsignar, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        IEnumerable<Tramo> tramos = await _tramoRepository.FindByIdsAsync(zonasAsignar.Select(x => x.Tramo.Id).ToArray(), cancellationToken);
        IEnumerable<Zona> zonas = await _zonaRepository.FindByIdsAsync(zonasAsignar.Select(x => x.Zona.Id).ToArray(), cancellationToken);

        IEnumerable<(ZonaPorTramoInfo Info, Tramo Tramo, Zona Zona)> zonasInfo = from info in zonasAsignar
                                                                                 join tramo in tramos on info.Tramo.Id equals tramo.Id
                                                                                 join zona in zonas on info.Zona.Id equals zona.Id
                                                                                 select (info, tramo, zona);

        return servicio.AssignZonasTramos(zonasInfo);
    }

    /// <summary>
    /// Asigna o desasigna grupos etarios de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a asignar los grupos etarios.</param>
    /// <param name="gruposEtariosAssign">Los grupos etarios a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar los grupos etarios.</returns>
    public async Task<ResultOf<Servicio>> AssignGruposEtariosAsync(int servicioId, ImmutableArray<GrupoEtarioInfo> gruposEtariosAssign, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        int[] grupoEtarioIds = gruposEtariosAssign.Select(x => x.Id).ToArray();

        IEnumerable<GrupoEtario> gruposEtarios = await _grupoEtarioRepository.ListAsync(predicate: x => grupoEtarioIds.Contains(x.Id), cancellationToken: cancellationToken);

        return servicio.AssignGruposEtarios(gruposEtarios);
    }

    /// <summary>
    /// Activa un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a activar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de activar el servicio.</returns>
    public async Task<ResultOf<Servicio>> ActivarServicioAsync(int servicioId, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        servicio.Activar();

        return servicio;
    }

    /// <summary>
    /// Desactiva un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a desactivar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de desactivar el servicio.</returns>
    public async Task<ResultOf<Servicio>> DesactivarServicioAsync(int servicioId, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(servicioId, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        servicio.Desactivar();

        return servicio;
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Servicio"/>.
    /// </summary>
    /// <param name="centroCostoInfo">El centro de costo.</param>
    /// <param name="franquiciaId">El id de franquicia.</param>
    /// <param name="aka">El aka del servicio.</param>
    /// <param name="nombre">El nombre del servicio.</param>
    /// <param name="tipoControl">El tipo de control.</param>
    /// <param name="tipoDistribucion">El tipo de distribución.</param>
    /// <param name="tipoServicio">El tipo de servicio.</param>
    /// <param name="tipoVigencia">El tipo de vigencia.</param>
    /// <param name="numeroVigencia">El número de vigencia.</param>
    /// <param name="numeroValidez">El número de validez.</param>
    /// <param name="numeroPaxMinimo">La cantidad mínima de pasajeros.</param>
    /// <param name="numeroPaxMaximo">La cantidad máxima de pasajeros.</param>
    /// <param name="esConHora">Si es con hora.</param>
    /// <param name="esPorTramos">Si es por tramos.</param>
    /// <param name="esParaVenta">Si es para venta.</param>
    /// <param name="orden">El orden.</param>
    /// <param name="holguraInicio">La holgura de inicio.</param>
    /// <param name="holguraFin">La holgura de fin.</param>
    /// <param name="esParaMovil">Si es para dispositivos móviles.</param>
    /// <param name="mostrarTramos">Si se muestra los tramos.</param>
    /// <param name="esParaBuses">Si es para buses.</param>
    /// <param name="idaVuelta">El tipo de servicio para buses.</param>
    /// <param name="holguraEntrada">La holgura de entrada.</param>
    /// <param name="controlParental">Si tiene control parental.</param>
    /// <param name="servicioResponsableId">El servicio responsable para cuando tiene control parental.</param>
    /// <param name="politicas">La políticas de uso.</param>
    /// <param name="plantillaId">La plantilla asociada al servicio.</param>
    /// <param name="plantillaDigitalId">La plantilla digital asociada al servicio.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de crear un nuevo servicio.</returns>
    public async Task<ResultOf<Servicio>> CreateAsync(
        CentroCostoInfo centroCostoInfo,
        int franquiciaId,
        string aka,
        string nombre,
        TipoControl tipoControl,
        TipoDistribucion tipoDistribucion,
        TipoServicio tipoServicio,
        TipoVigencia tipoVigencia,
        short numeroVigencia,
        short numeroValidez,
        short numeroPaxMinimo,
        short numeroPaxMaximo,
        bool esConHora,
        bool esPorTramos,
        bool esParaVenta,
        int orden,
        TimeSpan? holguraInicio,
        TimeSpan? holguraFin,
        bool esParaMovil,
        bool? mostrarTramos,
        bool esParaBuses,
        TipoServicio? idaVuelta,
        byte holguraEntrada,
        bool controlParental,
        int? servicioResponsableId,
        string? politicas,
        int plantillaId,
        int plantillaDigitalId,
        CancellationToken cancellationToken)
    {
        CentroCosto? centroCosto = await _centroCostoRepository.FindByIdAsync(centroCostoInfo.Id, cancellationToken);

        if (centroCosto is null)
        {
            return new ValidationError(nameof(Servicio.CentroCosto), "El centro de costo no existe");
        }

        int newId = await GenerarIdAsync(cancellationToken);

        return Servicio.Create(
            newId,
            centroCosto,
            franquiciaId,
            aka,
            nombre,
            tipoControl,
            tipoDistribucion,
            tipoServicio,
            tipoVigencia,
            numeroVigencia,
            numeroValidez,
            numeroPaxMinimo,
            numeroPaxMaximo,
            esConHora,
            esPorTramos,
            esParaVenta,
            orden,
            holguraInicio,
            holguraFin,
            esParaMovil,
            mostrarTramos,
            esParaBuses,
            idaVuelta,
            holguraEntrada,
            controlParental,
            servicioResponsableId,
            politicas,
            plantillaId,
            plantillaDigitalId);
    }

    /// <summary>
    /// Actualiza un servicio.
    /// </summary>
    /// <param name="id">El id del servicio a actualizar.</param>
    /// <param name="centroCostoInfo">El centro de costo.</param>
    /// <param name="franquiciaId">El id de franquicia.</param>
    /// <param name="aka">El aka del servicio.</param>
    /// <param name="nombre">El nombre del servicio.</param>
    /// <param name="tipoControl">El tipo de control.</param>
    /// <param name="tipoDistribucion">El tipo de distribución.</param>
    /// <param name="tipoServicio">El tipo de servicio.</param>
    /// <param name="tipoVigencia">El tipo de vigencia.</param>
    /// <param name="numeroVigencia">El número de vigencia.</param>
    /// <param name="numeroValidez">El número de validez.</param>
    /// <param name="numeroPaxMinimo">La cantidad mínima de pasajeros.</param>
    /// <param name="numeroPaxMaximo">La cantidad máxima de pasajeros.</param>
    /// <param name="esConHora">Si es con hora.</param>
    /// <param name="esPorTramos">Si es por tramos.</param>
    /// <param name="esParaVenta">Si es para venta.</param>
    /// <param name="orden">El orden.</param>
    /// <param name="holguraInicio">La holgura de inicio.</param>
    /// <param name="holguraFin">La holgura de fin.</param>
    /// <param name="esParaMovil">Si es para dispositivos móviles.</param>
    /// <param name="mostrarTramos">Si se muestra los tramos.</param>
    /// <param name="esParaBuses">Si es para buses.</param>
    /// <param name="idaVuelta">El tipo de servicio para buses.</param>
    /// <param name="holguraEntrada">La holgura de entrada.</param>
    /// <param name="controlParental">Si tiene control parental.</param>
    /// <param name="servicioResponsableId">El servicio responsable para cuando tiene control parental.</param>
    /// <param name="politicas">La políticas de uso.</param>
    /// <param name="plantillaId">La plantilla asociada al servicio.</param>
    /// <param name="plantillaDigitalId">La plantilla digital asociada al servicio.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de actualizar el servicio.</returns>
    public async Task<ResultOf<Servicio>> UpdateAsync(
        int id,
        CentroCostoInfo centroCostoInfo,
        int franquiciaId,
        string aka,
        string nombre,
        TipoControl tipoControl,
        TipoDistribucion tipoDistribucion,
        TipoServicio tipoServicio,
        TipoVigencia tipoVigencia,
        short numeroVigencia,
        short numeroValidez,
        short numeroPaxMinimo,
        short numeroPaxMaximo,
        bool esConHora,
        bool esPorTramos,
        bool esParaVenta,
        int orden,
        TimeSpan? holguraInicio,
        TimeSpan? holguraFin,
        bool esParaMovil,
        bool? mostrarTramos,
        bool esParaBuses,
        TipoServicio? idaVuelta,
        byte holguraEntrada,
        bool controlParental,
        int? servicioResponsableId,
        string? politicas,
        int plantillaId,
        int plantillaDigitalId,
        CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByIdAsync(id, cancellationToken);

        if (servicio is null)
        {
            return new NotFound();
        }

        CentroCosto? centroCosto = await _centroCostoRepository.FindByIdAsync(centroCostoInfo.Id, cancellationToken);

        if (centroCosto is null)
        {
            return new ValidationError(nameof(Servicio.CentroCosto), "El centro de costo no existe");
        }

        return servicio.Update(
            aka,
            nombre,
            tipoControl,
            tipoDistribucion,
            tipoServicio,
            tipoVigencia,
            numeroVigencia,
            numeroValidez,
            numeroPaxMinimo,
            numeroPaxMaximo,
            esConHora,
            esPorTramos,
            esParaVenta,
            orden,
            holguraInicio,
            holguraFin,
            esParaMovil,
            mostrarTramos,
            esParaBuses,
            idaVuelta,
            holguraEntrada,
            controlParental,
            servicioResponsableId,
            politicas,
            plantillaId,
            plantillaDigitalId);
    }

    private async Task<int> GenerarIdAsync(CancellationToken cancellationToken)
    {
        int id = await _repository.MaxIdAsync(cancellationToken);
        return id + 1;
    }

    
}
