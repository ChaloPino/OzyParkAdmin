using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Domain.CuposFecha;

/// <summary>
/// Contiene toda la información de un cupo por fecha.
/// </summary>
public sealed record CupoFechaFullInfo
{
    /// <summary>
    /// El id del cupo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// La fecha del cupo.
    /// </summary>
    public DateOnly Fecha { get; set; }

    /// <summary>
    /// El escenario del cupo por fecha.
    /// </summary>
    public EscenarioCupoInfo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// El canal de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// El día de semana.
    /// </summary>
    public DiaSemana DiaSemana { get; set; } = default!;

    /// <summary>
    /// La hora de inicio.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// La hora de fin.
    /// </summary>
    public TimeSpan HoraFin { get; set; }

    /// <summary>
    /// El total del cupo por fecha.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// El sobrecupo.
    /// </summary>
    public int SobreCupo { get; set; }

    /// <summary>
    /// El tope que se tiene en el cupo por fecha.
    /// </summary>
    public int TopeEnCupo { get; set; }
}
