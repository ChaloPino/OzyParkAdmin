namespace OzyParkAdmin.Application.Reportes;

/// <summary>
/// El dato de un reporte generado.
/// </summary>
public class DataInfo
{
    private readonly Dictionary<ColumnInfo, object?> _values = [];

    /// <summary>
    /// Consigue el valor de una columna.
    /// </summary>
    /// <param name="column">La columna.</param>
    /// <returns>El valor que se encuentra en la columna.</returns>
    public object? this[ColumnInfo column]
    {
        get => _values[column];
        set => _values[column] = value;
    }
}
