namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Contiene métodos de extensión para convertir elementos en html.
/// </summary>
public static class HtmlExtensions
{
    /// <summary>
    /// Convierte un texto para que tenga valores <br /> en lugar de salto de carro.
    /// </summary>
    /// <param name="value">El texto a modificar.</param>
    /// <returns>El texto preparado para html.</returns>
    public static string ToHtml(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return value.Replace("\r\n", "<br />");
    }
}
