using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Infrastructure.Reportes.DataSources;
internal sealed class DataSourceConfiguration : IEntityTypeConfiguration<DataSource>
{
    public void Configure(EntityTypeBuilder<DataSource> builder)
    {
        _ = builder.ToTable("rep_DataSources_td");

        _ = builder.Property(p => p.Id)
            .HasColumnName("DataSourceId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.ConnectionString)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.ProviderName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Script)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.IsStoredProcedure)
            .IsRequired(true);

        _ = builder.Property(p => p.CreationDate)
            .IsRequired(true);

        _ = builder.Property(p => p.LastUpdate)
            .IsRequired(true);

        _ = builder.Property(p => p.Timestamp)
            .IsRowVersion()
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired(true);

        _ = builder.HasKey(k => k.Id);

        _ = builder.HasMany(p => p.Parameters)
            .WithOne()
            .HasForeignKey(p => p.DataSourceId);

        _ = builder.Navigation(p => p.Parameters)
            .HasField("_parameters")
            .AutoInclude();
    }
}
