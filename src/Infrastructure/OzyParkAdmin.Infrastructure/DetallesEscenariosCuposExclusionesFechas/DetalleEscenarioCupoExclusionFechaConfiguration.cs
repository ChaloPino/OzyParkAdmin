using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Infrastructure.DetallesEscenariosCuposExclusionesFechas;

internal sealed class DetalleEscenarioCupoExclusionFechaConfiguration : IEntityTypeConfiguration<DetalleEscenarioCupoExclusionFecha>
{
    public void Configure(EntityTypeBuilder<DetalleEscenarioCupoExclusionFecha> builder)
    {
        builder.ToTable("cnf_DetalleEscenarioCupoExclusionFecha_td");

        // Definimos la clave compuesta
        builder.HasKey(x => new { x.EscenarioCupoId, x.ServicioId, x.CanalVentaId, x.FechaExclusion, x.HoraInicio });

        // Mapeo de columnas
        builder.Property(x => x.EscenarioCupoId).HasColumnName("EscenarioCupoId");
        builder.Property(x => x.ServicioId).HasColumnName("ServicioId");
        builder.Property(x => x.CanalVentaId).HasColumnName("CanalVentaId");
        builder.Property(x => x.FechaExclusion).HasColumnName("FechaExclusion");
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

        // Relación con EscenarioCupo
        builder.HasOne(x => x.EscenarioCupo)
               .WithMany(e => e.ExclusionesPorFecha)
               .HasForeignKey(x => x.EscenarioCupoId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}