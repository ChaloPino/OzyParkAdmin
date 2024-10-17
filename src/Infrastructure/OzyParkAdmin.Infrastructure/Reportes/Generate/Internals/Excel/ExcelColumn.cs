using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal sealed class ExcelColumn(IExcelColumnFormattable column)
{
    internal IExcelColumnFormattable InnerColumn { get; } = column;
    public int Id => InnerColumn.Id;
    public string Name => InnerColumn.Name;
    public string? Header => InnerColumn.Header;
    public DbType Type => InnerColumn.Type;
    public int Order => InnerColumn.Order;
    public AggregationType? AggregationType => InnerColumn.AggregationType;
    public ExcelFormat? ExcelFormat => InnerColumn.CurrentExcelFormat;
    public bool HasConditionalStyle => InnerColumn.HasConditionalStyle;
    public ConditionalStyle? SuccessStyle => InnerColumn.SuccessStyle;
    public OperatorStyleType? SuccessOperator => InnerColumn.SuccessOperator;
    public string? SuccessConditionalValue => InnerColumn.SuccessConditionalValue;
    public string? SuccessAlternateConditionalValue => InnerColumn.SuccessAlternateConditionalValue;
    public ConditionalStyle? WarningStyle => InnerColumn.WarningStyle;
    public OperatorStyleType? WarningOperator => InnerColumn.WarningOperator;
    public string? WarningConditionalValue => InnerColumn.WarningConditionalValue;
    public string? WarningAlternateConditionalValue => InnerColumn.WarningAlternateConditionalValue;
    public ConditionalStyle? ErrorStyle => InnerColumn.ErrorStyle;

    internal int HeaderStyleId { get; set; }
    internal int TotalHeaderStyleId { get; set; }
    internal int TotalStyleId { get; set; }
    internal int AlternateTotalStyleId { get; set; }
    internal int StyleId { get; set; }
    internal int AlternateStyleId { get; set; }
    internal int? FooterStyleId { get; set; }
    internal int? HeaderSharedTable { get; set; }
    internal int? FooterSharedTable { get; set; }
    internal int? SuccessStyleId { get; set; }
    internal int? WarningStyleId { get; set; }
    internal int? ErrorStyleId { get; set; }
    internal IDictionary<DataRow, int?> SharedTable { get; } = new Dictionary<DataRow, int?>();

    internal bool IsText() => InnerColumn.Type == DbType.Boolean ||
                              InnerColumn.IsStringAndGuidType();

    internal bool IsStringType() => InnerColumn.IsStringAndGuidType();

    internal static int GetExcelIndexStyle(ConditionalStyle style) => style.GetExcelIndexStyle();

    internal bool EvaluateSuccessCondition(DbType type, object variable)
        => InnerColumn.EvaluateSuccessCondition(type, variable);

    internal bool EvaluateWarningCondition(DbType type, object variable)
        => InnerColumn.EvaluateWarningCondition(type, variable);
}
