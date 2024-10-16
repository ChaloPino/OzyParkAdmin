using OzyParkAdmin.Domain.Reportes.Charts;
using System.Data;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// La información de un dataset del gráfico.
/// </summary>
public class ChartDataSetInfo : ChartDictionary
{
    internal static ChartDataSetInfo Create(ChartDataSet chartDataSet, DataSet dataSet, ChartReport report)
    {
        ChartDataSetInfo info = [];

        chartDataSet.Type.AddEnumTo("type", info);
        chartDataSet.BackgroundColor.AddColorsTo("backgroundColor", info);
        chartDataSet.Base.AddTo("base", info);
        chartDataSet.BarPercentage.AddTo("barPercentage", info);
        chartDataSet.BarThickness.AddThicknessTo("barThickness", info);
        chartDataSet.BorderAlign.AddEnumTo("borderAlign", info);
        chartDataSet.BorderCapStyle.AddEnumTo("borderCapStyle", info);
        chartDataSet.BorderColor.AddColorsTo("borderColor", info);
        chartDataSet.BorderDash.AddInt32ArrayTo("borderDash", info);
        chartDataSet.BorderDashOffset.AddTo("borderDashOffset", info);
        chartDataSet.BorderJoinStyle.AddEnumTo("borderJoinStyle", info);
        chartDataSet.BorderSkipped.AddBorderSkippedTo("borderSkipped", info);
        chartDataSet.BorderWidth.AddWidthTo("borderWidth", info);
        chartDataSet.BorderRadius.AddRadiusTo("borderRadius", info);
        chartDataSet.CategoryPercentage.AddTo("categoryPercentage", info);
        chartDataSet.Circumference.AddTo("circumference", info);
        chartDataSet.Clip.AddClipTo("clip", info);
        chartDataSet.CubicInterpolationMode.AddEnumTo("cubicInterpolationMode", info);
        chartDataSet.DrawActiveElementsOnTop.AddTo("drawActiveElementsOnTop", info);
        chartDataSet.Fill.AddFillTo("fill", info);
        chartDataSet.Grouped.AddTo("grouped", info);
        chartDataSet.HitRadius.AddTo("hitRadius", info);
        chartDataSet.HoverBackgroundColor.AddColorsTo("hoverBackgroundColor", info);
        chartDataSet.HoverBorderCapStyle.AddEnumTo("hoveBorderCapStyle", info);
        chartDataSet.HoverBorderColor.AddColorsTo("hoverBorderColor", info);
        chartDataSet.HoverBorderDash.AddInt32ArrayTo("hoverBorderDash", info);
        chartDataSet.HoverBorderDashOffset.AddTo("hoverBorderDashOffset", info);
        chartDataSet.HoverBorderJoinStyle.AddEnumTo("hoverBorderJoinStyle", info);
        chartDataSet.HoverBorderRadius.AddTo("hoverBorderRadius", info);
        chartDataSet.HoverBorderWidth.AddTo("hoverBorderWidth", info);
        chartDataSet.HoverOffset.AddTo("hoverOffset", info);
        chartDataSet.Hidden.AddTo("hidden", info);
        chartDataSet.HitRadius.AddTo("hitRadius", info);
        chartDataSet.IndexAxis.AddTo("indexAxis", info);
        chartDataSet.InflateAmount.AddInflateAmountTo("inflateAmount", info);
        chartDataSet.Label.AddTo("label", info);
        chartDataSet.MaxBarThickness.AddTo("maxBarThickness", info);
        chartDataSet.MinBarLength.AddTo("minBarLength", info);
        chartDataSet.Offset.AddTo("offset", info);
        chartDataSet.Order.AddTo("order", info);
        chartDataSet.PointBackgroundColor.AddColorsTo("pointBackgroundColor", info);
        chartDataSet.PointBorderColor.AddColorsTo("pointBorderColor", info);
        chartDataSet.PointBorderWidth.AddTo("pointBorderWidth", info);
        chartDataSet.PointHitRadius.AddTo("pointHitRadius", info);
        chartDataSet.PointHoverBackgroundColor.AddColorsTo("pointHoverBackgroundColor", info);
        chartDataSet.PointHoverBorderColor.AddColorsTo("pointHoverBorderColor", info);
        chartDataSet.PointHoverBorderWidth.AddTo("pointHoverBorderWidth", info);
        chartDataSet.PointHoverRadius.AddTo("pointHoverRadius", info);
        chartDataSet.PointRadius.AddTo("pointRadius", info);
        chartDataSet.PointRotation.AddTo("pointRotation", info);
        chartDataSet.PointStyle.AddEnumTo("pointStyle", info);
        chartDataSet.Radius.AddTo("radius", info);
        chartDataSet.Rotation.AddTo("rotation", info);
        chartDataSet.ShowLine.AddTo("showLine", info);
        chartDataSet.SkipNull.AddTo("skipNull", info);
        chartDataSet.Spacing.AddTo("spacing", info);
        chartDataSet.SpanGaps.AddSpanGapsTo("spanGaps", info);
        chartDataSet.Stack.AddTo("stack", info);
        chartDataSet.Stepped.AddSteppedTo("stepped", info);
        chartDataSet.Tension.AddTo("tension", info);
        chartDataSet.Weight.AddTo("weight", info);
        chartDataSet.XAxisID.AddTo("xAxisID", info);
        chartDataSet.YAxisID.AddTo("yAxisID", info);
        chartDataSet.Parsing.AddParsingTo(info);

        DataTable? dataTable = ChartHelper.ExtractDataTable(dataSet, chartDataSet.DataSourceIndex, chartDataSet.DataSource?.Name ?? report.DataSource.Name);
        var data = ChartHelper.ExtractData(dataTable, chartDataSet);

        data.Switch(
            onPrimitive: primitive => info.Add("data", primitive),
            onComplex: complex => info.Add("data", complex),
            onComplexList: complexList => info.Add("data", complexList));

        return info;
     }
}