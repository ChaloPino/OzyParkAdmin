using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class FilterConfiguration : IEntityTypeConfiguration<Filter>
{
    public void Configure(EntityTypeBuilder<Filter> builder)
    {
        _ = builder.UseTptMappingStrategy();

        _ = builder.ToTable("rep_Filters_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .HasColumnOrder(0)
            .IsRequired(true);

        _ = builder.Property(p => p.Id)
            .HasColumnName("FilterId")
            .ValueGeneratedNever()
            .HasColumnOrder(1)
            .IsRequired(true);

        _ = builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(2)
            .IsRequired(true);

        _ = builder.Property(p => p.Label)
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(3)
            .IsRequired(false);

        _ = builder.Property(p => p.IsRequired)
            .HasColumnOrder(4)
            .IsRequired(true);

        _ = builder.Property(p => p.Order)
            .HasColumnOrder(5)
            .IsRequired(true);

        _ = builder.Property(p => p.InvalidMessage)
            .IsUnicode(false)
            .HasColumnOrder(6)
            .IsRequired(false);

        _ = builder.Property(p => p.Format)
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(7)
            .IsRequired(false);

        _ = builder.Property<int?>("ExcelFormatId")
            .HasColumnOrder(8)
            .IsRequired(false);

        _ = builder.Property(p => p.CustomExcelFormatId)
            .HasColumnOrder(9)
            .IsRequired(false);

        _ = builder.Property(p => p.CustomExcelFormat)
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(10)
            .IsRequired(false);

        _ = builder.Property<Guid>("DataSourceId")
            .HasColumnOrder(11)
            .IsRequired(true);

        _ = builder.Property<string>("ParameterName")
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasColumnOrder(12)
            .IsRequired(true);

        _ = builder.Property(p => p.Roles)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .HasColumnOrder(13)
            .IsRequired(false)
            .HasConversion<RolesConverter>();

        _ = builder.HasKey(k => new { k.ReportId, k.Id });

        _ = builder.HasOne(p => p.Parameter)
            .WithMany()
            .HasForeignKey("DataSourceId", "ParameterName");

        _ = builder.Navigation(p => p.Parameter).AutoInclude();

        _ = builder.HasOne(p => p.ExcelFormat)
            .WithMany()
            .HasForeignKey("ExcelFormatId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.ExcelFormat).AutoInclude();

        _ = builder.OwnsMany<ParentFilter>("_parentFilters", navBuilder =>
        {
            navBuilder.ToTable("rep_DependantFilters_td");
            navBuilder.Property<Guid>("ReportId");
            navBuilder.Property<int>("ListFilterId");
            navBuilder.HasKey("ReportId", "ListFilterId", "FilterId");
            navBuilder.WithOwner().HasForeignKey("ReportId", "FilterId");
        });


        _ = builder.Ignore(p => p.CurrentExcelFormat);
        _ = builder.Ignore(p => p.HasMoreParameters);
        _ = builder.Ignore(p => p.ParentFilters);
    }
}
