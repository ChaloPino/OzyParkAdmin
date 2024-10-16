using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Un color con patrón.
/// </summary>
public readonly struct PatternChartColor : IChartColor
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="PatternChartColor"/>.
    /// </summary>
    /// <param name="name">El nombre del color.</param>
    /// <param name="value">El color.</param>
    public PatternChartColor(string name, IChartColor value)
    {
        Name = name;
        Value = value;
    }

    internal string Name { get; }
    internal IChartColor Value { get; }

    /// <inheritdoc/>
    public HexChartColor ToHex()
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public HslChartColor ToHsl()
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public RgbChartColor ToRgb()
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public override string ToString() =>
        $"pattern.draw(\"{Name}\", \"{Value}\")";
}
