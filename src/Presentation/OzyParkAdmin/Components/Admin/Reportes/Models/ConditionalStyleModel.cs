namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// Contiene el estilo condicional que se va aplicar a un elemento del reporte.
/// </summary>
/// <param name="IsStyle">Si es estilo o es clase.</param>
/// <param name="Value">El valor del estilo o la clase.</param>
public sealed record ConditionalStyleModel(bool IsStyle, string Value);
