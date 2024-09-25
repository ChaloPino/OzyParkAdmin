using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Infrastructure.Productos;
internal sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("mkt_Productos_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ProductoId").ValueGeneratedNever();
        builder.Property(x => x.Sku).HasColumnName("ProductoSku");
        builder.Property(x => x.FechaSistema).HasColumnType("datetime");
        builder.Property(x => x.UltimaModificacion).HasColumnType("datetime");

        builder.HasOne(x => x.CentroCosto).WithMany().HasForeignKey("CentroCostoId");
        builder.Navigation(x => x.CentroCosto).AutoInclude();

        builder.HasOne(x => x.Categoria).WithMany().HasForeignKey("CategoriaId");
        builder.Navigation(x => x.Categoria).AutoInclude();

        builder.HasOne(x => x.CategoriaDespliegue).WithMany().HasForeignKey("CategoriaDespliegueId");
        builder.Navigation(x => x.CategoriaDespliegue).AutoInclude();

        builder.HasOne(x => x.Imagen).WithMany().HasForeignKey("CatalogoImagenAka");
        builder.Navigation(x => x.Imagen).AutoInclude();

        builder.HasOne(x => x.UsuarioCreacion).WithMany().HasForeignKey("UsuarioCreacionId");
        builder.Navigation(x => x.UsuarioCreacion).AutoInclude();

        builder.HasOne(x => x.UsuarioModificacion).WithMany().HasForeignKey("UsuarioModificacionId");
        builder.Navigation(x => x.UsuarioModificacion).AutoInclude();

        builder.HasOne(x => x.TipoProducto).WithMany().HasForeignKey("TipoProductoId");
        builder.Navigation(x => x.TipoProducto).AutoInclude();

        builder.OwnsOne<ProductoAgrupacion>("_productoAgrupacion", navBuilder =>
        {
            navBuilder.ToTable("ctb_ProductoAgrupacion_td");
            navBuilder.HasKey("ProductoId");
            navBuilder.Property<int>("ProductoId").ValueGeneratedNever();
            navBuilder.HasOne(x => x.AgrupacionContable).WithMany().HasForeignKey("AgrupacionContableId");
            navBuilder.Navigation(x => x.AgrupacionContable).AutoInclude();
            navBuilder.WithOwner().HasForeignKey("ProductoId");
        });

        builder.HasMany(x => x.Cajas).WithMany().UsingEntity(
                "mkt_ProductosPorCaja_td",
                l => l.HasOne(typeof(Caja)).WithMany().HasForeignKey("CajaId"),
                r => r.HasOne(typeof(Producto)).WithMany().HasForeignKey("ProductoId"),
                j => j.HasKey("CajaId", "ProductoId"));
        builder.Navigation(x => x.Cajas).AutoInclude();

        builder.OwnsMany(x => x.Complementos, navBuilder =>
        {
            navBuilder.ToTable("mkt_Complementarios_td");
            navBuilder.Property<int>("ProductoId").ValueGeneratedNever();
            navBuilder.Property<int>("ComplementoId").ValueGeneratedNever();
            navBuilder.HasKey("ProductoId", "ComplementoId");
            navBuilder.WithOwner().HasForeignKey("ProductoId");
            navBuilder.HasOne(x => x.Complemento).WithMany().HasForeignKey("ComplementoId");
            navBuilder.Navigation(x => x.Complemento).AutoInclude();
        });

        builder.OwnsMany(x => x.Relacionados, navBuilder =>
        {
            navBuilder.ToTable("mkt_Relacionados_td");
            navBuilder.Property<int>("ProductoId").ValueGeneratedNever();
            navBuilder.Property<int>("RelacionadoId").ValueGeneratedNever();
            navBuilder.HasKey("ProductoId", "RelacionadoId");
            navBuilder.WithOwner().HasForeignKey("ProductoId");
            navBuilder.HasOne(x => x.Relacionado).WithMany().HasForeignKey("RelacionadoId");
            navBuilder.Navigation(x => x.Relacionado).AutoInclude();
        });

        builder.OwnsMany(x => x.Partes, navBuilder =>
        {
            navBuilder.ToTable("mkt_ProductosCompuestos_td");
            navBuilder.Property<int>("ProductoId").ValueGeneratedNever();
            navBuilder.Property<int>("ParteProductoId").ValueGeneratedNever();
            navBuilder.HasKey("ProductoId", "ParteProductoId");
            navBuilder.WithOwner().HasForeignKey("ProductoId");
            navBuilder.HasOne(x => x.Parte).WithMany().HasForeignKey("ParteProductoId");
            navBuilder.Navigation(x => x.Parte).AutoInclude();
            navBuilder.Property(x => x.Cantidad).HasPrecision(18, 2);
        });
    }
}
