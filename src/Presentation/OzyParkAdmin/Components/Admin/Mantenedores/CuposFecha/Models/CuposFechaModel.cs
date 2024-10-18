using MudBlazor;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;

/// <summary>
/// El modelo para crear varios cupos por fecha.
/// </summary>
public sealed class CuposFechaModel
{
    /// <summary>
    /// El rango de fechas usado para generar los cupos por fecha.
    /// </summary>
    public DateRange RangoFechas { get; set; } = new DateRange(DateTime.Today, DateTime.Today);

    /// <summary>
    /// El escenario de los cupos por fecha a crear.
    /// </summary>
    public EscenarioCupoInfo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// La cantidad total de los cupos por fecha a crear.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// El sobrecupo de los cupos por fecha a crear.
    /// </summary>
    public int SobreCupo { get; set; }

    /// <summary>
    /// El tope en los cupos por fecha que se van a crear.
    /// </summary>
    public int TopeEnCupo { get; set; }

    /// <summary>
    /// Los canales de venta asociados a los cupos por fecha a crear.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Los días de semana asociados a los cupos por fecha a crear.
    /// </summary>
    public IEnumerable<DiaSemana> DiasSemana { get; set; } = [];

    /// <summary>
    /// La hora de inicio de todos los cupos por fecha a crear.
    /// </summary>
    public TimeSpan? HoraInicio { get; set; }

    /// <summary>
    /// La hora de término de todos los cupos por fecha a crear.
    /// </summary>
    public TimeSpan? HoraTermino { get; set; }

    /// <summary>
    /// El intervalo en minutos de los cupos por fecha a crear.
    /// </summary>
    public int IntervaloMinutos { get; set; } = 20;

    internal CanalVenta? CanalVenta => CanalesVenta.FirstOrDefault();

    internal DiaSemana? DiaSemana => DiasSemana.FirstOrDefault();

    internal bool Loading { get; set; }
}
