using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.CanalesVenta;

namespace OzyParkAdmin.Infrastructure.CanalesVenta;
internal sealed class CanalVentaConfiguration : IEntityTypeConfiguration<CanalVenta>
{
    public void Configure(EntityTypeBuilder<CanalVenta> builder)
    {
        builder.ToTable("cnf_CanalesVenta_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CanalVentaId").ValueGeneratedNever();
    }
}
