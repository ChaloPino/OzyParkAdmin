using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.EscenariosCupo.Update;

/// <summary>
/// Actualiza un escenario cupo
/// </summary>
/// <param name="Id"></param>
/// <param name="CentroCosto">El centro de costo.</param>
/// <param name="ZonaInfo">La zona.</param>
/// <param name="Detalles">Detalles Escenario Cupo</param>
/// <param name="Nombre">El nombre del escenario.</param>
/// <param name="EsHoraInicio">Si es hora de inicio.</param>
/// <param name="MinutosAntes">Minutos antes.</param>
/// <param name="EsActivo">Si estará activo.</param>
public sealed record UpdateEscenarioCupo(
    int Id,
    string Nombre,
    bool EsHoraInicio,
    int MinutosAntes,
    bool EsActivo,
    CentroCostoInfo CentroCosto,
    ZonaInfo? ZonaInfo,
    IEnumerable<DetalleEscenarioCupoInfo> Detalles,
    IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> ExclusionesFecha,
    IEnumerable<DetalleEscenarioCupoExclusionFullInfo> Exclusiones) : ICommand<EscenarioCupoFullInfo>;
