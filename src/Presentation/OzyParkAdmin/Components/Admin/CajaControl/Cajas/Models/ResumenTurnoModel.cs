using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;

/// <summary>
/// El modelo de resumen de un turno.
/// </summary>
public class ResumenTurnoModel
{
    private readonly ResumenTurnoInfo _resumen;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ResumenTurnoModel"/>.
    /// </summary>
    /// <param name="resumen">El <see cref="ResumenTurnoInfo"/>.</param>
    /// <param name="detalles">Los detalles que se usarán para este resumen.</param>
    public ResumenTurnoModel(ResumenTurnoInfo resumen, IEnumerable<DetalleTurnoInfo> detalles)
    {
        _resumen = resumen;
        Detalle = detalles
            .Where(x => x.EsAbono == resumen.EsAbono && x.Tipo == resumen.Tipo && x.Movimiento == resumen.Movimiento)
            .OrderBy(x => x.Fecha).ThenBy(x => x.Hora).ThenBy(x => x.Orden)
            .ToList();
    }
    /// <summary>
    /// Si es abono.
    /// </summary>
    public bool EsAbono => _resumen.EsAbono;

    /// <summary>
    /// El tipo de movimiento.
    /// </summary>
    public string Tipo => _resumen.Tipo;

    /// <summary>
    /// El movimiento.
    /// </summary>
    public string Movimiento => _resumen.Movimiento;

    /// <summary>
    /// La cantidad del movimiento.
    /// </summary>
    public int Cantidad => _resumen.Cantidad;

    /// <summary>
    /// El monto del movimiento.
    /// </summary>
    public decimal Monto => _resumen.Monto;

    /// <summary>
    /// Si muestra el detalle
    /// </summary>
    public bool ShowDetail { get; set; }

    /// <summary>
    /// El detalle asociado al resumen.
    /// </summary>
    public List<DetalleTurnoInfo> Detalle { get; }
}
