using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Tickets;

namespace OzyParkAdmin.Infrastructure.Tickets;
internal sealed class TipoPasajeroConfiguration : IEntityTypeConfiguration<TipoPasajero>
{
    public void Configure(EntityTypeBuilder<TipoPasajero> builder)
    {
        builder.ToTable("tkt_TiposPasajeros_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoPasajeroId").ValueGeneratedNever();
    }
}
