using MassTransit.Internals;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Components.Admin.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El view-model del servicio.
/// </summary>
public sealed record ServicioViewModel
{
    /// <summary>
    /// El id del servicio.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El id de franquicia.
    /// </summary>
    public int FranquiciaId { get; set; }

    /// <summary>
    /// El centro de costo asociado al servicio.
    /// </summary>
    public CentroCostoModel CentroCosto { get; set; } = default!;

    /// <summary>
    /// El aka del servicio.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del servicio.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// El número de vigencia del servicio.
    /// </summary>
    public short NumeroVigencia { get; set; } = 0;

    /// <summary>
    /// El número de validez del servicio.
    /// </summary>
    public short NumeroValidez { get; set; } = 0;

    /// <summary>
    /// Número mínimo de pasajeros.
    /// </summary>
    public short NumeroPaxMinimo { get; set; } = 1;

    /// <summary>
    /// Número máximo de pasajeros.
    /// </summary>
    public short NumeroPaxMaximo { get; set; } = 1;

    /// <summary>
    /// El tipo de control del servicio.
    /// </summary>
    public TipoControlModel TipoControl { get; set; } = default!;

    /// <summary>
    /// El tipo de distribución del servicio.
    /// </summary>
    public TipoDistribucionModel TipoDistribucion { get; set; } = default!;

    /// <summary>
    /// El tipo de vigencia del servicio.
    /// </summary>
    public TipoVigenciaModel TipoVigencia { get; set; } = default!;

    /// <summary>
    /// Si el servicio se controla con horas.
    /// </summary>
    public bool EsConHora { get; set; }

    /// <summary>
    /// Si el servicio tiene tramos.
    /// </summary>
    public bool EsPorTramos { get; set; }

    /// <summary>
    /// Si el servicio es para venta.
    /// </summary>
    public bool EsParaVenta { get; set; }

    /// <summary>
    /// El orden en que se presenta el servicio.
    /// </summary>
    public int Orden { get; set; } = 1;

    /// <summary>
    /// La holgura de inicio.
    /// </summary>
    public TimeSpan? HolguraInicio { get; set; }

    /// <summary>
    /// La holgura de fin.
    /// </summary>
    public TimeSpan? HolguraFin { get; set; }

    /// <summary>
    /// Si es un servicio para móviles.
    /// </summary>
    public bool EsParaMovil { get; set; }

    /// <summary>
    /// Mostrar los tramos para los móviles.
    /// </summary>
    public bool? MostrarTramos { get; set; }

    /// <summary>
    /// Si es un servicio para buses.
    /// </summary>
    public bool EsParaBuses { get; set; }

    /// <summary>
    /// El tipo de servicio para buses.
    /// </summary>
    public TipoServicio? IdaVuelta { get; set; }

    /// <summary>
    /// La holgura de entrada en minutos.
    /// </summary>
    public byte HolguraEntrada { get; set; } = 0;

    /// <summary>
    /// Las políticas de uso del servicio.
    /// </summary>
    public string? Politicas { get; set; }

    /// <summary>
    /// Si el servicio tiene control parental.
    /// </summary>
    public bool ControlParental { get; set; }

    /// <summary>
    /// El servicio responsable cuando tiene control parental.
    /// </summary>
    public int? ServicioResponsableId { get; internal set; }

    /// <summary>
    /// Si el servicio está activo.
    /// </summary>
    public bool EsActivo { get; set; }

    /// <summary>
    /// El tipo del servicio.
    /// </summary>
    public TipoServicio TipoServicio { get; set; } = TipoServicio.SoloIda;

    /// <summary>
    /// La plantilla asociada al servicio.
    /// </summary>
    public int PlantillaId { get; set; }

    /// <summary>
    /// La plantilla digital asociada al servicio.
    /// </summary>
    public int PlantillaDigitalId { get; set; }

    /// <summary>
    /// Los tramos asociados al servicio.
    /// </summary>
    public List<TramoServicioModel> Tramos { get; set; } = [];

    /// <summary>
    /// Las zonas por tramo asociadas al servicio.
    /// </summary>
    public List<ZonaTramoModel> Zonas { get; set; } = [];

    /// <summary>
    /// Los centros de costo asociados al servicio para reemplazar el nombre del servicio.
    /// </summary>
    public List<CentroCostoServicioModel> CentrosCosto { get; set; } = [];

    /// <summary>
    /// Los grupos etarios que pueden adquirir este servicio.
    /// </summary>
    public List<GrupoEtarioModel> GruposEtarios { get; set; } = [];

    /// <summary>
    /// Las cajas que tienen permiso de vender este servicio.
    /// </summary>
    public List<CajaServicioModel> Cajas { get; set; } = [];

    /// <summary>
    /// Los permisos que tiene este servicio.
    /// </summary>
    public List<PermisoServicioModel> Permisos { get; set; } = [];

    /// <summary>
    /// Si el servicio en nuevo.
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// Si se cargó el detalle del servicio.
    /// </summary>
    public bool DetailLoaded { get; set; }

    internal string Vigencia => NumeroVigencia != 1 ? $"{NumeroVigencia} {TipoVigencia}s" : $"{NumeroVigencia} {TipoVigencia}";

    internal string Validez => NumeroValidez != 1 ? $"{NumeroValidez} días" : $"{NumeroValidez} día";

    internal void Update(ServicioFullInfo servicio)
    {
        if (IsNew)
        {
            Id = servicio.Id;
            FranquiciaId = servicio.FranquiciaId;
            CentroCosto = servicio.CentroCosto.ToModel();
            IsNew = false;
        }

        Aka = servicio.Aka;
        Nombre = servicio.Nombre;
        TipoControl = servicio.TipoControl.ToModel();
        TipoDistribucion = servicio.TipoDistribucion.ToModel();
        TipoServicio = servicio.TipoServicio;
        TipoVigencia = servicio.TipoVigencia.ToModel();
        NumeroVigencia = servicio.NumeroVigencia;
        NumeroValidez = servicio.NumeroValidez;
        NumeroPaxMinimo = servicio.NumeroPaxMinimo;
        NumeroPaxMaximo = servicio.NumeroPaxMaximo;
        EsConHora = servicio.EsConHora;
        EsPorTramos = servicio.EsPorTramos;
        EsParaVenta = servicio.EsParaVenta;
        Orden = servicio.Orden;
        HolguraInicio = servicio.HolguraInicio;
        HolguraFin = servicio.HolguraFin;
        EsParaMovil = servicio.EsParaMovil;
        MostrarTramos = servicio.MostrarTramos;
        EsParaBuses = servicio.EsParaBuses;
        IdaVuelta = servicio.IdaVuelta;
        HolguraEntrada = servicio.HolguraEntrada;
        Politicas = servicio.Politicas;
        ControlParental = servicio.ControlParental;
        ServicioResponsableId = servicio.ServicioResponsableId;
        Tramos = servicio.Tramos.ToModel();
        CentrosCosto = servicio.CentrosCosto.ToModel();
        GruposEtarios = servicio.GruposEtarios.ToModel();
        Cajas = servicio.Cajas.ToModel();
        Zonas = servicio.Zonas.ToModel();
        Permisos = servicio.Permisos.ToModel();
        PlantillaId = servicio.PlantillaId;
        PlantillaDigitalId = servicio.PlantillaDigitalId;
        EsActivo = servicio.EsActivo;
    }

    internal void Update(ServicioViewModel servicio)
    {
        Id = servicio.Id;
        FranquiciaId = servicio.FranquiciaId;
        CentroCosto = servicio.CentroCosto;
        IsNew = servicio.IsNew;
        Aka = servicio.Aka;
        Nombre = servicio.Nombre;
        TipoControl = servicio.TipoControl;
        TipoDistribucion = servicio.TipoDistribucion;
        TipoServicio = servicio.TipoServicio;
        TipoVigencia = servicio.TipoVigencia;
        NumeroVigencia = servicio.NumeroVigencia;
        NumeroValidez = servicio.NumeroValidez;
        NumeroPaxMinimo = servicio.NumeroPaxMinimo;
        NumeroPaxMaximo = servicio.NumeroPaxMaximo;
        EsConHora = servicio.EsConHora;
        EsPorTramos = servicio.EsPorTramos;
        EsParaVenta = servicio.EsParaVenta;
        Orden = servicio.Orden;
        HolguraInicio = servicio.HolguraInicio;
        HolguraFin = servicio.HolguraFin;
        EsParaMovil = servicio.EsParaMovil;
        MostrarTramos = servicio.MostrarTramos;
        EsParaBuses = servicio.EsParaBuses;
        IdaVuelta = servicio.IdaVuelta;
        HolguraEntrada = servicio.HolguraEntrada;
        Politicas = servicio.Politicas;
        ControlParental = servicio.ControlParental;
        ServicioResponsableId = servicio.ServicioResponsableId;
        Tramos = servicio.Tramos;
        CentrosCosto = servicio.CentrosCosto;
        GruposEtarios = servicio.GruposEtarios;
        Cajas = servicio.Cajas;
        Zonas = servicio.Zonas;
        Permisos = servicio.Permisos;
        PlantillaId = servicio.PlantillaId;
        PlantillaDigitalId = servicio.PlantillaDigitalId;
        EsActivo = servicio.EsActivo;
    }

    internal ServicioViewModel Copy() =>
        new()
        {
            Id = Id,
            FranquiciaId = FranquiciaId,
            CentroCosto = CentroCosto,
            IsNew = IsNew,
            Aka = Aka,
            Nombre = Nombre,
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
            EsParaMovil = EsParaMovil,
            MostrarTramos = MostrarTramos,
            EsParaBuses = EsParaBuses,
            IdaVuelta = IdaVuelta,
            HolguraEntrada = HolguraEntrada,
            Politicas = Politicas,
            ControlParental = ControlParental,
            ServicioResponsableId = ServicioResponsableId,
            Tramos = Tramos,
            CentrosCosto = CentrosCosto,
            GruposEtarios = GruposEtarios,
            Cajas = Cajas,
            Zonas = Zonas,
            Permisos = Permisos,
            PlantillaId = PlantillaId,
            PlantillaDigitalId = PlantillaDigitalId,
            EsActivo = EsActivo,
        };
}
