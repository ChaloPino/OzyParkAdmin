namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Las acciones de ejecución que puede tener un reporte.
/// </summary>
public enum ActionType
{
    /// <summary>
    /// Ejecutar el reporte y que se muestre en html.
    /// </summary>
    Html = 1,

    /// <summary>
    /// Crear un reporte para Excel.
    /// </summary>
    Excel = 2,

    /// <summary>
    /// Crear un reporte en pdf.
    /// </summary>
    Pdf = 3,
}
