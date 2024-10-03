using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Cupos;

namespace OzyParkAdmin.Infrastructure.Cupos;
internal sealed class CupoConfiguration : IEntityTypeConfiguration<Cupo>
{
    public void Configure(EntityTypeBuilder<Cupo> builder)
    {
        builder.ToTable("cnf_Cupos_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CupoId").ValueGeneratedNever();
        builder.Property(x => x.Total).HasColumnName("Cupo");
        builder.Property(x => x.UltimaModificacion).HasColumnType("datetime");
        builder.HasOne(x => x.EscenarioCupo).WithMany().HasForeignKey("EscenarioCupoId");
        builder.Navigation(x => x.EscenarioCupo).AutoInclude();
        builder.HasOne(x => x.CanalVenta).WithMany().HasForeignKey("CanalVentaId");
        builder.Navigation(x => x.CanalVenta).AutoInclude();
        builder.HasOne(x => x.DiaSemana).WithMany().HasForeignKey("DiaSemanaId");
        builder.Navigation(x => x.DiaSemana).AutoInclude();
    }
}
