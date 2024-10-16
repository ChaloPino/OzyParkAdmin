using OzyParkAdmin.Domain.Reportes.Excel;
using System.Data;
using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// La clase base para crear cualquier tipo de columna de un reporte.
/// </summary>
public abstract class ColumnBase : SecureComponent<ColumnBase>, IExcelColumnFormattable, IConditionable
{
    private ExcelFormat? _currentExcelFormat;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ColumnBase"/>.
    /// </summary>
    protected ColumnBase()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ColumnBase"/>.
    /// </summary>
    /// <param name="reportId">El id del reporte al que pertenece esta columna.</param>
    /// <param name="id">El identificador de la columna.</param>
    protected ColumnBase(Guid reportId, int id)
    {
        ReportId = reportId;
        Id = id;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; protected set; }

    /// <summary>
    /// El id de la columna.
    /// </summary>
    public int Id { get; protected set; }

    /// <summary>
    /// El nombre de la columna.
    /// </summary>
    public string Name { get; protected set; } = default!;

    /// <summary>
    /// El título de la columna.
    /// </summary>
    public string Header { get; protected set; } = default!;

    /// <summary>
    /// El tipo de dato de la columna.
    /// </summary>
    public DbType Type { get; protected set; }

    /// <summary>
    /// El formato usado para cuando se despliega en Html o en Pdf.
    /// </summary>
    public string? Format { get; protected set; }

    /// <summary>
    /// Si se puede ordenar.
    /// </summary>
    public bool CanSort { get; protected set; }

    /// <summary>
    /// El tipo de agregación.
    /// </summary>
    public AggregationType? AggregationType { get; protected set; }

    /// <summary>
    /// El orden de despliegue.
    /// </summary>
    public int Order { get; protected set; }

    /// <summary>
    /// El formato de Excel.
    /// </summary>
    public ExcelFormat? ExcelFormat { get; protected set; }

    /// <summary>
    /// El íd del formato de Excel personalizado.
    /// </summary>
    public int? CustomExcelFormatId { get; protected set; }

    /// <summary>
    /// El formato de Excel personalizado.
    /// </summary>
    public string? CustomExcelFormat { get; protected set; }

    /// <summary>
    /// Si tiene estilo condicional.
    /// </summary>
    public bool HasConditionalStyle { get; protected set; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como exitoso.
    /// </summary>
    public ConditionalStyle? SuccessStyle { get; protected set; }

    /// <summary>
    /// El operador condicional para evaluar el valor como exitoso.
    /// </summary>
    public OperatorStyleType? SuccessOperator { get; protected set; }

    /// <summary>
    /// El valor condicional para evaluar el valor de la columna como exitoso.
    /// </summary>
    public string? SuccessConditionalValue { get; protected set; }

    /// <summary>
    /// El valor alternativo condicional para evaluar el valor de la columna como exitoso.
    /// Se usa para evaluaciones de tipo rango como el operador <see cref="OperatorStyleType.Between"/>.
    /// </summary>
    public string? SuccessAlternateConditionalValue { get; protected set; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como advertencia.
    /// </summary>
    public ConditionalStyle? WarningStyle { get; protected set; }

    /// <summary>
    /// El operador condicional para evaluar el valor como advertencia.
    /// </summary>
    public OperatorStyleType? WarningOperator { get; protected set; }

    /// <summary>
    /// El valor condicional para evaluar el valor de la columna como advertencia.
    /// </summary>
    public string? WarningConditionalValue { get; protected set; }

    /// <summary>
    /// El valor alternativo condicional para evaluar el valor de la columna como advertencia.
    /// Se usa para evaluaciones de tipo rango como el operador <see cref="OperatorStyleType.Between"/>.
    /// </summary>
    public string? WarningAlternateConditionalValue { get; protected set; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como error.
    /// </summary>
    public ConditionalStyle? ErrorStyle { get; protected set; }

    /// <inheritdoc/>
    public ExcelFormat? CurrentExcelFormat
    {
        get
        {
            _currentExcelFormat ??= ExcelFormat ?? (CustomExcelFormatId.HasValue && !string.IsNullOrEmpty(CustomExcelFormat) ? new ExcelFormat(CustomExcelFormatId.Value, CustomExcelFormat) : null);
            return _currentExcelFormat;
        }
    }

    /// <inheritdoc/>
    public bool IsStringAndGuidType()
        => Type == DbType.String
        || Type == DbType.StringFixedLength
        || Type == DbType.AnsiString
        || Type == DbType.AnsiStringFixedLength
        || Type == DbType.Guid;

    public bool IsStringType()
        => Type == DbType.String
        || Type == DbType.StringFixedLength
        || Type == DbType.AnsiString
        || Type == DbType.AnsiStringFixedLength;

    public bool IsNumericType()
        => Type == DbType.Byte
        || Type == DbType.Int16
        || Type == DbType.Int32
        || Type == DbType.Int64
        || Type == DbType.Decimal
        || Type == DbType.Double
        || Type == DbType.Single
        || Type == DbType.SByte
        || Type == DbType.UInt16
        || Type == DbType.UInt32
        || Type == DbType.UInt64
        || Type == DbType.Currency
        || Type == DbType.VarNumeric;

    public bool IsDecimalType
        => Type == DbType.Decimal
        || Type == DbType.Double
        || Type == DbType.Single
        || Type == DbType.Currency
        || Type == DbType.VarNumeric;

    public bool IsDateType()
        => Type == DbType.Date
        || Type == DbType.DateTime
        || Type == DbType.DateTime2
        || Type == DbType.DateTimeOffset;

    public int? CurrentFormatId => CurrentExcelFormat?.Id;

    public string? CurrentFormat => CurrentExcelFormat?.Format;

    public string DoFormat(object value)
    {
        return string.Format(CultureInfo.CurrentCulture, string.Concat("{0:", Format, "}"), value);
    }
}

/// <summary>
/// La columna base.
/// </summary>
/// <typeparam name="TColumn">El tipo de la columna.</typeparam>
public abstract class ColumnBase<TColumn> : ColumnBase
    where TColumn : ColumnBase<TColumn>
{
    protected ColumnBase()
    {
    }

    protected ColumnBase(Guid reportId, int id)
        : base(reportId, id)
    {
    }
}
