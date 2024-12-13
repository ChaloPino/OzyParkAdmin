using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Infrastructure.TarifasProducto;
internal sealed class TarifaProductoConfiguration : IEntityTypeConfiguration<TarifaProducto>
{
    public void Configure(EntityTypeBuilder<TarifaProducto> builder)
    {
        builder.ToTable("mkt_Precios_td");

        builder.Property<int>("MonedaId").ValueGeneratedNever();
        builder.Property<int>("ProductoId").ValueGeneratedNever();
        builder.Property<int>("TipoDiaId").ValueGeneratedNever();
        builder.Property<int>("TipoHorarioId").ValueGeneratedNever();
        builder.Property<int>("CanalVentaId").ValueGeneratedNever();

        builder.HasKey("InicioVigencia", "MonedaId", "ProductoId", "TipoDiaId", "TipoHorarioId", "CanalVentaId");

        builder.HasOne(x => x.Moneda)
            .WithMany()
            .HasForeignKey(x => x.MonedaId);


        builder.HasOne(x => x.Producto)
            .WithMany()
            .HasForeignKey(x => x.ProductoId);


        builder.HasOne(x => x.TipoDia)
            .WithMany()
            .HasForeignKey(x => x.TipoDiaId);



        builder.HasOne(x => x.TipoHorario)
            .WithMany()
            .HasForeignKey(x => x.TipoHorarioId);

        builder.HasOne(x => x.CanalVenta)
            .WithMany()
            .HasForeignKey(x => x.CanalVentaId);


        builder.Navigation(x => x.Moneda).AutoInclude(false);
        builder.Navigation(x => x.Producto).AutoInclude(false);
        builder.Navigation(x => x.TipoDia).AutoInclude(false);
        builder.Navigation(x => x.TipoHorario).AutoInclude(false);
        builder.Navigation(x => x.CanalVenta).AutoInclude(false);


        builder.Property(x => x.ValorAfecto)
            .HasPrecision(18, 2);

        builder.Property(x => x.ValorExento)
            .HasPrecision(18, 2);

        builder.Property(x => x.Valor)
            .HasPrecision(18, 2);
    }
}
