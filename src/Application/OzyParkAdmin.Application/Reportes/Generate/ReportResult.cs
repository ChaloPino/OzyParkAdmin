using OzyParkAdmin.Domain.Shared;
using TypeUnions;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// El resultado de la generación del reporte en html.
/// </summary>
[TypeUnion<ReportGenerated, Failure>]
public sealed partial class ReportResult
{
}