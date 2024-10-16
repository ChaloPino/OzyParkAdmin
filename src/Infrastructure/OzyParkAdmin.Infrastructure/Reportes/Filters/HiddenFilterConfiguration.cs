using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class HiddenFilterConfiguration : IEntityTypeConfiguration<HiddenFilter>
{
    public void Configure(EntityTypeBuilder<HiddenFilter> builder)
    {
        _ = builder.ToTable("rep_HiddenFilters_td");

        _ = builder.Property(p => p.DefaultValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.MappedTo)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);
    }
}
