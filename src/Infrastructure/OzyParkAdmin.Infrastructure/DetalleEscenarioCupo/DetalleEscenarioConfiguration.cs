using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;

namespace OzyParkAdmin.Infrastructure.DetallesEscenariosCupos;

internal sealed class DetalleEscenarioCupoConfiguration : IEntityTypeConfiguration<DetalleEscenarioCupo>
{
    public void Configure(EntityTypeBuilder<DetalleEscenarioCupo> builder)
    {
        builder.ToTable("cnf_DetalleEscenarioCupo_td");

        // Definimos la clave compuesta
        builder.HasKey(x => new { x.EscenarioCupoId, x.ServicioId });

        // Mapeo de columnas
        builder.Property(x => x.EscenarioCupoId).HasColumnName("EscenarioCupoId");
        builder.Property(x => x.ServicioId).HasColumnName("ServicioId");
        builder.Property(x => x.TopeDiario).HasColumnName("TopeDiario");
        builder.Property(x => x.UsaSobreCupo).HasColumnName("UsaSobreCupo").HasDefaultValue(false);
        builder.Property(x => x.HoraMaximaVenta).HasColumnName("HoraMaximaVenta").HasDefaultValue(new TimeSpan(18, 40, 0));
        builder.Property(x => x.HoraMaximaRevalidacion).HasColumnName("HoraMaximaRevalidacion").HasDefaultValue(new TimeSpan(18, 40, 0));
        builder.Property(x => x.UsaTopeEnCupo).HasColumnName("UsaTopeEnCupo").HasDefaultValue(false);
        builder.Property(x => x.TopeFlotante).HasColumnName("TopeFlotante").HasDefaultValue(false);

        // Relación con Servicio
        builder.HasOne(x => x.Servicio)
               .WithMany()
               .HasForeignKey(x => x.ServicioId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        //builder.Navigation(x => x.Servicio).AutoInclude(false);

        //builder.Navigation(x => x.EscenarioCupo).AutoInclude();
    }
}
