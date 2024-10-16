namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// Información de una tabla para el dashboard.
/// </summary>
public sealed class ChartTableInfo
{
    /// <summary>
    /// Las columnas de la tabla.
    /// </summary>
    public IEnumerable<ColumnInfo> Colummns { get; set; } = [];

    /// <summary>
    /// Los datos de la tabla.
    /// </summary>
    public IEnumerable<DataInfo> Data { get; set; } = [];
}
