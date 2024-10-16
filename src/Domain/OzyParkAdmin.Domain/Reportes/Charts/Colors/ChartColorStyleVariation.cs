using System.Collections;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;
/// <summary>
/// La colección de <see cref="ChartColorStyleVariationToken"/>.
/// </summary>
public sealed class ChartColorStyleVariation : IEnumerable<ChartColorStyleVariationToken>
{
    /// <summary>
    /// La lista de variaciones vacía.
    /// </summary>
    public static readonly ChartColorStyleVariation Empty = new();

    private readonly List<ChartColorStyleVariationToken> _list = [];

    /// <summary>
    /// Crea una nueva instancia de <see cref="ChartColorStyleVariation"/>.
    /// </summary>
    public ChartColorStyleVariation()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ChartColorStyleVariationToken"/> con una colección de tokens.
    /// </summary>
    /// <param name="tokens">Los <see cref="ChartColorStyleVariationToken"/> usados para inicializar la lista.</param>
    public ChartColorStyleVariation(params ChartColorStyleVariationToken[] tokens)
    {
        _list.AddRange(tokens);
    }

    /// <summary>
    /// Aplica la variación a un color.
    /// </summary>
    /// <param name="color">El color al que se le va a aplicar la variación.</param>
    /// <returns>El nuevo color con la variación aplicada.</returns>
    public IChartColor Apply(IChartColor color)
    {
        foreach (var token in _list)
        {
            color = token.Apply(color);
        }

        return color;
    }

    /// <inheritdoc/>
    public IEnumerator<ChartColorStyleVariationToken> GetEnumerator() =>
        _list.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() =>
        _list.GetEnumerator();
}
