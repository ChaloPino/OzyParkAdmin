namespace OzyParkAdmin.Domain.Reportes.MasterDetails;

/// <summary>
/// Representa una columna para el detalle de un reporte de tipo maestro detalle.
/// </summary>
public sealed class DetailColumn : ColumnBase<DetailColumn>
{
    private DetailColumn()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="DetailColumn"/>.
    /// </summary>
    /// <param name="detail">El detalle al que pertence la columna.</param>
    /// <param name="id">El identificador de la columna.</param>
    public DetailColumn(ReportDetail detail, int id)
        : base(detail.ReportId, id)
    {
        DetailId = detail.DetailId;
    }

    /// <summary>
    /// El id del detalle.
    /// </summary>
    public int DetailId { get; private set; }
}
