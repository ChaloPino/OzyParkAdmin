using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Domain.Reportes.Pivoted;

/// <summary>
/// Los nombres para columnas de tipo fecha de un reporte tipo pivote.
/// </summary>
public static class PivotDatePartNames
{
    /// <summary>
    /// El nombre para año.
    /// </summary>
    public static string YearName => PivotResources.YearName;

    /// <summary>
    /// El nombre para mes.
    /// </summary>
    public static string MonthName => PivotResources.MonthName;

    /// <summary>
    /// El nombre para día.
    /// </summary>
    public static string DayName => PivotResources.DayName;

    /// <summary>
    /// El nombre para semestre.
    /// </summary>
    public static string QuarterName => PivotResources.QuarterName;

    /// <summary>
    /// El nombre para el nombre corto de cada mes.
    /// </summary>
    public static string ShortMonthName => PivotResources.ShortMonthName;

    /// <summary>
    /// El nombre para el nombre largo de cada mes.
    /// </summary>
    public static string LongMonthName => PivotResources.LongMonthName;

    /// <summary>
    /// El nombre para fecha.
    /// </summary>
    public static string DateName => PivotResources.DateName;
}