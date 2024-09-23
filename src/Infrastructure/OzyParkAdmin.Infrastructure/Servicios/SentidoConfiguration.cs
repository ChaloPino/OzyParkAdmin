using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Infrastructure.Servicios;

internal sealed class SentidoConfiguration : IEntityTypeConfiguration<Sentido>
{
    public void Configure(EntityTypeBuilder<Sentido> builder)
    {
        builder.ToTable("tkt_ZonasRutaSentidos_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ZonaRutaSentidoId");
    }
}
