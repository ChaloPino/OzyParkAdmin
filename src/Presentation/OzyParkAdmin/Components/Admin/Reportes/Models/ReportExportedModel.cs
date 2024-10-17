namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo para un reporte que se puede exportar como Html o Pdf.
/// </summary>
/// <param name="FileName">El nombre del archivo.</param>
/// <param name="Stream">El contenido del archivo.</param>
/// <param name="MimeType">El tipo del contenido.</param>
public sealed record ReportExportedModel(string FileName, Stream Stream, string MimeType);
