using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.TarifasServicio;

/// <summary>
/// Contiene métodos de extensión para <see cref="TarifaServicio"/>.
/// </summary>
public static class TarifaServicioExtensions
{
    /// <summary>
    /// Convierte una lista de <see cref="TarifaServicio"/> en una lista de <see cref="TarifaServicioFullInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="TarifaServicio"/>.</param>
    /// <returns>La lista de <see cref="TarifaServicioFullInfo"/> convertida de <paramref name="source"/>.</returns>
    public static IEnumerable<TarifaServicioFullInfo> ToFullInfo(this IEnumerable<TarifaServicio> source) =>
        [.. source.Select(ToFullInfo)];

    /// <summary>
    /// Convierte un <see cref="TarifaServicio"/> en un <see cref="TarifaServicioFullInfo"/>.
    /// </summary>
    /// <param name="tarifa">La <see cref="TarifaServicio"/> a convertir.</param>
    /// <returns>La <see cref="TarifaServicioFullInfo"/> convertida de <paramref name="tarifa"/>.</returns>
    public static TarifaServicioFullInfo ToFullInfo(this TarifaServicio tarifa) =>
        new()
        {
            InicioVigencia = tarifa.InicioVigencia,
            Moneda = tarifa.Moneda,
            Servicio = tarifa.Servicio.ToInfo(),
            Tramo = tarifa.Tramo.ToInfo(),
            GrupoEtario = tarifa.GrupoEtario.ToInfo(),
            CanalVenta = tarifa.CanalVenta,
            TipoDia = tarifa.TipoDia,
            TipoHorario = tarifa.TipoHorario,
            TipoSegmentacion = tarifa.TipoSegmentacion,
            ValorAfecto = tarifa.ValorAfecto,
            ValorExento = tarifa.ValorExento,
            Valor = tarifa.Valor,
        };
}
