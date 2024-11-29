using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// La entidad servicio.
/// </summary>
public sealed class Servicio
{
    private ServicioPolitica? _servicioPolitica;
    private ServicioControlParental? _servicioControlParental;
    private readonly List<TramoServicio> _tramosServicio = [];
    private readonly List<CentroCostoServicio> _centrosCosto = [];
    private readonly List<GrupoEtario> _gruposEtarios = [];
    private readonly List<ServicioPorCaja> _serviciosPorCaja = [];
    private readonly List<PermisoServicio> _permisos = [];
    /// <summary>
    /// El id del servicio.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// La franquicia a la que pertenece el servicio.
    /// </summary>
    public int FranquiciaId { get; private set; }

    /// <summary>
    /// El aka del servicio.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// El nombre del servicio.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// El número de vigencia.
    /// </summary>
    public short NumeroVigencia { get; private set; }

    /// <summary>
    /// El número de validez.
    /// </summary>
    public short NumeroValidez { get; private set; }

    /// <summary>
    /// El número mínimo de pasajeros.
    /// </summary>
    public short NumeroPaxMinimo { get; private set; }

    /// <summary>
    /// El número máximo de pasajeros.
    /// </summary>
    public short NumeroPaxMaximo { get; private set; }

    /// <summary>
    /// El tipo de distribución.
    /// </summary>
    public TipoDistribucion TipoDistribucion { get; private set; } = default!;

    /// <summary>
    /// El tipo de control.
    /// </summary>
    public TipoControl TipoControl { get; private set; } = default!;

    /// <summary>
    /// El tipo de vigencia.
    /// </summary>
    public TipoVigencia TipoVigencia { get; private set; } = default!;

    /// <summary>
    /// El tipo de servicio.
    /// </summary>
    public TipoServicio TipoServicio { get; private set; } = default!;

    /// <summary>
    /// El id de la plantilla para los tickets.
    /// </summary>
    public int PlantillaId { get; private set; }

    /// <summary>
    /// El id de la plantilla para los tickets digitales.
    /// </summary>
    public int PlantillaDigitalId { get; private set; }

    /// <summary>
    /// El id del centro de costo.
    /// </summary>
    public int? CentroCostoId { get; private set; }

    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCosto? CentroCosto { get; private set; }

    /// <summary>
    /// Si el servicio tiene horas de entrada y salida.
    /// </summary>
    public bool EsConHora { get; private set; }

    /// <summary>
    /// Si el servicio tiene tramos.
    /// </summary>
    public bool EsPorTramos { get; private set; }

    /// <summary>
    /// Si el servicio es para la venta.
    /// </summary>
    public bool EsParaVenta { get; private set; }

    /// <summary>
    /// Si el servicio está activo.
    /// </summary>
    public bool EsActivo { get; private set; }

    /// <summary>
    /// El orden cómo se presenta el servicio.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// La holgura de inicio que tiene el servicio para su control.
    /// </summary>
    public TimeSpan? HolguraInicio { get; private set; }

    /// <summary>
    /// La holgura de fin que tiene el servicio para su control.
    /// </summary>
    public TimeSpan? HolguraFin { get; private set; }

    /// <summary>
    /// Las políticas que tiene el servicio.
    /// </summary>
    public string? PoliticasTexto => _servicioPolitica?.Politicas;

    /// <summary>
    /// Las políticas del servicio.
    /// </summary>
    public ServicioPolitica? Politicas => _servicioPolitica;

    /// <summary>
    /// Los tramos asociados al servicio.
    /// </summary>
    public IEnumerable<Tramo> Tramos => _tramosServicio.Select(x => x.Tramo);

    /// <summary>
    /// Los grupos etarios asociados al servicio.
    /// </summary>
    public IEnumerable<GrupoEtario> GruposEtarios => _gruposEtarios;

    /// <summary>
    /// Las cajas asociadas al servicio.
    /// </summary>
    public IEnumerable<Caja> Cajas => _serviciosPorCaja.Select(x => x.Caja);

    /// <summary>
    /// Los datos móviles para un servicio móvil.
    /// </summary>
    public ServicioMovil? Movil { get; private set; }

    /// <summary>
    /// Los datos de bus para un servicio de tipo bus.
    /// </summary>
    public ServicioBus? Bus { get; private set; }

    /// <summary>
    /// Holgura de para empezar la entrada.
    /// </summary>
    public byte HolguraEntrada { get; private set; }

    /// <summary>
    /// Si el servicio tiene control parental.
    /// </summary>
    public bool ControlParental { get; private set; }

    /// <summary>
    /// Los persmisos del servicio.
    /// </summary>
    public IEnumerable<PermisoServicio> Permisos => _permisos;

    /// <summary>
    /// Devuelve toda la información del servicio.
    /// </summary>
    /// <returns>Una nueva instancia de <see cref="ServicioFullInfo"/>.</returns>
    public ServicioFullInfo ToFullInfo() =>
        new()
        {
            Id = Id,
            Aka = Aka,
            Nombre = Nombre,
            FranquiciaId = FranquiciaId,
            CentroCosto = CentroCosto?.ToInfo(),
            TipoControl = TipoControl,
            TipoDistribucion = TipoDistribucion,
            TipoServicio = TipoServicio,
            TipoVigencia = TipoVigencia,
            NumeroVigencia = NumeroVigencia,
            NumeroValidez = NumeroValidez,
            NumeroPaxMinimo = NumeroPaxMinimo,
            NumeroPaxMaximo = NumeroPaxMaximo,
            EsConHora = EsConHora,
            EsPorTramos = EsPorTramos,
            EsParaVenta = EsParaVenta,
            Orden = Orden,
            HolguraInicio = HolguraInicio,
            HolguraFin = HolguraFin,
            EsParaMovil = Movil is not null,
            MostrarTramos = Movil?.MostrarTramos,
            EsParaBuses = Bus is not null,
            IdaVuelta = Bus?.IdaVuelta,
            HolguraEntrada = HolguraEntrada,
            Politicas = PoliticasTexto,
            ControlParental = ControlParental,
            ServicioResponsableId = _servicioControlParental?.ServicioResponsableId,
            Tramos = _tramosServicio.ToInfo(),
            CentrosCosto = _centrosCosto.ToInfo(),
            GruposEtarios = _gruposEtarios.ToInfo(),
            Cajas = _serviciosPorCaja.ToInfo(),
            Permisos = _permisos.ToInfo(),
            PlantillaId = PlantillaId,
            PlantillaDigitalId = PlantillaDigitalId,
            EsActivo = EsActivo,
        };

    /// <summary>
    /// Crea una nueva instancia de <see cref="Servicio"/>.
    /// </summary>
    /// <param name="id">El id del servicio.</param>
    /// <param name="centroCosto">El centro de costo.</param>
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
    /// <param name="servicioResponsableId">El servicio responsable cuando tiene control parental.</param>
    /// <param name="politicas">La políticas de uso.</param>
    /// <param name="plantillaId">La plantilla asociada al servicio.</param>
    /// <param name="plantillaDigitalId">La plantilla digital asociada al servicio.</param>
    public static ResultOf<Servicio> Create(
        int id,
        CentroCosto centroCosto,
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
        int plantillaDigitalId)
    {
        List<ValidationError> errors = [];

        ValidarHolgura(holguraInicio, holguraFin, errors);
        ValidarPax(numeroPaxMinimo, numeroPaxMaximo, errors);

        if (errors.Count > 0)
        {
            return new Validation([..errors]);
        }

        Servicio servicio = new()
        {
            Id = id,
            CentroCostoId = centroCosto.Id,
            CentroCosto = centroCosto,
            FranquiciaId = franquiciaId,
            Aka = aka,
            Nombre = nombre,
            TipoControl = tipoControl,
            TipoDistribucion = tipoDistribucion,
            TipoServicio = tipoServicio,
            TipoVigencia = tipoVigencia,
            NumeroVigencia = numeroVigencia,
            NumeroValidez = numeroValidez,
            NumeroPaxMinimo = numeroPaxMinimo,
            NumeroPaxMaximo = numeroPaxMaximo,
            EsConHora = esConHora,
            EsPorTramos = esPorTramos,
            EsParaVenta = esParaVenta,
            Orden = orden,
            HolguraInicio = holguraInicio,
            HolguraFin = holguraFin,
            HolguraEntrada = holguraEntrada,
            ControlParental = controlParental,
            PlantillaId = plantillaId,
            PlantillaDigitalId = plantillaDigitalId,
            EsActivo = true,
        };

        servicio.AsignarMovil(esParaMovil, mostrarTramos);
        servicio.AsignarBuses(esParaBuses, idaVuelta);
        servicio.AsignarPoliticas(politicas);
        servicio.AsignarServicioResponsable(controlParental, servicioResponsableId);

        return servicio;
    }

    /// <summary>
    /// Actualiza el estado del servicio.
    /// </summary>
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
    /// <param name="servicioResponsableId">El servicio responsable cuando tiene control parental.</param>
    /// <param name="politicas">La políticas de uso.</param>
    /// <param name="plantillaId">La plantilla asociada al servicio.</param>
    /// <param name="plantillaDigitalId">La plantilla digital asociada al servicio.</param>
    public ResultOf<Servicio> Update(
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
        int plantillaDigitalId)
    {
        List<ValidationError> errors = [];
        ValidarHolgura(holguraInicio, holguraFin, errors);
        ValidarPax(numeroPaxMinimo, numeroPaxMaximo, errors);

        if (errors.Count > 0)
        {
            return new Validation([.. errors]);
        }

        Aka = aka;
        Nombre = nombre;
        TipoControl = tipoControl;
        TipoDistribucion = tipoDistribucion;
        TipoServicio = tipoServicio;
        TipoVigencia = tipoVigencia;
        NumeroVigencia = numeroVigencia;
        NumeroValidez = numeroValidez;
        NumeroPaxMinimo = numeroPaxMinimo;
        NumeroPaxMaximo = numeroPaxMaximo;
        EsConHora = esConHora;
        EsPorTramos = esPorTramos;
        EsParaVenta = esParaVenta;
        Orden = orden;
        HolguraInicio = holguraInicio;
        HolguraFin = holguraFin;
        HolguraEntrada = holguraEntrada;
        ControlParental = controlParental;
        PlantillaId = plantillaId;
        PlantillaDigitalId = plantillaDigitalId;

        AsignarMovil(esParaMovil, mostrarTramos);
        AsignarBuses(esParaBuses, idaVuelta);
        AsignarPoliticas(politicas);
        AsignarServicioResponsable(controlParental, servicioResponsableId);

        return this;
    }

    private void AsignarServicioResponsable(bool controlParental, int? servicioResponsableId)
    {
        if (controlParental && servicioResponsableId is not null)
        {
            if (_servicioControlParental is null)
            {
                _servicioControlParental = ServicioControlParental.Create(servicioResponsableId.Value);
                return;
            }

            _servicioControlParental.Update(servicioResponsableId.Value);
            return;
        }

        _servicioControlParental = null;
    }

    private void AsignarMovil(bool esParaMovil, bool? mostrarTramos)
    {
        if (esParaMovil)
        {
            if (Movil is null)
            {
                Movil = ServicioMovil.Crear(mostrarTramos ?? false);
                return;
            }

            Movil.Update(mostrarTramos ?? false);
            return;
        }

        Movil = null;
    }

    private void AsignarBuses(bool esParaBuses, TipoServicio? idaVuelta)
    {
        if (esParaBuses)
        {
            if (Bus is null)
            {
                Bus = ServicioBus.Create(idaVuelta ?? TipoServicio.SoloIda);
                return;
            }

            Bus.Update(idaVuelta ?? TipoServicio.SoloIda);
            return;
        }

        Bus = null;
    }

    private void AsignarPoliticas(string? politicas)
    {
        if (politicas is not null)
        {
            if (_servicioPolitica is null)
            {
                _servicioPolitica = ServicioPolitica.Create(politicas);
                return;
            }

            _servicioPolitica.Update(politicas);
            return;
        }

        _servicioPolitica = null;
    }

    private static void ValidarHolgura(TimeSpan? holguraInicio, TimeSpan? holguraFin, List<ValidationError> errors)
    {
        if (holguraInicio is not null && holguraFin is not null && holguraInicio.Value > holguraFin.Value)
        {
            errors.Add(new ValidationError(nameof(HolguraFin), "La holgura de fin debe ser mayor o igual a la holura de inicio."));
        }
    }

    private static void ValidarPax(int numeroPaxMinimo, int numeroPaxMaximo, List<ValidationError> errors)
    {
        if (numeroPaxMinimo > numeroPaxMaximo)
        {
            errors.Add(new ValidationError(nameof(NumeroPaxMaximo), "La cantidad máxima de pasajeros debe ser mayor o igual a la cantidad mínima de pasajeros."));
        }
    }

    internal ResultOf<Servicio> AssignCajas(IEnumerable<(ServicioPorCajaInfo Info, Caja Caja)> cajas)
    {
        List<(ServicioPorCajaInfo Info, Caja Caja)> toAdd = (from caja in cajas
                                                             join persisted in _serviciosPorCaja on caja.Caja.Id equals persisted.Caja.Id into serviciosPorCaja
                                                             from servicioPorCaja in serviciosPorCaja.DefaultIfEmpty()
                                                             where servicioPorCaja is null
                                                             select caja).ToList();

        List<ServicioPorCaja> toRemove = (from servicioPorCaja in _serviciosPorCaja
                                          join caja in cajas on servicioPorCaja.Caja.Id equals caja.Caja.Id into defCajas
                                          from defCaja in defCajas.DefaultIfEmpty()
                                          where defCaja.Caja is null
                                          select servicioPorCaja).ToList();

        List<(ServicioPorCaja ServicioPorCaja, bool NoUsaZona)> toUpdate = (from servicioPorCaja in _serviciosPorCaja
                                                                            join caja in cajas on servicioPorCaja.Caja.Id equals caja.Caja.Id
                                                                            select (servicioPorCaja, caja.Info.NoUsaZona)).ToList();

        toAdd.ForEach(AddCaja);
        toRemove.ForEach(RemoveCaja);
        toUpdate.ForEach(UpdateCaja);

        return this;
    }

    private void AddCaja((ServicioPorCajaInfo Info, Caja Caja) caja) =>
        _serviciosPorCaja.Add(ServicioPorCaja.Create(caja.Caja, caja.Info.NoUsaZona));

    private void RemoveCaja(ServicioPorCaja caja) =>
        _serviciosPorCaja.Remove(caja);

    private static void UpdateCaja((ServicioPorCaja ServicioPorCaja, bool NoUsaZona) toUpdate) =>
        toUpdate.ServicioPorCaja.Update(toUpdate.NoUsaZona);

    internal ResultOf<Servicio> AssignCentrosCosto(IEnumerable<(CentroCostoServicioInfo Info, CentroCosto CentroCosto)> centrosCostoInfo)
    {
        List<(CentroCostoServicioInfo Info, CentroCosto CentroCosto)> toAdd = (from info in centrosCostoInfo
                                                                               join persisted in _centrosCosto on info.CentroCosto.Id equals persisted.CentroCosto.Id into serviciosPorCentroCosto
                                                                               from servicioPorCentroCosto in serviciosPorCentroCosto.DefaultIfEmpty()
                                                                               where servicioPorCentroCosto is null
                                                                               select info).ToList();

        List<CentroCostoServicio> toRemove = (from servicioPorCentroCosto in _centrosCosto
                                              join info in centrosCostoInfo on servicioPorCentroCosto.CentroCosto.Id equals info.CentroCosto.Id into defCentrosCosto
                                              from defCentroCosto in defCentrosCosto.DefaultIfEmpty()
                                              where defCentroCosto.CentroCosto is null
                                              select servicioPorCentroCosto).ToList();

        List<(CentroCostoServicio CentroCosto, string? Nombre)> toUpdate = (from servicioPorCentroCosto in _centrosCosto
                                                                            join info in centrosCostoInfo on servicioPorCentroCosto.CentroCosto.Id equals info.CentroCosto.Id
                                                                            select (servicioPorCentroCosto, info.Info.Nombre)).ToList();

        toAdd.ForEach(AddCentroCosto);
        toRemove.ForEach(RemoveCentroCosto);
        toUpdate.ForEach(UpdateCentroCosto);

        return this;
    }

    private void AddCentroCosto((CentroCostoServicioInfo Info, CentroCosto CentroCosto) info) =>
        _centrosCosto.Add(CentroCostoServicio.Create(info.CentroCosto, info.Info.Nombre));

    private void RemoveCentroCosto(CentroCostoServicio centroCostoServicio) =>
        _centrosCosto.Remove(centroCostoServicio);

    private static void UpdateCentroCosto((CentroCostoServicio CentroCosto, string? Nombre) info) =>
        info.CentroCosto.Update(info.Nombre);

    internal ResultOf<Servicio> AssignTramos(IEnumerable<(TramoServicioInfo Info, CentroCosto CentroCosto, Tramo Tramo)> tramosInfo)
    {
        var notCentrosCosto = (from info in tramosInfo
                               join centroCosto in _centrosCosto on info.CentroCosto.Id equals centroCosto.CentroCosto.Id into defCentrosCosto
                               from defCentroCosto in defCentrosCosto.DefaultIfEmpty()
                               where defCentroCosto is null
                               select info.CentroCosto).ToList();
        if (notCentrosCosto.Count != 0)
        {
            string centroCostos = string.Join(", ", notCentrosCosto.Select(x => x.Descripcion));
            return new ValidationError("Tramos", $"Debe asignar los siguientes centros de costo antes de poder asociar tramos: {centroCostos}.");
        }

        List<(TramoServicioInfo Info, CentroCosto CentroCosto, Tramo Tramo)> toAdd = (from info in tramosInfo
                                                                                      join persisted in _tramosServicio on info.Info.Id equals persisted.Id into serviciosTramos
                                                                                      from servicioTramo in serviciosTramos.DefaultIfEmpty()
                                                                                      where servicioTramo is null
                                                                                      select info).ToList();

        List<TramoServicio> toRemove = (from servicioTramo in _tramosServicio
                                        join info in tramosInfo on servicioTramo.Id equals info.Info.Id into defTramos
                                        from defTramo in defTramos.DefaultIfEmpty()
                                        where defTramo.CentroCosto is null
                                        select servicioTramo).ToList();

        List<(TramoServicio TramoServicio, TramoServicioInfo Info)> toUpdate = (from servicioTramo in _tramosServicio
                                                                                join info in tramosInfo on servicioTramo.Id equals info.Info.Id
                                                                                select (servicioTramo, info.Info)).ToList();

        toAdd.ForEach(AddTramo);
        toRemove.ForEach(RemoveTramo);
        toUpdate.ForEach(UpdateTramo);

        return this;
    }

    private void AddTramo((TramoServicioInfo Info, CentroCosto CentroCosto, Tramo Tramo) info) =>
        _tramosServicio.Add(TramoServicio.Create(info.CentroCosto, info.Tramo, info.Info.Nombre, info.Info.CantidadPermisos));

    private void RemoveTramo(TramoServicio tramoServicio) =>
        _tramosServicio.Remove(tramoServicio);

    private static void UpdateTramo((TramoServicio TramoServicio, TramoServicioInfo Info) info) =>
        info.TramoServicio.Update(info.Info.Nombre, info.Info.CantidadPermisos);

    internal ResultOf<Servicio> AssignPermisos(IEnumerable<(PermisoServicioInfo Info, Tramo Tramo, CentroCosto CentroCosto)> permisosInfo)
    {
        List<(PermisoServicioInfo Info, Tramo Tramo, CentroCosto CentroCosto)> toAdd = (from info in permisosInfo
                                                                                        join persisted in _permisos on info.Info.Id equals persisted.Id into permisos
                                                                                        from permiso in permisos.DefaultIfEmpty()
                                                                                        where permiso is null
                                                                                        select info).ToList();

        List<PermisoServicio> toRemove = (from permiso in _permisos
                                          join info in permisosInfo on permiso.Id equals info.Info.Id into defPermisos
                                          from defPermiso in defPermisos.DefaultIfEmpty()
                                          where defPermiso.Tramo is null
                                          select permiso).ToList();

        toAdd.ForEach(AddPermiso);
        toRemove.ForEach(RemovePermiso);

        return this;
    }

    private void AddPermiso((PermisoServicioInfo Info, Tramo Tramo, CentroCosto CentroCosto) info) =>
        _permisos.Add(PermisoServicio.Create(info.Tramo, info.CentroCosto));

    private void RemovePermiso(PermisoServicio permisoServicio) =>
        _permisos.Remove(permisoServicio);

    internal ResultOf<Servicio> AssignGruposEtarios(IEnumerable<GrupoEtario> gruposEtariosInfo)
    {
        List<GrupoEtario> toAdd = (from info in gruposEtariosInfo
                                   join persisted in _gruposEtarios on info.Id equals persisted.Id into gruposEtarios
                                   from grupoEtario in gruposEtarios.DefaultIfEmpty()
                                   where grupoEtario is null
                                   select info).ToList();

        List<GrupoEtario> toRemove = (from persisted in _gruposEtarios
                                      join info in gruposEtariosInfo on persisted.Id equals info.Id into gruposEtarios
                                      from grupoEtario in gruposEtarios.DefaultIfEmpty()
                                      where grupoEtario is null
                                      select persisted).ToList();

        toAdd.ForEach(AddGrupoEtario);
        toRemove.ForEach(RemoveGrupoEtario);

        return this;
    }

    private void AddGrupoEtario(GrupoEtario grupoEtario) =>
        _gruposEtarios.Add(grupoEtario);

    private void RemoveGrupoEtario(GrupoEtario grupoEtario) =>
        _gruposEtarios.Remove(grupoEtario);

    internal void Activar() =>
        EsActivo = true;

    internal void Desactivar() =>
        EsActivo = false;

    /// <summary>
    /// Consigue el nombre dado el tramo.
    /// </summary>
    /// <param name="tramoId">El id del tramo para buscar el nombre.</param>
    /// <param name="centroCostoId">El id del centro de costo para buscar el nombre</param>
    /// <returns>El nombre del servicio asociado al tramo, o si no existe el nombre del servicio.</returns>
    public string NombrePorTramo(int tramoId, int centroCostoId) =>
        _tramosServicio.Find(x => x.Tramo.Id == tramoId && x.CentroCosto.Id == centroCostoId)?.Nombre ?? Nombre;

    /// <summary>
    /// Consigue el nombre dado el tramo.
    /// </summary>
    /// <param name="tramoId">El id del tramo para buscar el nombre.</param>
    /// <returns>El nombre del servicio asociado al tramo, o si no existe el nombre del servicio.</returns>
    public string NombrePorTramo(int tramoId) =>
        NombrePorTramo(tramoId, CentroCostoId ?? 0);
}
