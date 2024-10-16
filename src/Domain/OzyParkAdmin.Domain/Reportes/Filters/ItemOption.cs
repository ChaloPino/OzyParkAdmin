namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Representa el elemento de un filtro de tipo lista.
/// </summary>
public class ItemOption
{
    /// <summary>
    /// El valor del elemento.
    /// </summary>
    public string Valor { get; set; } = string.Empty;

    /// <summary>
    /// El texto a desplegar del elemento.
    /// </summary>
    public string Display { get; set; } = string.Empty;
}
