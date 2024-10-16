using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// La información de un reporte.
/// </summary>
/// <param name="Aka">El aka del reporte.</param>
/// <param name="Title">El título del reporte.</param>
/// <param name="Order">El orden de despliegue del reporte.</param>
/// <param name="Roles">La lista de roles que pueden ver el reporte.</param>
public sealed record ReportInfo(string Aka, string Title, int Order, ImmutableArray<string> Roles);
