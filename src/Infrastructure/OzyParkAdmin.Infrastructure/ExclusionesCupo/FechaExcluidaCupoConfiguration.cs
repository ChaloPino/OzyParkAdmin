using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.ExclusionesCupo;

namespace OzyParkAdmin.Infrastructure.ExclusionesCupo;
internal sealed class FechaExcluidaCupoConfiguration : IEntityTypeConfiguration<FechaExcluidaCupo>
{
    public void Configure(EntityTypeBuilder<FechaExcluidaCupo> builder)
    {
        builder.ToTable("cnf_FechasExcluidasCupo_td");
        builder.Property<int>("CentroCostoId");
        builder.Property<int>("CanalVentaId");
        builder.HasKey("CentroCostoId", "CanalVentaId", "Fecha");

        builder.Property(x => x.Fecha).HasColumnName("FechaExcluida");

        builder.HasOne(x => x.CentroCosto)
            .WithMany()
            .HasForeignKey("CentroCostoId");

        builder.HasOne(x => x.CanalVenta)
            .WithMany()
            .HasForeignKey("CanalVentaId");
    }
}
