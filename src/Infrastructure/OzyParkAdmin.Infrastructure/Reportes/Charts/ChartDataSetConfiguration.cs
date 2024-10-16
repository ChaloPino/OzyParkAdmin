using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Charts;

namespace OzyParkAdmin.Infrastructure.Reportes.Charts;
internal sealed class ChartDataSetConfiguration : IEntityTypeConfiguration<ChartDataSet>
{
    public void Configure(EntityTypeBuilder<ChartDataSet> builder)
    {
        _ = builder.ToTable("rep_ChartDataSets_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.ChartId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Id)
            .HasColumnName("ChartDataSetId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.BackgroundColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Base)
            .IsRequired(false);

        _ = builder.Property(p => p.BarPercentage)
            .IsRequired(false);

        _ = builder.Property(p => p.BarThickness)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.BorderAlign)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.BorderCapStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.BorderColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.BorderDash)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.BorderDashOffset)
            .IsRequired(false);

        _ = builder.Property(p => p.BorderJoinStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.BorderSkipped)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.BorderRadius)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.BorderWidth)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.CategoryPercentage)
            .IsRequired(false);

        _ = builder.Property(p => p.Circumference)
            .IsRequired(false);

        _ = builder.Property(p => p.Clip)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.CubicInterpolationMode)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.DrawActiveElementsOnTop)
            .IsRequired(false);

        _ = builder.Property(p => p.Fill)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Grouped)
            .IsRequired(false);

        _ = builder.Property(p => p.HitRadius)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBackgroundColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderCapStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderDash)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderDashOffset)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderJoinStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderRadius)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverBorderWidth)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverOffset)
            .IsRequired(false);

        _ = builder.Property(p => p.HoverRadius)
            .IsRequired(false);

        _ = builder.Property(p => p.IndexAxis)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.InflateAmount)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Label)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.MaxBarThickness)
            .IsRequired(false);

        _ = builder.Property(p => p.MinBarLength)
            .IsRequired(false);

        _ = builder.Property(p => p.Offset)
            .IsRequired(false);

        _ = builder.Property(p => p.Order)
            .IsRequired(false);

        _ = builder.Property(p => p.PointBackgroundColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.PointBorderColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.PointBorderWidth)
            .IsRequired(false);

        _ = builder.Property(p => p.PointHitRadius)
            .IsRequired(false);

        _ = builder.Property(p => p.PointHoverBackgroundColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.PointHoverBorderColor)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.PointHoverBorderWidth)
            .IsRequired(false);

        _ = builder.Property(p => p.PointHoverRadius)
            .IsRequired(false);

        _ = builder.Property(p => p.PointRadius)
            .IsRequired(false);

        _ = builder.Property(p => p.PointRotation)
            .IsRequired(false);

        _ = builder.Property(p => p.PointStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.Radius)
            .IsRequired(false);

        _ = builder.Property(p => p.Rotation)
            .IsRequired(false);

        _ = builder.Property(p => p.ShowLine)
            .IsRequired(false);

        _ = builder.Property(p => p.SkipNull)
            .IsRequired(false);

        _ = builder.Property(p => p.Spacing)
            .IsRequired(false);

        _ = builder.Property(p => p.SpanGaps)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Stack)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Stepped)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Tension)
            .IsRequired(false);

        _ = builder.Property(p => p.Weight)
            .IsRequired(false);

        _ = builder.Property(p => p.XAxisID)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.YAxisID)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Parsing)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Hidden)
            .IsRequired(false);

        _ = builder.Property<Guid?>("DataSourceId")
            .IsRequired(false);

        _ = builder.Property(p => p.DataSourceIndex)
            .IsRequired(false);

        _ = builder.Property(p => p.ColumnName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.XColumnName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.YColumnName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.FilterColumnName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.FilterValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.DataType)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.HasKey(k => new { k.ReportId, k.ChartId, k.Id });

        _ = builder.HasOne(p => p.DataSource)
            .WithMany()
            .HasForeignKey("DataSourceId");

        _ = builder.Navigation(p => p.DataSource)
            .AutoInclude();
    }
}
