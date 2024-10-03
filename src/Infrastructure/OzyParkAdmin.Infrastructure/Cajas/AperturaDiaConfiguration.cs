using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Infrastructure.Cajas;

internal sealed class AperturaDiaConfiguration : IEntityTypeConfiguration<AperturaDia>
{
    public void Configure(EntityTypeBuilder<AperturaDia> builder)
    {
        builder.ToTable("rec_AperturasCaja_to");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("AperturaCajaId");

        builder.HasOne(x => x.Caja).WithMany().HasForeignKey("CajaId");

        builder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UserId");

        builder.HasOne(x => x.Supervisor).WithMany().HasForeignKey("SupervisorId").IsRequired(false);

        builder.Property(x => x.FechaApertura).HasColumnType("datetime");
        builder.Property(x => x.FechaCierre).HasColumnType("datetime");

        builder.OwnsMany(x => x.Turnos, navBuilder =>
        {
            navBuilder.ToTable("rec_AperturaTurnos_to");
            navBuilder.Property<Guid>("AperturaCajaId");
            navBuilder.HasKey("AperturaCajaId", "Id");
            navBuilder.Property(x => x.Id).HasColumnName("AperturaTurnoId");
            navBuilder.HasOne(x => x.Gaveta).WithMany().HasForeignKey("GavetaId");
            navBuilder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UserId");
            navBuilder.Property(x => x.FechaApertura).HasColumnType("datetime");
            navBuilder.Property(x => x.FechaSistema).HasColumnType("datetime");
            navBuilder.Property(x => x.FechaCierre).HasColumnType("datetime");
            navBuilder.HasOne(x => x.Supervisor).WithMany().HasForeignKey("SupervisorId").IsRequired(false);
            navBuilder.WithOwner().HasForeignKey("AperturaCajaId");

            navBuilder.OwnsMany(x => x.Movimientos, movBuilder =>
            {
                movBuilder.ToTable("rec_MovimientosTurno_to");
                movBuilder.Property<Guid>("AperturaCajaId");
                movBuilder.Property<Guid>("AperturaTurnoId");
                movBuilder.HasKey("AperturaCajaId", "AperturaTurnoId", "Correlativo");
                movBuilder.Property(x => x.TipoTurnoMovimiento).HasColumnName("TipoMovimientoTurnoId");
                movBuilder.Property(x => x.Fecha).HasColumnType("datetime");
                movBuilder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UserId");
                movBuilder.HasOne(x => x.Supervisor).WithMany().HasForeignKey("SupervisorId").IsRequired(false);
                movBuilder.WithOwner().HasForeignKey("AperturaCajaId", "AperturaTurnoId");
            });
        });
    }
}
