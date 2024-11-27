using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Infrastructure.TarifasServicio;
internal sealed class TarifaServicioConfiguration : IEntityTypeConfiguration<TarifaServicio>
{
    public void Configure(EntityTypeBuilder<TarifaServicio> builder)
    {
        builder.ToTable("tkt_Tarifas_td");

        builder.Property<int>("MonedaId").ValueGeneratedNever();
        builder.Property<int>("ServicioId").ValueGeneratedNever();
        builder.Property<int>("TramoId").ValueGeneratedNever();
        builder.Property<int>("GrupoEtarioId").ValueGeneratedNever();
        builder.Property<int>("TipoDiaId").ValueGeneratedNever();
        builder.Property<int>("TipoHorarioId").ValueGeneratedNever();
        builder.Property<int>("CanalVentaId").ValueGeneratedNever();
        builder.Property<int>("TipoSegmentacionId").ValueGeneratedNever();

        builder.HasKey("InicioVigencia", "MonedaId", "ServicioId", "TramoId", "GrupoEtarioId", "TipoDiaId", "TipoHorarioId", "CanalVentaId", "TipoSegmentacionId");

        builder.HasOne(x => x.Moneda)
            .WithMany()
            .HasForeignKey("MonedaId");

        builder.Navigation(x => x.Moneda).AutoInclude(true);

        builder.HasOne(x => x.Servicio)
            .WithMany()
            .HasForeignKey("ServicioId");

        builder.Navigation(x => x.Servicio).AutoInclude(true);

        builder.HasOne(x => x.Tramo)
            .WithMany()
            .HasForeignKey("TramoId");

        builder.Navigation(x => x.Tramo).AutoInclude(true);

        builder.HasOne(x => x.GrupoEtario)
            .WithMany()
            .HasForeignKey("GrupoEtarioId");

        builder.Navigation(x => x.GrupoEtario).AutoInclude(true);

        builder.HasOne(x => x.TipoDia)
            .WithMany()
            .HasForeignKey("TipoDiaId");

        builder.Navigation(x => x.TipoDia).AutoInclude(true);

        builder.HasOne(x => x.TipoHorario)
            .WithMany()
            .HasForeignKey("TipoHorarioId");

        builder.Navigation(x => x.TipoHorario).AutoInclude(true);

        builder.HasOne(x => x.CanalVenta)
            .WithMany()
            .HasForeignKey("CanalVentaId");

        builder.Navigation(x => x.CanalVenta).AutoInclude(true);

        builder.HasOne(x => x.TipoSegmentacion)
            .WithMany()
            .HasForeignKey("TipoSegmentacionId");

        builder.Navigation(x => x.TipoSegmentacion).AutoInclude(true);

        builder.Property(x => x.ValorAfecto)
            .HasPrecision(18, 2);

        builder.Property(x => x.ValorExento)
            .HasPrecision(18, 2);

        builder.Property(x => x.Valor)
            .HasPrecision(18, 2);
    }
}
