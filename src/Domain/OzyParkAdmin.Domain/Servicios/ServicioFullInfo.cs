using OzyParkAdmin.Domain.CentrosCosto;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información de un servicio.
/// </summary>
public sealed record ServicioFullInfo
{
    /// <summary>
    /// El id del servicio.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka del servicio.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// El nombre del servicio.
    /// </summary>
    public string Nombre { get; init; } = string.Empty;

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int FranquiciaId { get; init; }

    /// <summary>
    /// El centro de costo del servicio.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; init; } = default!;

    /// <summary>
    /// El tipo de control del servicio.
    /// </summary>
    public TipoControl TipoControl { get; init; } = default!;

    /// <summary>
    /// El tipo de distribución del servicio.
    /// </summary>
    public TipoDistribucion TipoDistribucion { get; init; } = default!;

    /// <summary>
    /// El tipo del servicio.
    /// </summary>
    public TipoServicio TipoServicio { get; init; }

    /// <summary>
    /// El tipo de vigencia del servicio.
    /// </summary>
    public TipoVigencia TipoVigencia { get; init; } = default!;

    /// <summary>
    /// El número de vigencia.
    /// </summary>
    public short NumeroVigencia { get; init; }

    /// <summary>
    /// El número de validez.
    /// </summary>
    public short NumeroValidez { get; init; }

    /// <summary>
    /// El número mínimo de pasajeros.
    /// </summary>
    public short NumeroPaxMinimo { get; init; }

    /// <summary>
    /// El número máximo de pasajeros.
    /// </summary>
    public short NumeroPaxMaximo { get; init; }

    /// <summary>
    /// Si el servicio es con hora.
    /// </summary>
    public bool EsConHora { get; init; }

    /// <summary>
    /// Si el servicio es por tramos.
    /// </summary>
    public bool EsPorTramos { get; init; }

    /// <summary>
    /// Si el servicio es para la venta.
    /// </summary>
    public bool EsParaVenta { get; init; }

    /// <summary>
    /// El orden del servicio.
    /// </summary>
    public int Orden { get; init; }

    /// <summary>
    /// La holgura de inicio.
    /// </summary>
    public TimeSpan? HolguraInicio { get; init; }

    /// <summary>
    /// La holgura de fin.
    /// </summary>
    public TimeSpan? HolguraFin { get; init; }

    /// <summary>
    /// La plantilla asociada al servicio.
    /// </summary>
    public int PlantillaId { get; init; }

    /// <summary>
    /// La plantilla digital asociada al servicio.
    /// </summary>
    public int PlantillaDigitalId { get; init; }

    /// <summary>
    /// Los tramos asociados al servicio.
    /// </summary>
    public ImmutableArray<TramoServicioInfo> Tramos { get; set; } = [];

    /// <summary>
    /// Los centros de costo asociados al servicio.
    /// </summary>
    public ImmutableArray<CentroCostoServicioInfo> CentrosCosto { get; set; } = [];

    /// <summary>
    /// Los grupos etarios asociados al servicio.
    /// </summary>
    public ImmutableArray<GrupoEtarioInfo> GruposEtarios { get; set; } = [];

    /// <summary>
    /// Las cajas que pueden vender el servicio.
    /// </summary>
    public ImmutableArray<ServicioPorCajaInfo> Cajas { get; set; } = [];

    /// <summary>
    /// Los permisos por tramo y centro de costo del servicio.
    /// </summary>
    public ImmutableArray<PermisoServicioInfo> Permisos { get; set; } = [];

    /// <summary>
    /// Las zonas asociadas al servicio.
    /// </summary>
    public ImmutableArray<ZonaPorTramoInfo> Zonas { get; set; } = [];

    /// <summary>
    /// Si el servicio está activo.
    /// </summary>
    public bool EsActivo { get; init; }

    /// <summary>
    /// Si es un servicio para móviles.
    /// </summary>
    public bool EsParaMovil { get; init; }

    /// <summary>
    /// Si se muestra los tramos.
    /// </summary>
    public bool? MostrarTramos { get; init; }

    /// <summary>
    /// Si es un servicio para buses.
    /// </summary>
    public bool EsParaBuses { get; init; }

    /// <summary>
    /// El tipo de servicio para buses.
    /// </summary>
    public TipoServicio? IdaVuelta { get; init; }

    /// <summary>
    /// La holgura de entrada en minutos.
    /// </summary>
    public byte HolguraEntrada { get; init; }

    /// <summary>
    /// Si tiene control parental.
    /// </summary>
    public bool ControlParental { get; init; }

    /// <summary>
    /// El servicio responsable para los servicios con control parental.
    /// </summary>
    /// <remarks>
    /// No es necesario que un servicio con control parental tenga un servicio responsable.
    /// </remarks>
    public int? ServicioResponsableId { get; init; }

    /// <summary>
    /// Las políticas de uso del servicio.
    /// </summary>
    public string? Politicas { get; init; }
}
