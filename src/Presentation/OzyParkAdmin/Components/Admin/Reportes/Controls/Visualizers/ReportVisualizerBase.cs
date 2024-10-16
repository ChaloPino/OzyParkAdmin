using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Components.Admin.Reportes.Models;
using OzyParkAdmin.Domain.Reportes;
using System.Globalization;

namespace OzyParkAdmin.Components.Admin.Reportes.Controls.Visualizers;

/// <summary>
/// El componente base para todos los visualizadores de reportes.
/// </summary>
public abstract class ReportVisualizerBase : ComponentBase
{
    /// <summary>
    /// Si el reporte se está cargando.
    /// </summary>
    [Parameter]
    public LoadingState Loading { get; set; } = LoadingState.None;

    /// <summary>
    /// Formatea el valor dependiendo de las opciones de la columna.
    /// </summary>
    /// <param name="column">La columna a la pertenece el valor.</param>
    /// <param name="value">El valor a formatear.</param>
    /// <returns>El valor formateado.</returns>
    protected static string? Format(ColumnInfo column, object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(column.Format))
        {
            return value.ToString();
        }

        string format = $"{{0:{column.Format}}}";
        return string.Format(CultureInfo.CurrentCulture, format, value);
    }

    /// <summary>
    /// Consigue una definición de agregación para una columna, si es que lo requiere.
    /// </summary>
    /// <param name="column">La columna a la que se le crará la definición de agregación.</param>
    /// <returns>La definición de agregación para la columna, si es que lo requiere.</returns>
    protected static AggregateDefinition<DataInfo>? GetAggregateDefinition(ColumnInfo column)
    {
        if (column.AggregationType is null)
        {
            return null;
        }

        return new AggregateDefinition<DataInfo>
        {
            Type = AggregateType.Custom,
            CustomAggregate = (source) => AggregateAndFormat(source, column)
        };
    }

    private static string AggregateAndFormat(IEnumerable<DataInfo> source, ColumnInfo column)
    {
        object? value = AggregationUtils.Aggregate(source, column);
        return Format(column, value) ?? string.Empty;
    }

    /// <summary>
    /// Consigue la clase css de la celda.
    /// </summary>
    /// <param name="column">La columna a evaluar.</param>
    /// <param name="data">El dato a evaluar.</param>
    /// <returns>La clase css de la celda.</returns>
    protected static string GetCellClass(ColumnInfo column, DataInfo data)
    {
        var cssBuilder = new CssBuilder()
            .AddClass("number", column.IsNumericType());

        object? value = data[column];
        EvaluateAndAddStyle(column, value, cssBuilder);
        return cssBuilder.Build();
    }

    /// <summary>
    /// Consigue el estilo css de la celda.
    /// </summary>
    /// <param name="column">La columna a evaluar.</param>
    /// <param name="data">El dato a evaluar.</param>
    /// <returns>El estilo css de la celda.</returns>
    protected static string GetCellStyle(ColumnInfo column, DataInfo data)
    {
        object? value = data[column];

        var styleBuilder = new StyleBuilder();
        EvaluateAndAddStyle(column, value, styleBuilder);
        return styleBuilder.Build();
    }

    private static void EvaluateAndAddStyle(ColumnInfo column, object? value, CssBuilder cssBuilder)
    {
        if (column.HasConditionalStyle)
        {
            if (column.SuccessStyle is not null && column.EvaluateSuccessCondition(column.Type, value))
            {
                if (column.TryGenerateSuccessCssStyle(out var successStyle))
                {
                    cssBuilder.AddClass(successStyle.Value, !successStyle.IsStyle);
                }

                return;
            }

            if (column.WarningStyle is not null && column.EvaluateWarningCondition(column.Type, value))
            {
                if (column.TryGenerateWarningCssStyle(out var warningStyle))
                {
                    cssBuilder.AddClass(warningStyle.Value, !warningStyle.IsStyle);
                }

                return;
            }

            if (column.ErrorStyle is not null && column.TryGenerateErrorCssStyle(out var errorStyle))
            {
                cssBuilder.AddClass(errorStyle.Value, !errorStyle.IsStyle);
            }
        }
    }

    private static void EvaluateAndAddStyle(ColumnInfo column, object? value, StyleBuilder styleBuilder)
    {
        if (column.HasConditionalStyle)
        {
            if (column.SuccessStyle is not null && column.EvaluateSuccessCondition(column.Type, value))
            {
                if (column.TryGenerateSuccessCssStyle(out var successStyle))
                {
                    styleBuilder.AddStyle(successStyle.Value, successStyle.IsStyle);
                }

                return;
            }

            if (column.WarningStyle is not null && column.EvaluateWarningCondition(column.Type, value))
            {
                if (column.TryGenerateWarningCssStyle(out var warningStyle))
                {
                    styleBuilder.AddStyle(warningStyle.Value, warningStyle.IsStyle);
                }

                return;
            }

            if (column.ErrorStyle is not null && column.TryGenerateErrorCssStyle(out var errorStyle))
            {
                styleBuilder.AddStyle(errorStyle.Value, errorStyle.IsStyle);
            }
        }
    }

    /// <summary>
    /// Consigue la clase css de la celda del footer.
    /// </summary>
    /// <param name="column">La columna a evaluar.</param>
    /// <returns>La clase css de la celda del footer.</returns>
    protected static string GetFooterClass(ColumnInfo column)
    {
        var cssBuilder = new CssBuilder()
            .AddClass("number", column.IsNumericType());
        return cssBuilder.Build();
    }
}
