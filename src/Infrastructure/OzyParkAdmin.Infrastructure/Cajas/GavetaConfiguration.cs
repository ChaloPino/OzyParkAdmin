using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Infrastructure.Cajas;
internal sealed class GavetaConfiguration : IEntityTypeConfiguration<Gaveta>
{
    public void Configure(EntityTypeBuilder<Gaveta> builder)
    {
        builder.ToTable("rec_Gavetas_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("GavetaId").ValueGeneratedNever();
        builder.HasOne(x => x.Caja).WithMany(x => x.Gavetas).HasForeignKey("CajaId");
    }
}
