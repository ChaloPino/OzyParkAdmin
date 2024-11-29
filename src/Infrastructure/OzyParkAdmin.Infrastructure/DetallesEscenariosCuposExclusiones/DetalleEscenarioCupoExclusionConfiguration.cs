using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;

namespace OzyParkAdmin.Infrastructure.DetallesEscenariosCuposExclusiones;
internal sealed class DetalleEscenarioCupoExclusionConfiguration : IEntityTypeConfiguration<DetalleEscenarioCupoExclusion>
{
    public void Configure(EntityTypeBuilder<DetalleEscenarioCupoExclusion> builder)
    {
        builder.ToTable("cnf_DetalleEscenarioCupoExclusion_td");

        // Definimos la clave compuesta
        builder.HasKey(x => new { x.EscenarioCupoId, x.ServicioId, x.CanalVentaId, x.DiaSemanaId, x.HoraInicio });

        // Mapeo de columnas
        builder.Property(x => x.EscenarioCupoId).HasColumnName("EscenarioCupoId");
        builder.Property(x => x.ServicioId).HasColumnName("ServicioId");
        builder.Property(x => x.CanalVentaId).HasColumnName("CanalVentaId");
        builder.Property(x => x.DiaSemanaId).HasColumnName("DiaSemanaId");
        builder.Property(x => x.HoraInicio).HasColumnName("HoraInicio");
        builder.Property(x => x.HoraFin).HasColumnName("HoraFin");

        // Relación con Servicio
        builder.HasOne(x => x.Servicio)
               .WithMany()
               .HasForeignKey(x => x.ServicioId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.Navigation(x => x.Servicio).AutoInclude();

        // Relación con CanalVenta
        builder.HasOne(x => x.CanalVenta)
               .WithMany()
               .HasForeignKey(x => x.CanalVentaId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.Navigation(x => x.CanalVenta).AutoInclude();

        // Relación con DiaSemana
        builder.HasOne(x => x.DiaSemana)
               .WithMany()
               .HasForeignKey(x => x.DiaSemanaId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.Navigation(x => x.DiaSemana).AutoInclude();


    }
}
