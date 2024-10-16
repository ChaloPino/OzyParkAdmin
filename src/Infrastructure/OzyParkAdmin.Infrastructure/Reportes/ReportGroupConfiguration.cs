using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes;
internal sealed class ReportGroupConfiguration : IEntityTypeConfiguration<ReportGroup>
{
    public void Configure(EntityTypeBuilder<ReportGroup> builder)
    {
        _ = builder.ToTable("rep_ReportGroups_td");

        _ = builder.Property(p => p.Id)
            .HasColumnName("ReportGroupId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Order)
            .IsRequired(true);

        _ = builder.HasKey(k => k.Id);
    }
}
