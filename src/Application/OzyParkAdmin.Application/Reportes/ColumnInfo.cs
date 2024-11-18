using OzyParkAdmin.Domain.Reportes;
using System.Data;

namespace OzyParkAdmin.Application.Reportes;
/// <summary>
/// La información de una columna.
/// </summary>
public class ColumnInfo : IConditionable
{
    /// <summary>
    /// El nombre de la columna.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// El título de la columna.
    /// </summary>
    public string Header { get; set; } = default!;

    /// <summary>
    /// El tipo de dato de la columna.
    /// </summary>
    public DbType Type { get; set; }

    /// <summary>
    /// El formato usado para cuando se despliega en Html o en Pdf.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Si se puede ordenar.
    /// </summary>
    public bool CanSort { get; set; }

    /// <summary>
    /// El tipo de agregación.
    /// </summary>
    public AggregationType? AggregationType { get; set; }

    /// <summary>
    /// El orden de despliegue.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Si tiene estilo condicional.
    /// </summary>
    public bool HasConditionalStyle { get; set; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como exitoso.
    /// </summary>
    public ConditionalStyle? SuccessStyle { get; set; }

    /// <summary>
    /// El operador condicional para evaluar el valor como exitoso.
    /// </summary>
    public OperatorStyleType? SuccessOperator { get; set; }

    /// <summary>
    /// El valor condicional para evaluar el valor de la columna como exitoso.
    /// </summary>
    public string? SuccessConditionalValue { get; set; }

    /// <summary>
    /// El valor alternativo condicional para evaluar el valor de la columna como exitoso.
    /// Se usa para evaluaciones de tipo rango como el operador <see cref="OperatorStyleType.Between"/>.
    /// </summary>
    public string? SuccessAlternateConditionalValue { get; set; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como advertencia.
    /// </summary>
    public ConditionalStyle? WarningStyle { get; set; }

    /// <summary>
    /// El operador condicional para evaluar el valor como advertencia.
    /// </summary>
    public OperatorStyleType? WarningOperator { get; set; }

    /// <summary>
    /// El valor condicional para evaluar el valor de la columna como advertencia.
    /// </summary>
    public string? WarningConditionalValue { get; set; }

    /// <summary>
    /// El valor alternativo condicional para evaluar el valor de la columna como advertencia.
    /// Se usa para evaluaciones de tipo rango como el operador <see cref="OperatorStyleType.Between"/>.
    /// </summary>
    public string? WarningAlternateConditionalValue { get; set; }

    /// <summary>
    /// El estilo condicional para que se pinte el valor como error.
    /// </summary>
    public ConditionalStyle? ErrorStyle { get; set; }

    /// <summary>
    /// La función de ordenamiento que puede tener.
    /// </summary>
    public Func<DataInfo, object?>? SortBy => CanSort ? (info) => info[this] : null;


    internal bool IsStringType()
        => Type == DbType.String
        || Type == DbType.StringFixedLength
        || Type == DbType.AnsiString
        || Type == DbType.AnsiStringFixedLength;

    /// <summary>
    /// Revisa si el tipo de la columna es numérico.
    /// </summary>
    /// <returns><c>true</c> si el tipo de la columna es numérico; en caso contrario, <c>false</c>.</returns>
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

    internal bool IsDecimalType()
        => Type == DbType.Decimal
        || Type == DbType.Double
        || Type == DbType.Single
        || Type == DbType.Currency
        || Type == DbType.VarNumeric;

    internal bool IsDateType()
        => Type == DbType.Date
        || Type == DbType.DateTime
        || Type == DbType.DateTime2
        || Type == DbType.DateTimeOffset;

    /// <summary>
    /// Crea un <see cref="ColumnInfo"/> a partir de <paramref name="column"/>.
    /// </summary>
    /// <param name="column">El <see cref="ColumnBase"/> usado para crear un <see cref="ColumnInfo"/>.</param>
    /// <returns>Un nuevo <see cref="ColumnInfo"/>.</returns>
    public static ColumnInfo FromColumn(ColumnBase column)
    {
        return new()
        {
            Name = column.Name,
            Type = column.Type,
            Header = column.Header,
            CanSort = column.CanSort,
            Order = column.Order,
            Format = column.Format,
            AggregationType = column.AggregationType,
            HasConditionalStyle = column.HasConditionalStyle,
            SuccessStyle = column.SuccessStyle,
            SuccessOperator = column.SuccessOperator,
            SuccessConditionalValue = column.SuccessConditionalValue,
            SuccessAlternateConditionalValue = column.SuccessAlternateConditionalValue,
            WarningStyle = column.WarningStyle,
            WarningOperator = column.WarningOperator,
            WarningConditionalValue = column.WarningConditionalValue,
            WarningAlternateConditionalValue = column.WarningAlternateConditionalValue,
            ErrorStyle = column.ErrorStyle,
        };
    }
}