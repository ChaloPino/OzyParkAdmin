using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.PuntosVenta;

namespace OzyParkAdmin.Infrastructure.PuntosVenta;
internal sealed class PuntoVentaConfiguration : IEntityTypeConfiguration<PuntoVenta>
{
    public void Configure(EntityTypeBuilder<PuntoVenta> builder)
    {
        builder.ToTable("mkt_PuntosVenta_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("PuntoVentaId").ValueGeneratedNever();
    }
}
