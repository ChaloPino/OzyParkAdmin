namespace OzyParkAdmin.Domain.Reportes.Listed;

/// <summary>
/// Representa una columna del reporte.
/// </summary>
public sealed class Column : ColumnBase<Column>
{
    private Column()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Column"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece la columna.</param>
    /// <param name="id">El identificador de la columna.</param>
    public Column(Report report, int id)
        : base(report.Id, id)
    {
    }
}
