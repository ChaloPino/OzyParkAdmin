namespace OzyParkAdmin.Domain.Reportes.Listed;

/// <summary>
/// Un reporte de tipo lista.
/// </summary>
public sealed class ListedReport : Report
{
    private ListedReport()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListedReport"/>.
    /// </summary>
    /// <param name="aka">El aka del reporte.</param>
    public ListedReport(string aka)
        : base(ReportType.Listed, aka)
    {
    }

    public bool IsSortingInDatabase { get; private set; }

    public ListedReport ChangeIsSortingInDatabase(bool isSortingInDatabase)
    {
        IsSortingInDatabase = isSortingInDatabase;
        return this;
    }
}
