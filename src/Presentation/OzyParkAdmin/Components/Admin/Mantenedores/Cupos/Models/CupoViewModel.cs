using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;

/// <summary>
/// El view model de cupo.
/// </summary>
public sealed record CupoViewModel
{
    /// <summary>
    /// El id del cupo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El escenario de cupo asociado al cupo.
    /// </summary>
    public EscenarioCupoInfo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// El canal de venta del cupo.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// El día de semana del cupo.
    /// </summary>
    public DiaSemana DiaSemana { get; set; } = default!;

    /// <summary>
    /// La hora de inicio del cupo.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// La hora de fin del cupo.
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

    internal DateTime? FechaEfectivaDate
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

    internal void Save(CupoFullInfo cupo)
    {
        EscenarioCupo = cupo.EscenarioCupo;
        CanalVenta = cupo.CanalVenta;
        DiaSemana = cupo.DiaSemana;
        HoraInicio = cupo.HoraInicio;
        HoraFin = cupo.HoraFin;
        Total = cupo.Total;
        SobreCupo = cupo.SobreCupo;
        TopeEnCupo = cupo.TopeEnCupo;
        FechaEfectiva = cupo.FechaEfectiva;
        UltimaModificacion = cupo.UltimaModificacion;
    }

    internal void Update(CupoViewModel cupo)
    {
        EscenarioCupo = cupo.EscenarioCupo;
        CanalVenta = cupo.CanalVenta;
        DiaSemana = cupo.DiaSemana;
        HoraInicio = cupo.HoraInicio;
        HoraFin = cupo.HoraFin;
        Total = cupo.Total;
        SobreCupo = cupo.SobreCupo;
        TopeEnCupo = cupo.TopeEnCupo;
        FechaEfectiva = cupo.FechaEfectiva;
        UltimaModificacion = cupo.UltimaModificacion;
    }
}
