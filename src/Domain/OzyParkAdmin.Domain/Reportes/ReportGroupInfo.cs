using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// La información de la agrupación de reportes.
/// </summary>
/// <param name="Title">El título de la agrupación de reportes.</param>
/// <param name="Order">El orden de despliegue de la agrupación.</param>
/// <param name="Reports">Listado de reportes del grupo.</param>
public sealed record ReportGroupInfo(string Title, int Order, ImmutableArray<ReportInfo> Reports);
