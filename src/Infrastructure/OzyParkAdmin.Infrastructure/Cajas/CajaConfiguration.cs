using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Infrastructure.Cajas;
internal sealed class CajaConfiguration : IEntityTypeConfiguration<Caja>
{
    public void Configure(EntityTypeBuilder<Caja> builder)
    {
        builder.ToTable("rec_Cajas_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CajaId").ValueGeneratedNever();

        builder.HasOne(x => x.Franquicia).WithMany().HasForeignKey("FranquiciaId");
        builder.Navigation(x => x.Franquicia).AutoInclude();
        builder.HasOne(x => x.CentroCosto).WithMany().HasForeignKey("CentroCostoId");
        builder.Navigation(x => x.CentroCosto).AutoInclude();
        builder.HasOne(x => x.PuntoVenta).WithMany().HasForeignKey("PuntoVentaId");
    }
}
