using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;

/// <summary>
/// El view model de cupo por fecha.
/// </summary>
public sealed record CupoFechaViewModel
{
    /// <summary>
    /// El id del cupo por fecha.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// La fecha del cupo.
    /// </summary>
    public DateOnly Fecha { get; set; }

    /// <summary>
    /// El escenario de cupo asociado al cupo por fecha.
    /// </summary>
    public EscenarioCupoInfo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// El canal de venta del cupo por fecha.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// El día de semana del cupo por fecha.
    /// </summary>
    public DiaSemana DiaSemana { get; set; } = default!;

    /// <summary>
    /// La hora de inicio del cupo por fecha.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// La hora de fin del cupo por fecha.
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

    internal DateTime? FechaDate
    {
        get => Fecha.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value is not null)
            {
                Fecha = DateOnly.FromDateTime(value.Value);
            }
        }
    }

    internal TimeSpan? HoraInicioTime
    {
        get => HoraInicio;
        set
        {
            if (value is not null)
            {
                HoraInicio = value.Value;
            }
        }
    }

    internal TimeSpan? HoraFinTime
    {
        get => HoraFin;
        set
        {
            if (value is not null)
            {
                HoraFin = value.Value;
            }
        }
    }

    /// <summary>
    /// Si se está cargando el cupo.
    /// </summary>
    public bool Loading { get; set; }

    internal void Save(CupoFechaFullInfo cupoFecha)
    {
        Fecha = cupoFecha.Fecha;
        EscenarioCupo = cupoFecha.EscenarioCupo;
        CanalVenta = cupoFecha.CanalVenta;
        DiaSemana = cupoFecha.DiaSemana;
        HoraInicio = cupoFecha.HoraInicio;
        HoraFin = cupoFecha.HoraFin;
        Total = cupoFecha.Total;
        SobreCupo = cupoFecha.SobreCupo;
        TopeEnCupo = cupoFecha.TopeEnCupo;
    }

    internal void Update(CupoFechaViewModel cupoFecha)
    {
        Fecha = cupoFecha.Fecha;
        EscenarioCupo = cupoFecha.EscenarioCupo;
        CanalVenta = cupoFecha.CanalVenta;
        DiaSemana = cupoFecha.DiaSemana;
        HoraInicio = cupoFecha.HoraInicio;
        HoraFin = cupoFecha.HoraFin;
        Total = cupoFecha.Total;
        SobreCupo = cupoFecha.SobreCupo;
        TopeEnCupo = cupoFecha.TopeEnCupo;
    }
}
