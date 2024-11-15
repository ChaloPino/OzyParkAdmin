using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.EscenariosCupo.Create;

/// <summary>
/// Crea varias fechas de exclusión para los cupos.
/// </summary>
/// <param name="CentroCosto">El centro de costo.</param>
/// <param name="ZonaInfo">La zona.</param>
/// <param name="Nombre">El nombre del escenario.</param>
/// <param name="EsHoraInicio">Si es hora de inicio.</param>
/// <param name="MinutosAntes">Minutos antes.</param>
/// <param name="EsActivo">Si estará activo.</param>
public sealed record CreateEscenarioCupo(
    CentroCostoInfo CentroCosto,
    ZonaInfo ZonaInfo,
    IEnumerable<DetalleEscenarioCupoInfo> Detalles,
    string Nombre,
    bool EsHoraInicio,
    int MinutosAntes,
    bool EsActivo) : ICommand;
