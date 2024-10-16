using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Infrastructure.Reportes.DataSources;
internal sealed class ParameterConfiguration : IEntityTypeConfiguration<Parameter>
{
    public void Configure(EntityTypeBuilder<Parameter> builder)
    {
        _ = builder.ToTable("rep_DataSourceParameters_td");

        _ = builder.Property(p => p.DataSourceId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.Property(p => p.AlternativeType)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.AlternativeTypeName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Order)
            .IsRequired(true);

        _ = builder.HasKey(k => new { k.DataSourceId, k.Name });
    }
}
