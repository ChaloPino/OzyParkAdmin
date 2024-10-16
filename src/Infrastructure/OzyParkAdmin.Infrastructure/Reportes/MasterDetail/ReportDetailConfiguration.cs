using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.MasterDetails;

namespace OzyParkAdmin.Infrastructure.Reportes.MasterDetail;
internal sealed class ReportDetailConfiguration : IEntityTypeConfiguration<ReportDetail>
{
    public void Configure(EntityTypeBuilder<ReportDetail> builder)
    {
        _ = builder.ToTable("rep_ReportDetails_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.DetailId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.DetailResultSetIndex)
            .IsRequired(false);

        _ = builder.Property<Guid?>("DetailDataSourceId")
            .IsRequired(false);

        _ = builder.Property(p => p.IsTabular)
            .IsRequired(true);

        _ = builder.Property(p => p.Title)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.HasKey(k => new { k.ReportId, k.DetailId });

        _ = builder.HasMany(p => p.DetailColumns)
            .WithOne()
            .HasForeignKey(p => new { p.ReportId, p.DetailId });

        _ = builder.HasOne(p => p.DetailDataSource)
            .WithMany()
            .HasForeignKey("DetailDataSourceId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.DetailDataSource);

        _ = builder.Navigation(p => p.DetailColumns)
            .HasField("_detailColumns")
            .AutoInclude();
    }
}
