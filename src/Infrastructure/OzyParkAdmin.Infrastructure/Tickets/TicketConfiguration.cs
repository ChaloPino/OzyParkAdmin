using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Tickets;

namespace OzyParkAdmin.Infrastructure.Tickets;
internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tkt_Tickets_to");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TicketId");
        builder.Property(x => x.FechaImpresion).HasColumnType("datetime");
        builder.Property(x => x.FechaReimpresion).HasColumnType("datetime");
        builder.Property(x => x.InicioVigencia).HasColumnType("datetime");
        builder.Property(x => x.FinVigencia).HasColumnType("datetime");
        builder.Property(x => x.SentidoId).HasColumnName("ZonaRutaSentidoId");
        builder.HasOne(x => x.Servicio).WithMany().HasForeignKey(x => x.ServicioId);
        builder.HasOne(x => x.Tramo).WithMany().HasForeignKey(x => x.TramoId);
        builder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UserId");
        builder.HasOne(x => x.TipoPasajero).WithMany().HasForeignKey(x => x.TipoPasajeroId).IsRequired(false);
        builder.HasOne(x => x.ZonaOrigen).WithMany().HasForeignKey(x => x.ZonaOrigenId).IsRequired(false);
        builder.HasOne(x => x.ZonaDestino).WithMany().HasForeignKey(x => x.ZonaDestinoId).IsRequired(false);
        builder.HasOne(x => x.ZonaCupoOrigen).WithMany().HasForeignKey(x => x.ZonaCupoOrigenId).IsRequired(false);
        builder.HasOne(x => x.Sentido).WithMany().HasForeignKey(x => x.SentidoId).IsRequired(false);
        
        builder.OwnsMany(x => x.Detalle, navBuilder =>
        {
            navBuilder.ToTable("tkt_DetallesTicket_to");
            navBuilder.Property<string>("TicketId");
            navBuilder.HasKey("TicketId", "GrupoEtarioId");
            navBuilder.HasOne(x => x.GrupoEtario).WithMany().HasForeignKey(x => x.GrupoEtarioId);
            navBuilder.WithOwner().HasForeignKey("TicketId");
        });

        builder.OwnsMany<TicketReimpreso>("_reimpresiones", navBuilder =>
        {
            navBuilder.ToTable("tkt_TicketsReimpresos_to");
            navBuilder.HasKey(x => new { x.TicketId, x.FechaReimpresion });
            navBuilder.Property(x => x.FechaReimpresion).HasColumnType("datetime");
            navBuilder.WithOwner().HasForeignKey(x => x.TicketId);
        });
    }
}
