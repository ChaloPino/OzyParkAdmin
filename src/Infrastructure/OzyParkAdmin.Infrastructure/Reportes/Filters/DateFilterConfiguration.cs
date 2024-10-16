using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class DateFilterConfiguration : IEntityTypeConfiguration<DateFilter>
{
    public void Configure(EntityTypeBuilder<DateFilter> builder)
    {
        _ = builder.ToTable("rep_DateFilters_td");

        _ = builder.Property(p => p.MinDate)
            .HasColumnType("date")
            .IsRequired(false);

        _ = builder.Property(p => p.MaxDate)
            .HasColumnType("date")
            .IsRequired(false);

        _ = builder.Property(p => p.UseToday)
            .IsRequired(true);

        _ = builder.Property(p => p.Placeholder)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.UseLastTimeOfDay)
            .IsRequired(true);

        _ = builder.OwnsOne<MinDateFilter>("_minDateFilter", navBuilder =>
        {
            _ = navBuilder.ToTable("rep_MinDateFilters_td");

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

        _ = builder.OwnsOne<MaxDateFilter>("_maxDateFilter", navBuilder =>
        {
            _ = navBuilder.ToTable("rep_MaxDateFilters_td");

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
