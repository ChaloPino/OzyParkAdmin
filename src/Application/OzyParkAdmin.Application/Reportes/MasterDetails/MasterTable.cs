using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.MasterDetails;

namespace OzyParkAdmin.Application.Reportes.MasterDetails;

/// <summary>
/// Información de la tabla maestra del reporte.
/// </summary>
public sealed class MasterTable : IAggregatable
{
    /// <summary>
    /// El título.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Si la información es tabular.
    /// </summary>
    public bool IsTabular { get; set; }

    /// <summary>
    /// Las columnas.
    /// </summary>
    public IEnumerable<ColumnInfo> Columns { get; set; } = [];

    /// <summary>
    /// La información del reporte.
    /// </summary>
    public List<DataInfo> Data { get; set; } = [];

    /// <inheritdoc/>
    public object? Aggregate(ColumnInfo column) =>
        AggregationUtils.Aggregate(Data, column);

    /// <summary>
    /// Crea un <see cref="MasterTable"/>.
    /// </summary>
    /// <param name="report">El reporte que genera el <see cref="MasterTable"/>.</param>
    /// <param name="columns">Las columnas.</param>
    /// <param name="data">La información del reporte.</param>
    /// <returns>Un nuevo <see cref="MasterTable"/>.</returns>
    public static MasterTable FromMaster(MasterDetailReport report, IEnumerable<ColumnInfo> columns, IEnumerable<DataInfo> data)
    {
        return new()
        {
            Title = report.TitleInReport,
            IsTabular = report.IsTabular,
            Columns = columns,
            Data = [..data],
        };
    }

    
}
