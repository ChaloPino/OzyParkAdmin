using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;

/// <summary>
/// El modelo para editar varios cupos por fecha.
/// </summary>
public sealed class CuposFechaEditModel
{
    /// <summary>
    /// La fecha del cupo a editar.
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

    /// <summary>
    /// Si se está cargando el cupo.
    /// </summary>
    public bool Loading { get; set; }
}
