using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;

internal sealed class ListFilterConfiguration : IEntityTypeConfiguration<ListFilter>
{
    public void Configure(EntityTypeBuilder<ListFilter> builder)
    {
        _ = builder.ToTable("rep_ListFilters_td");

        _ = builder.Property(p => p.OptionalValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.IsMultiple)
            .IsRequired(true);

        _ = builder.Property(p => p.Size)
            .IsRequired(false);

        _ = builder.Property(p => p.IsRequired)
            .IsRequired(true);

        _ = builder.Property<Guid?>("RemoteDataSourceId")
            .IsRequired(false);

        _ = builder.HasOne(p => p.RemoteDataSource)
            .WithMany()
            .HasForeignKey("RemoteDataSourceId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.RemoteDataSource)
            .AutoInclude();

        _ = builder.OwnsMany<FilterData>("_list", navBuilder =>
        {
            _ = navBuilder.ToTable("rep_FilterData_td");

            _ = navBuilder.Property(p => p.ReportId)
                .ValueGeneratedNever()
                .IsRequired(true);

            _ = navBuilder.Property(p => p.FilterId)
                .ValueGeneratedNever()
                .IsRequired(true);

            _ = navBuilder.Property(p => p.Value)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired(true);

            _ = navBuilder.Property(p => p.Display)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired(true);

            _ = navBuilder.Property(p => p.Order)
                .IsRequired(true);

            _ = navBuilder.HasKey(k => new { k.ReportId, k.FilterId, k.Value });

            _ = navBuilder.WithOwner().HasForeignKey(p => new { p.ReportId, p.FilterId });
        });

        _ = builder.Ignore(p => p.List);
    }
}
