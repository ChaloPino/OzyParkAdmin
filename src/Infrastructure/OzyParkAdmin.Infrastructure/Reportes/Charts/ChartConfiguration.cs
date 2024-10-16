using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Charts;

namespace OzyParkAdmin.Infrastructure.Reportes.Charts;
internal sealed class ChartConfiguration : IEntityTypeConfiguration<Chart>
{
    public void Configure(EntityTypeBuilder<Chart> builder)
    {
        _ = builder.ToTable("rep_Charts_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Id)
            .HasColumnName("ChartId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property<Guid?>("LabelDataSourceId")
            .IsRequired(false);

        _ = builder.Property(p => p.LabelDataSourceIndex)
            .IsRequired(false);

        _ = builder.Property(p => p.Labels)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.LabelColumnName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property<Guid?>("TableDataSourceId")
            .IsRequired(false);

        _ = builder.Property(p => p.TableDataSourceIndex)
            .IsRequired(false);

        _ = builder.Property(p => p.Parsing)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Animations)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Decimation)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Interaction)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Layout)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Legend)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Responsive)
            .IsRequired(false);

        _ = builder.Property(p => p.MaintainAspectRatio)
            .IsRequired(false);

        _ = builder.Property(p => p.AspectRatio)
            .IsRequired(false);

        _ = builder.Property(p => p.ResizeDelay)
            .IsRequired(false);

        _ = builder.Property(p => p.Width)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Height)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Tooltip)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.DataLabels)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Title)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Subtitle)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Scales)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Roles)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired(false)
            .HasConversion<RolesConverter>();

        _ = builder.HasKey(k => new { k.ReportId, k.Id });

        _ = builder.HasMany(p => p.DataSets)
            .WithOne()
            .HasForeignKey(p => new { p.ReportId, p.ChartId });

        _ = builder.HasOne(p => p.LabelDataSource)
            .WithMany()
            .HasForeignKey("LabelDataSourceId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.LabelDataSource).AutoInclude();

        _ = builder.HasOne(p => p.TableDataSource)
            .WithMany()
            .HasForeignKey("TableDataSourceId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.TableDataSource).AutoInclude();

        _ = builder.HasMany(p => p.Columns)
            .WithOne()
            .HasForeignKey(p => new { p.ReportId, p.ChartId });

        _ = builder.Navigation(p => p.DataSets)
            .HasField("_dataSets")
            .AutoInclude();

        _ = builder.Navigation(p => p.Columns)
            .HasField("_columns")
            .AutoInclude();
    }
}
