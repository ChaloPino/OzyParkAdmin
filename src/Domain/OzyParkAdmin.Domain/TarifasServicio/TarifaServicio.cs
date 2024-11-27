using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.TarifasServicio;

/// <summary>
/// Representa la entidad de tarifa por servicio.
/// </summary>
public sealed class TarifaServicio
{
    /// <summary>
    /// El inicio de vigencia de la tarifa.
    /// </summary>
    public DateTime InicioVigencia { get; private set; }

    /// <summary>
    /// La moneda de la tarifa.
    /// </summary>
    public Moneda Moneda { get; private set; } = default!;

    /// <summary>
    /// El servicio de la tarifa.
    /// </summary>
    public Servicio Servicio { get; private set; } = default!;

    /// <summary>
    /// El tramo de la tarifa.
    /// </summary>
    public Tramo Tramo { get; private set; } = default!;

    /// <summary>
    /// El grupo etario de la tarifa.
    /// </summary>
    public GrupoEtario GrupoEtario { get; private set; } = default!;

    /// <summary>
    /// El tipo de día de la tarifa.
    /// </summary>
    public TipoDia TipoDia { get; private set; } = default!;

    /// <summary>
    /// El tipo de horario de la tarifa.
    /// </summary>
    public TipoHorario TipoHorario { get; private set; } = default!;

    /// <summary>
    /// El canal de venta de la tarifa.
    /// </summary>
    public CanalVenta CanalVenta { get; private set; } = default!;

    /// <summary>
    /// El tipo de segmentación de la tarifa.
    /// </summary>
    public TipoSegmentacion TipoSegmentacion { get; private set; } = default!;

    /// <summary>
    /// El valor afecto.
    /// </summary>
    public decimal ValorAfecto { get; private set; }

    /// <summary>
    /// El valor exento.
    /// </summary>
    public decimal ValorExento { get; private set; }

    /// <summary>
    /// El valor de la tarifa.
    /// </summary>
    public decimal Valor { get; private set; }

    internal static TarifaServicio Create(
        DateTime inicioVigencia,
        Moneda moneda,
        Servicio servicio,
        Tramo tramo,
        GrupoEtario grupoEtario,
        CanalVenta canalVenta,
        TipoDia tipoDia,
        TipoHorario tipoHorario,
        TipoSegmentacion tipoSegmentacion,
        decimal valorAfecto,
        decimal valorExento) =>
        new()
        {
            InicioVigencia = inicioVigencia,
            Moneda = moneda,
            Servicio = servicio,
            Tramo = tramo,
            GrupoEtario = grupoEtario,
            CanalVenta = canalVenta,
            TipoDia = tipoDia,
            TipoHorario = tipoHorario,
            TipoSegmentacion = tipoSegmentacion,
            ValorAfecto = valorAfecto,
            ValorExento = valorExento,
            Valor = valorExento + valorAfecto,
        };

    internal void Update(decimal valorAfecto, decimal valorExento)
    {
        ValorAfecto = valorAfecto;
        ValorExento = valorExento;
        Valor = valorAfecto + valorExento;
    }
}
