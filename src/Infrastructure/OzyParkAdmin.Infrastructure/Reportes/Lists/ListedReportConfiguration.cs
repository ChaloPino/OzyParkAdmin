using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Listed;

namespace OzyParkAdmin.Infrastructure.Reportes.Lists;
internal sealed class ListedReportConfiguration : IEntityTypeConfiguration<ListedReport>
{
    public void Configure(EntityTypeBuilder<ListedReport> builder)
    {
        _ = builder.ToTable("rep_ListedReports_td");

        _ = builder.Property(p => p.IsSortingInDatabase)
            .IsRequired(true);
    }
}
