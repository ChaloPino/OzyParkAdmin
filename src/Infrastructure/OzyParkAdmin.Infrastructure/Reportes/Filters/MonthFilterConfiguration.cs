using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class MonthFilterConfiguration : IEntityTypeConfiguration<MonthFilter>
{
    public void Configure(EntityTypeBuilder<MonthFilter> builder)
    {
        _ = builder.ToTable("rep_MonthFilters_td");

        _ = builder.Property(p => p.UseToday)
            .HasColumnOrder(14)
            .IsRequired(true);

        _ = builder.Property<Guid?>("ToDataSourceId")
            .HasColumnOrder(15)
            .IsRequired(false);

        _ = builder.Property<string>("ToParameterName")
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(16)
            .IsRequired(false);

        _ = builder.Property(p => p.Placeholder)
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(17)
            .IsRequired(false);

        _ = builder.HasOne(p => p.ToParameter)
            .WithMany()
            .HasForeignKey("ToDataSourceId", "ToParameterName")
            .IsRequired(false);

        _ = builder.Navigation(p => p.ToParameter).AutoInclude();

        _ = builder.Ignore(p => p.HasMoreParameters);
    }
}
