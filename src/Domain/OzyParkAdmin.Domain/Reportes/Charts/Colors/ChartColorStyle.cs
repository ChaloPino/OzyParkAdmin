namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// El estilo de color para un gráfico.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="ChartColorStyle"/>.
/// </remarks>
/// <param name="key">La clave del estilo.</param>
public class ChartColorStyle(string key)
{

    /// <summary>
    /// La clave del estilo.
    /// </summary>
    public string Key { get; } = key;

    /// <summary>
    /// Los colores que forman parte del estilo.
    /// </summary>
    public IEnumerable<IChartColor>? Colors { get; private set; }

    /// <summary>
    /// Las variaciones que forman parte del estilo.
    /// </summary>
    public IEnumerable<ChartColorStyleVariation>? Variations { get; private set; }

    /// <summary>
    /// Agrega colores al estilo.
    /// </summary>
    /// <param name="colors">Los colores a ser agregados.</param>
    /// <returns>El mismo <see cref="ChartColorStyle"/>, de forma que se pueden ejecutar operaciones concatenadas.</returns>
    public ChartColorStyle AddColors(params IChartColor[] colors)
    {
        Colors = colors;
        return this;
    }

    /// <summary>
    /// Agrega variaciones de color al estilo.
    /// </summary>
    /// <param name="variations">Las variaciones de color a ser agregados.</param>
    /// <returns>El mismo <see cref="ChartColorStyle"/>, de forma que se pueden ejecutar operaciones concatenadas.</returns>
    public ChartColorStyle AddVariations(params ChartColorStyleVariation[] variations)
    {
        Variations = variations;
        return this;
    }

    /// <summary>
    /// Genera los colores usando las variaciones.
    /// </summary>
    /// <param name="token">
    /// El <see cref="ChartColorStyleVariationToken"/> usado para generar los colores.
    /// <para>Es opcional, si viene un valor se usa ese valor, en caso contario se aplican todas las variaciones del estilo.</para>
    /// </param>
    /// <returns>La lista de colores generados.</returns>
    public IEnumerable<IChartColor> GenerateColors(ChartColorStyleVariationToken? token)
    {
        if (Variations is not null && Colors is not null)
        {
            foreach (ChartColorStyleVariation variation in Variations)
            {
                foreach (IChartColor color in Colors)
                {
                    IChartColor variatedColor = token.HasValue
                        ? token.Value.Apply(color)
                        : color;

                    yield return variation.Apply(variatedColor);
                }
            }
        }
    }
}
