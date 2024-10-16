namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// El valor que tiene el filtro para la generación de un reporte.
/// </summary>
/// <param name="FilterId">El id del filtro.</param>
/// <param name="Value">El valor del filtro.</param>
public sealed record FilterValue(int FilterId, object? Value);
