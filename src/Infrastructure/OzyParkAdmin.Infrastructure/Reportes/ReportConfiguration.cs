using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes;
internal sealed class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        _ = builder.UseTptMappingStrategy();

        _ = builder.ToTable("rep_Reports_td");

        _ = builder.Property(p => p.Id)
            .HasColumnName("ReportId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Aka)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Title)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.Property<Guid>("DataSourceId")
            .IsRequired(true);

        _ = builder.Property(p => p.CanSort)
            .IsRequired(true);

        _ = builder.Property(p => p.FilterLayout)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.ServerSide)
            .IsRequired(true);

        _ = builder.Property(p => p.Order)
            .IsRequired(true);

        _ = builder.Property(p => p.Published)
            .IsRequired(true);

        _ = builder.Property(p => p.Roles)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired(false)
            .HasConversion<RolesConverter>();

        _ = builder.Property<int?>("ReportGroupId")
            .IsRequired(false);

        _ = builder.Property(p => p.ForDashboard)
            .IsRequired(true);

        _ = builder.Property(p => p.CreationDate)
            .IsRequired(true);

        _ = builder.Property(p => p.LastUpdate)
            .IsRequired(true);

        _ = builder.Property(p => p.Timestamp)
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired(true);

        _ = builder.HasKey(k => k.Id);

        _ = builder.HasOne(p => p.DataSource)
            .WithMany()
            .HasForeignKey("DataSourceId");

        _ = builder.Navigation(p => p.DataSource)
            .AutoInclude();

        _ = builder.HasMany(p => p.Filters)
            .WithOne()
            .HasForeignKey(p => p.ReportId)
            .OnDelete(DeleteBehavior.NoAction);

        _ = builder.HasMany(p => p.Templates)
            .WithOne()
            .HasForeignKey(p => p.ReportId)
            .OnDelete(DeleteBehavior.NoAction);

        _ = builder.HasMany(p => p.Columns)
            .WithOne()
            .HasForeignKey(p => p.ReportId);

        _ = builder.HasMany(p => p.Actions)
            .WithOne()
            .HasForeignKey(p => p.ReportId);

        _ = builder.HasOne(p => p.ReportGroup)
            .WithMany()
            .HasForeignKey("ReportGroupId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.ReportGroup)
            .AutoInclude();

        _ = builder.Navigation(p => p.Filters)
            .HasField("_filters")
            .AutoInclude();

        _ = builder.Navigation(p => p.Columns)
            .HasField("_columns")
            .AutoInclude();

        _ = builder.Navigation(p => p.Templates)
            .HasField("_templates")
            .AutoInclude();

        _ = builder.Navigation(p => p.Actions)
            .HasField("_actions")
            .AutoInclude();
    }
}
