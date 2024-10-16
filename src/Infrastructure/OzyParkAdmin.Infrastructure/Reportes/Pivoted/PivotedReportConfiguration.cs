using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Pivoted;

namespace OzyParkAdmin.Infrastructure.Reportes.Pivoted;
internal sealed class PivotedReportConfiguration : IEntityTypeConfiguration<PivotedReport>
{
    public void Configure(EntityTypeBuilder<PivotedReport> builder)
    {
        _ = builder.ToTable("rep_PivotedReports_td");

        _ = builder.Property(p => p.IncludeTotalsRow)
            .IsRequired(true);

        _ = builder.Property(p => p.IncludeTotalsColumn)
            .IsRequired(true);

        _ = builder.Property(p => p.IncludeGrandTotal)
            .IsRequired(true);

        _ = builder.HasMany(p => p.PivotedMembers)
            .WithOne()
            .HasForeignKey(p => p.ReportId)
            .OnDelete(DeleteBehavior.NoAction);

        _ = builder.Navigation(p => p.PivotedMembers)
            .HasField("_pivotedMembers")
            .AutoInclude();
    }
}
