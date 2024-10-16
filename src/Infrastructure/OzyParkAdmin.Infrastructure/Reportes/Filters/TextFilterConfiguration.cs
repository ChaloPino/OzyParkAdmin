using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Infrastructure.Reportes.Filters;
internal sealed class TextFilterConfiguration : IEntityTypeConfiguration<TextFilter>
{
    public void Configure(EntityTypeBuilder<TextFilter> builder)
    {
        _ = builder.ToTable("rep_TextFilters_td");

        _ = builder.Property(p => p.Placeholder)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.MaxLength)
            .IsRequired(false);

        _ = builder.Property(p => p.Pattern)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.DefaultValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);
    }
}
