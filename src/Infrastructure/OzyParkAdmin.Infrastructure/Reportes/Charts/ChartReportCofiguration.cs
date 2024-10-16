using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Charts;

namespace OzyParkAdmin.Infrastructure.Reportes.Charts;
internal sealed class ChartReportCofiguration : IEntityTypeConfiguration<ChartReport>
{
    public void Configure(EntityTypeBuilder<ChartReport> builder)
    {
        _ = builder.ToTable("rep_ChartReports_td");

        _ = builder.Property(p => p.Layout)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.ChartsPerRow)
            .IsRequired(false);

        _ = builder.HasMany(p => p.Charts)
            .WithOne()
            .HasForeignKey(p => p.ReportId);

        _ = builder.Navigation(p => p.Charts)
            .HasField("_charts")
            .AutoInclude();
    }
}
