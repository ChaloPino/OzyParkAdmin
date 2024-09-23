using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Infrastructure.Zonas;
internal sealed class ZonaConfiguration : IEntityTypeConfiguration<Zona>
{
    public void Configure(EntityTypeBuilder<Zona> builder)
    {
        builder.ToTable("tkt_Zonas_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ZonaId").ValueGeneratedNever();
    }
}
