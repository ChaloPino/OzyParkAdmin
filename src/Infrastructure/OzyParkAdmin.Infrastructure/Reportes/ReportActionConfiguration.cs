using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes;
internal sealed class ReportActionConfiguration : IEntityTypeConfiguration<ReportAction>
{
    public void Configure(EntityTypeBuilder<ReportAction> builder)
    {
        _ = builder.ToTable("rep_ReportActions_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.HasKey(k => new { k.ReportId, k.Type });
    }
}
