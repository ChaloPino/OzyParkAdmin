using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Domain.TarifasProducto;
public static class TarifaProductoExtensions
{
    /// <summary>
    /// Convierte una lista de <see cref="TarifaProducto"/> en una lista de <see cref="TarifaProductoFullInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="TarifaProducto"/>.</param>
    /// <returns>La lista de <see cref="TarifaProductoFullInfo"/> convertida de <paramref name="source"/>.</returns>
    public static IEnumerable<TarifaProductoFullInfo> ToFullInfo(this IEnumerable<TarifaProducto> source) =>
        [.. source.Select(ToFullInfo)];

    /// <summary>
    /// Convierte un <see cref="TarifaProducto"/> en un <see cref="TarifaProductoFullInfo"/>.
    /// </summary>
    /// <param name="tarifa">La <see cref="TarifaProducto"/> a convertir.</param>
    /// <returns>La <see cref="TarifaProductoFullInfo"/> convertida de <paramref name="tarifa"/>.</returns>
    public static TarifaProductoFullInfo ToFullInfo(this TarifaProducto tarifa) =>
        new()
        {
            InicioVigencia = tarifa.InicioVigencia,
            Moneda = tarifa.Moneda,
            Producto = tarifa.Producto.ToInfo(),
            CanalVenta = tarifa.CanalVenta,
            TipoDia = tarifa.TipoDia,
            TipoHorario = tarifa.TipoHorario,
            ValorAfecto = tarifa.ValorAfecto,
            ValorExento = tarifa.ValorExento,
            Valor = tarifa.Valor,
        };
}
