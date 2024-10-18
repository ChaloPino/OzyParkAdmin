using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.CuposFecha;

namespace OzyParkAdmin.Infrastructure.CuposFecha;
internal sealed class CupoFechaConfiguration : IEntityTypeConfiguration<CupoFecha>
{
    public void Configure(EntityTypeBuilder<CupoFecha> builder)
    {
        builder.ToTable("cnf_CuposPorFecha_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CupoPorFechaId");
        builder.Property(x => x.Total).HasColumnName("Cupo");
        builder.HasOne(x => x.EscenarioCupo).WithMany().HasForeignKey("EscenarioCupoId");
        builder.Navigation(x => x.EscenarioCupo).AutoInclude();
        builder.HasOne(x => x.CanalVenta).WithMany().HasForeignKey("CanalVentaId");
        builder.Navigation(x => x.CanalVenta).AutoInclude();
        builder.HasOne(x => x.DiaSemana).WithMany().HasForeignKey("DiaSemanaId");
        builder.Navigation(x => x.DiaSemana).AutoInclude();
    }
}
