using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.TarifasServicio;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;

/// <summary>
/// El view model de la tarifa de servicios.
/// </summary>
public sealed class TarifaServicioViewModel : IEquatable<TarifaServicioViewModel>
{
    /// <summary>
    /// El inicio de vigencia de la tarifa.
    /// </summary>
    public DateTime InicioVigencia { get; set; }

    /// <summary>
    /// La moneda de la tarifa.
    /// </summary>
    public Moneda Moneda { get; set; } = default!;

    /// <summary>
    /// El servicio.
    /// </summary>
    public ServicioInfo Servicio { get; set; } = default!;

    /// <summary>
    /// El tramo.
    /// </summary>
    public TramoInfo Tramo { get; set; } = default!;

    /// <summary>
    /// El grupo etario.
    /// </summary>
    public GrupoEtarioInfo GrupoEtario { get; set; } = default!;

    /// <summary>
    /// El tipo de día.
    /// </summary>
    public TipoDia TipoDia { get; set; } = default!;

    /// <summary>
    /// El tipo de horario.
    /// </summary>
    public TipoHorario TipoHorario { get; set; } = default!;

    /// <summary>
    /// El canal de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// El tipo de segmentación.
    /// </summary>
    public TipoSegmentacion TipoSegmenetacion { get; set; } = default!;

    /// <summary>
    /// El valor exento de la tarifa.
    /// </summary>
    public decimal ValorExento { get; set; }

    /// <summary>
    /// El valor afecto de la tarifa.
    /// </summary>
    public decimal ValorAfecto { get; set; }

    /// <summary>
    /// El valor de la tarifa.
    /// </summary>
    public decimal Valor => ValorExento + ValorAfecto;

    internal void Update(TarifaServicioFullInfo info)
    {
        ValorAfecto = info.ValorAfecto;
        ValorExento = info.ValorExento;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is TarifaServicioViewModel other && Equals(other);

    /// <inheritdoc/>
    public bool Equals(TarifaServicioViewModel? other) =>
        other is not null &&
        InicioVigencia == other.InicioVigencia &&
        Moneda.Id == other.Moneda.Id &&
        Servicio.Id == other.Servicio.Id &&
        Tramo.Id == other.Tramo.Id &&
        GrupoEtario.Id == other.GrupoEtario.Id &&
        TipoDia.Id == other.TipoDia.Id &&
        TipoHorario.Id == other.TipoHorario.Id &&
        CanalVenta.Id == other.CanalVenta.Id &&
        TipoSegmenetacion.Id == other.TipoSegmenetacion.Id;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(HashCode.Combine(InicioVigencia, Moneda.Id, Servicio.Id, Tramo.Id, GrupoEtario.Id, TipoDia.Id, TipoHorario.Id, CanalVenta.Id), TipoSegmenetacion.Id);
}
