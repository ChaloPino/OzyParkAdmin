using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Application.CuposFecha.Update;

/// <summary>
/// Actualiza varios cupos por fecha.
/// </summary>
/// <param name="Fecha">La fecha de todos los cupos por fecha a actualizar.</param>
/// <param name="EscenarioCupo">El escenario de cupo de los cupos por fecha.</param>
/// <param name="CanalVenta">El canal de venta de los cupos por fecha.</param>
/// <param name="DiaSemana">El día de semana de los cupos por fecha.</param>
/// <param name="Total">El total de los cupos por fecha a actualizar.</param>
/// <param name="Sobrecupo">El sobrecupo a actualizar.</param>
/// <param name="TopeEnCupo">El tope en tramo por fecha a actualizar.</param>
public sealed record UpdateCuposFecha(
    DateOnly Fecha,
    EscenarioCupoInfo EscenarioCupo,
    CanalVenta CanalVenta,
    DiaSemana DiaSemana,
    int Total,
    int Sobrecupo,
    int TopeEnCupo) : ICommand;
