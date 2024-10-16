using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Domain.Reportes.Listed;

/// <summary>
/// Un reporte paginado.
/// </summary>
public sealed class PaginatedReport : Report
{
    /// <summary>
    /// La lista de páginas por defecto.
    /// </summary>
    public static readonly int[] DefaultPages = [50, 100, 150, 200];

    private PaginatedReport()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="PaginatedReport"/>.
    /// </summary>
    /// <param name="aka">El aka del reporte.</param>
    public PaginatedReport(string aka)
        : base(ReportType.Paginated, aka)
    {
    }

    /// <summary>
    /// El tamaño de cada página.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// La posibles págines que puede haber.
    /// </summary>
    public int[] Pages { get; private set; } = DefaultPages;

    /// <summary>
    /// Si la paginación se realiza en la base de datos.
    /// </summary>
    public bool IsPaginationInDatabase { get; private set; }

    /// <summary>
    /// El indíce del result set usado para conseguir el conteo de totales.
    /// </summary>
    public int? TotalRecordsResultIndex { get; private set; }

    /// <summary>
    /// El índice del result set usado para conseguir los elementos paginados.
    /// </summary>
    public int? DataResultIndex { get; private set; }

    /// <summary>
    /// Si el ordenamiento se realizará en la base de datos.
    /// </summary>
    public bool IsSortingInDatabase { get; private set; }

    /// <summary>
    /// La fuente de datos para conseguir el conteo de totales.
    /// </summary>
    public DataSource? TotalRecordsDataSource { get; private set; }

    /// <summary>
    /// El nombre del parámetro usado para el inicio de la página.
    /// </summary>
    public Parameter? StartParameter { get; private set; }

    /// <summary>
    /// El nombre del parámetro usado para el tamaño de la página.
    /// </summary>
    public Parameter? LengthParameter { get; private set; }

    /// <summary>
    /// Cambia el tamaño de la página.
    /// </summary>
    /// <param name="pageSize">El nuevo tamaño de la página.</param>
    /// <returns>El mismo <see cref="PaginatedReport"/>.</returns>
    public PaginatedReport ChangePageSize(int pageSize)
    {
        PageSize = pageSize;
        return this;
    }

    /// <summary>
    /// Cambia el listado de páginas.
    /// </summary>
    /// <param name="pages">La lista de páginas nuevas.</param>
    /// <returns>El mismo <see cref="PaginatedReport"/>.</returns>
    public PaginatedReport ChangePages(int[]? pages)
    {
        Pages = pages ?? DefaultPages;
        return this;
    }

    /// <summary>
    /// Cambia si la paginación se hace en la base de datos.
    /// </summary>
    /// <param name="isPaginationInDatabase">El valor que indica si la paginación se hace en la base de datos.</param>
    /// <returns>El mismo <see cref="PaginatedReport"/>.</returns>
    public PaginatedReport ChangeIsPaginationInDatabase(bool isPaginationInDatabase)
    {
        IsPaginationInDatabase = isPaginationInDatabase;
        return this;
    }

    /// <summary>
    /// Cambia el data source para conseguir el conteo de totales.
    /// </summary>
    /// <param name="totalRecordsDataSource">El data source para el conteo de totales.</param>
    /// <returns>El mismo <see cref="PaginatedReport"/>.</returns>
    public PaginatedReport ChangeTotalRecordsDataSource(DataSource? totalRecordsDataSource)
    {
        TotalRecordsDataSource = totalRecordsDataSource;
        return this;
    }

    public PaginatedReport ChangeTotalRecordsResultIndex(int? totalRecordsResultIndex)
    {
        TotalRecordsResultIndex = totalRecordsResultIndex;
        return this;
    }

    public PaginatedReport ChangeDataResultIndex(int? dataResultIndex)
    {
        DataResultIndex = dataResultIndex;
        return this;
    }

    public PaginatedReport ChangeStartParameter(Parameter? parameter)
    {
        StartParameter = parameter;
        return this;
    }

    public PaginatedReport ChangeLengthParameter(Parameter? parameter)
    {
        LengthParameter = parameter;
        return this;
    }
}
