namespace OzyParkAdmin.Application.Reportes.MasterDetails;

/// <summary>
/// Permite realizar agregaciones.
/// </summary>
public interface IAggregatable
{
    /// <summary>
    /// Realiza la agregación.
    /// </summary>
    /// <param name="column">La columna con la que se realizará la agregación.</param>
    /// <returns>El valor agregado.</returns>
    public object? Aggregate(ColumnInfo column);
}