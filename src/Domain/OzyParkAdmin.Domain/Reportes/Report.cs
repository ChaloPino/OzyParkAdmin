using OzyParkAdmin.Domain.Reportes.DataSources;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Reportes.Listed;

namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// La entidad de un reporte.
/// </summary>
public abstract class Report : SecureComponent<Report>
{
    private readonly List<Filter> _filters = [];
    private readonly List<ReportTemplate> _templates = [];
    private readonly List<Column> _columns = [];
    private readonly List<ReportAction> _actions = [];

    /// <summary>
    /// Crea una nueva instancia de <see cref="Report"/>.
    /// </summary>
    protected Report()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Report"/>.
    /// </summary>
    /// <param name="type">El tipo del reporte.</param>
    /// <param name="aka">El aka del reporte.</param>
    protected Report(ReportType type, string aka)
    {
        Id = Guid.NewGuid();
        Type = type;
        Aka = aka.Trim();
    }

    public Guid Id { get; private set; }
    public string Aka { get; private set; } = default!;
    public ReportType Type { get; private set; }
    public string Title { get; private set; } = default!;
    public string FilterLayout { get; private set; } = default!;
    public DataSource DataSource { get; private set; } = default!;
    public bool CanSort { get; private set; }
    public bool ServerSide { get; private set; }
    public int Order { get; private set; }
    public bool Published { get; private set; }
    public ReportGroup ReportGroup { get; private set; } = default!;
    public bool ForDashboard { get; private set; }
    public IEnumerable<Filter> Filters => _filters;
    public IEnumerable<ReportTemplate> Templates => _templates;
    public IEnumerable<Column> Columns => _columns;
    public IEnumerable<ReportAction> Actions => _actions;
    public DateTimeOffset CreationDate { get; private set; }
    public DateTimeOffset LastUpdate { get; private set; }
    public byte[] Timestamp { get; private set; } = default!;
}
