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

    }
}