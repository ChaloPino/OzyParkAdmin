using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Listed;

namespace OzyParkAdmin.Infrastructure.Reportes.Lists;
internal sealed class PaginatedReportConfiguration : IEntityTypeConfiguration<PaginatedReport>
{
    public void Configure(EntityTypeBuilder<PaginatedReport> builder)
    {
        _ = builder.ToTable("rep_PaginatedReports_td");

        _ = builder.Property(p => p.PageSize)
            .IsRequired(true);

        _ = builder.Property(p => p.Pages)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true)
            .HasConversion<PagesConverter>();

        _ = builder.Property(p => p.IsPaginationInDatabase)
            .IsRequired(true);

        _ = builder.Property(p => p.TotalRecordsResultIndex)
            .IsRequired(false);

        _ = builder.Property(p => p.DataResultIndex)
            .IsRequired(false);

        _ = builder.Property(p => p.IsSortingInDatabase)
            .IsRequired(true);

        _ = builder.Property<Guid?>("TotalRecordsDataSourceId")
            .IsRequired(false);

        _ = builder.Property<Guid?>("StartDataSourceId")
            .IsRequired(false);

        _ = builder.Property<string>("StartParameterName")
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property<Guid?>("LengthDataSourceId")
            .IsRequired(false);

        _ = builder.Property<string>("LengthParameterName")
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.HasOne(p => p.TotalRecordsDataSource)
            .WithMany()
            .HasForeignKey("TotalRecordsDataSourceId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.TotalRecordsDataSource).AutoInclude();

        _ = builder.HasOne(p => p.StartParameter)
            .WithMany()
            .HasForeignKey("StartDataSourceId", "StartParameterName")
            .IsRequired(false);

        _ = builder.Navigation(p => p.StartParameter).AutoInclude();

        _ = builder.HasOne(p => p.LengthParameter)
            .WithMany()
            .HasForeignKey("LengthDataSourceId", "LengthParameterName")
            .IsRequired(false);

        _ = builder.Navigation(p => p.LengthParameter).AutoInclude();
    }
}
