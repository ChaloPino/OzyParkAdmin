using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.Update;

/// <summary>
/// Información para actualizar un servicio.
/// </summary>
/// <param name="Id">El id del servicio.</param>
/// <param name="CentroCosto">El centro de costo.</param>
/// <param name="FranquiciaId">El id de franquicia.</param>
/// <param name="Aka">El aka del servicio.</param>
/// <param name="Nombre">El nombre del servicio.</param>
/// <param name="TipoControl">El tipo de control.</param>
/// <param name="TipoDistribucion">El tipo de distribución.</param>
/// <param name="TipoServicio">El tipo de servicio.</param>
/// <param name="TipoVigencia">El tipo de vigencia.</param>
/// <param name="NumeroVigencia">El número de vigencia.</param>
/// <param name="NumeroValidez">El número de validez.</param>
/// <param name="NumeroPaxMinimo">La cantidad mínima de pasajeros.</param>
/// <param name="NumeroPaxMaximo">La cantidad máxima de pasajeros.</param>
/// <param name="EsConHora">Si es con hora.</param>
/// <param name="EsPorTramos">Si es por tramos.</param>
/// <param name="EsParaVenta">Si es para venta.</param>
/// <param name="Orden">El orden.</param>
/// <param name="HolguraInicio">La holgura de inicio.</param>
/// <param name="HolguraFin">La holgura de fin.</param>
/// <param name="EsParaMovil">Si es para dispositivos móviles.</param>
/// <param name="MostrarTramos">Si se muestra los tramos.</param>
/// <param name="EsParaBuses">Si es para buses.</param>
/// <param name="IdaVuelta">El tipo de servicio para buses.</param>
/// <param name="HolguraEntrada">La holgura de entrada.</param>
/// <param name="ControlParental">Si tiene control parental.</param>
/// <param name="ServicioResponsableId">El servicio responsable cuando tiene control parental.</param>
/// <param name="Politicas">La políticas de uso.</param>
/// <param name="PlantillaId">La plantilla asociada al servicio.</param>
/// <param name="PlantillaDigitalId">La plantilla digital asociada al servicio.</param>
public sealed record UpdateServicio(
    int Id,
    CentroCostoInfo CentroCosto,
    int FranquiciaId,
    string Aka,
    string Nombre,
    TipoControl TipoControl,
    TipoDistribucion TipoDistribucion,
    TipoServicio TipoServicio,
    TipoVigencia TipoVigencia,
    short NumeroVigencia,
    short NumeroValidez,
    short NumeroPaxMinimo,
    short NumeroPaxMaximo,
    bool EsConHora,
    bool EsPorTramos,
    bool EsParaVenta,
    int Orden,
    TimeSpan? HolguraInicio,
    TimeSpan? HolguraFin,
    bool EsParaMovil,
    bool? MostrarTramos,
    bool EsParaBuses,
    TipoServicio? IdaVuelta,
    byte HolguraEntrada,
    bool ControlParental,
    int? ServicioResponsableId,
    string? Politicas,
    int PlantillaId,
    int PlantillaDigitalId) : IServicioStateChangeable;
