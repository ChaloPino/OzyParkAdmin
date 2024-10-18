using Microsoft.Data.SqlClient;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;

/// <summary>
/// El modelo para crear varios cupos.
/// </summary>
public sealed record CuposModel
{
    /// <summary>
    /// La fecha efectiva de los cupos a crear.
    /// </summary>
    public DateOnly FechaEfectiva { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// El escenario de los cupos a crear.
    /// </summary>
    public EscenarioCupoInfo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// La cantidad total de los cupos a crear.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// El sobrecupo de los cupos a crear.
    /// </summary>
    public int SobreCupo { get; set; }

    /// <summary>
    /// El tope en los cupos que se van a crear.
    /// </summary>
    public int TopeEnCupo { get; set; }

    /// <summary>
    /// Los canales de venta asociados a los cupos a crear.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Los días de semana asociados a los cupos a crear.
    /// </summary>
    public IEnumerable<DiaSemana> DiasSemana { get; set; } = [];

    /// <summary>
    /// La hora de inicio de todos los cupos a crear.
    /// </summary>
    public TimeSpan? HoraInicio { get; set; }

    /// <summary>
    /// La hora de término de todos los cupos a crear.
    /// </summary>
    public TimeSpan? HoraTermino { get; set; }

    /// <summary>
    /// El intervalo en minutos de los cupos a crear.
    /// </summary>
    public int IntervaloMinutos { get; set; } = 20;

    /// <summary>
    /// La fecha efectiva convertida en <see cref="DateTime"/>.
    /// </summary>
    public DateTime? FechaEfectivaDate
    {
        get => FechaEfectiva.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value is not null)
            {
                FechaEfectiva = DateOnly.FromDateTime(value.Value);
            }
        }
    }

    internal CanalVenta? ValidationCanalVenta => CanalesVenta.FirstOrDefault();

    internal DiaSemana? ValidationDiaSemana => DiasSemana.FirstOrDefault();

    internal bool Loading { get; set; }
}
