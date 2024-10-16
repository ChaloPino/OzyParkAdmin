namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// Los estados de la generación de un reporte.
/// </summary>
public enum LoadingState
{
    /// <summary>
    /// Estado inicial.
    /// </summary>
    None,

    /// <summary>
    /// Se está generando el reporte.
    /// </summary>
    Loading,

    /// <summary>
    /// Se generó el reporte.
    /// </summary>
    Loaded,
}
