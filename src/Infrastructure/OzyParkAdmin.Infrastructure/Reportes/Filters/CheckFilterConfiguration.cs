using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class CheckFilterConfiguration : IEntityTypeConfiguration<CheckFilter>
{
    public void Configure(EntityTypeBuilder<CheckFilter> builder)
    {
        _ = builder.ToTable("rep_CheckFilters_td");

        _ = builder.Property(p => p.Checked)
            .IsRequired(true);
    }
}
