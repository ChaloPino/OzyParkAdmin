using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Models;

/// <summary>
/// El view model de la tarifa de productos.
/// </summary>
public sealed class TarifaProductoViewModel : IEquatable<TarifaProductoViewModel>
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
    public ProductoInfo Producto { get; set; } = default!;

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

    internal void Update(TarifaProductoFullInfo info)
    {
        ValorAfecto = info.ValorAfecto;
        ValorExento = info.ValorExento;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is TarifaProductoViewModel other && Equals(other);

    /// <inheritdoc/>
    public bool Equals(TarifaProductoViewModel? other) =>
        other is not null &&
        InicioVigencia == other.InicioVigencia &&
        Moneda.Id == other.Moneda.Id &&
        Producto.Id == other.Producto.Id &&
        TipoDia.Id == other.TipoDia.Id &&
        TipoHorario.Id == other.TipoHorario.Id &&
        CanalVenta.Id == other.CanalVenta.Id;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(HashCode.Combine(InicioVigencia, Moneda.Id, Producto.Id, TipoDia.Id, TipoHorario.Id, CanalVenta.Id));
}
