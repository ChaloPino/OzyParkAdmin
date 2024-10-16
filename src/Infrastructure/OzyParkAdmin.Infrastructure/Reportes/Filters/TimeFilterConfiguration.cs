using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class TimeFilterConfiguration : IEntityTypeConfiguration<TimeFilter>
{
    public void Configure(EntityTypeBuilder<TimeFilter> builder)
    {
        _ = builder.ToTable("rep_TimeFilters_td");

        _ = builder.Property(p => p.MinTime)
            .IsRequired(false);

        _ = builder.Property(p => p.MaxTime)
            .IsRequired(false);

        _ = builder.Property(p => p.UseNow)
            .IsRequired(true);

        _ = builder.Property(p => p.Placeholder)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.StepMinutes)
            .IsRequired(false);

        _ = builder.Property(p => p.DefaultValue)
            .IsRequired(false);

        _ = builder.OwnsOne<MinTimeFilter>("_minTimeFilter", navBuilder =>
        {
            _ = navBuilder.ToTable("rep_MinTimeFilters_td");

            _ = navBuilder.Property<Guid>("ReportId")
               .ValueGeneratedNever()
               .IsRequired(true);

            _ = navBuilder.Property<int>("FilterId")
                .ValueGeneratedNever()
                .IsRequired(true);

            _ = navBuilder.Property<int>("MinFilterId")
                .ValueGeneratedNever()
                .IsRequired(true);

            _ = navBuilder.HasOne(p => p.Filter)
                .WithMany()
                .HasForeignKey("ReportId", "MinFilterId")
                .OnDelete(DeleteBehavior.NoAction);

            _ = navBuilder.HasKey("ReportId", "FilterId", "MinFilterId");

            _ = navBuilder.WithOwner()
                .HasForeignKey("ReportId", "FilterId");
        });

        _ = builder.OwnsOne<MaxTimeFilter>("_maxTimeFilter", navBuilder =>
        {
            _ = navBuilder.ToTable("rep_MaxTimeFilters_td");

            _ = navBuilder.Property<Guid>("ReportId")
               .ValueGeneratedNever()
               .IsRequired(true);

            _ = navBuilder.Property<int>("FilterId")
                .ValueGeneratedNever()
                .IsRequired(true);

            _ = navBuilder.Property<int>("MaxFilterId")
                .ValueGeneratedNever()
                .IsRequired(true);

            _ = navBuilder.HasOne(p => p.Filter)
                .WithMany()
                .HasForeignKey("ReportId", "MaxFilterId")
                .OnDelete(DeleteBehavior.NoAction);

            _ = navBuilder.HasKey("ReportId", "FilterId", "MaxFilterId");

            _ = navBuilder.WithOwner()
                .HasForeignKey("ReportId", "FilterId");
        });
    }
}
