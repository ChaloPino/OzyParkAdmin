using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// Contiene toda la información de un cupo.
/// </summary>
public sealed record CupoFullInfo
{
    /// <summary>
    /// El id del cupo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El escenario del cupo.
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
    /// El total del cupo.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// El sobrecupo.
    /// </summary>
    public int SobreCupo { get; set; }

    /// <summary>
    /// El tope que se tiene en el cupo.
    /// </summary>
    public int TopeEnCupo { get; set; }

    /// <summary>
    /// La fecha efectiva del cupo.
    /// </summary>
    public DateOnly FechaEfectiva { get; set; }

    /// <summary>
    /// La última fecha de modificación del cupo.
    /// </summary>
    public DateTime UltimaModificacion { get; set; }
}
