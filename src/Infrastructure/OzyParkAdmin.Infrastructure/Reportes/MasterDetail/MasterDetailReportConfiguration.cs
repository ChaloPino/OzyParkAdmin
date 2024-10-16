using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.MasterDetails;

namespace OzyParkAdmin.Infrastructure.Reportes.MasterDetail;
internal sealed class MasterDetailReportConfiguration : IEntityTypeConfiguration<MasterDetailReport>
{
    public void Configure(EntityTypeBuilder<MasterDetailReport> builder)
    {
        _ = builder.ToTable("rep_MasterDetailReports_td");

        _ = builder.Property(p => p.HasDetail)
            .IsRequired(true);

        _ = builder.Property(p => p.MasterResultIndex)
            .IsRequired(false);

        _ = builder.Property(p => p.IsTabular)
            .IsRequired(true);

        _ = builder.Property(p => p.TitleInReport)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.HasDynamicDetails)
            .IsRequired(true);

        _ = builder.HasMany(p => p.Details)
            .WithOne()
            .HasForeignKey(p => p.ReportId);

        _ = builder.Navigation(p => p.Details)
            .HasField("_details")
            .AutoInclude();
    }
}
