using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Ventas;

namespace OzyParkAdmin.Infrastructure.Ventas;

internal sealed class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("vnt_Ventas_to");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("VentaId");
    }
}
