namespace OzyParkAdmin.Domain.Reportes.Pdf;

/// <summary>
/// El margen que tendrá una página de pdf.
/// </summary>
/// <param name="Left">El margen izquierdo.</param>
/// <param name="Top">El margen de arriba.</param>
/// <param name="Right">El margen derecho.</param>
/// <param name="Bottom">El margen de abajo.</param>
public sealed record Margin(float? Left, float? Top, float? Right, float? Bottom)
{
    /// <summary>
    /// Un margen con valores nulos.
    /// </summary>
    public static readonly Margin Empty = new(null, null, null, null);
}
