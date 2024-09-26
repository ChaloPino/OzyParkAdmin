using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Infrastructure.CategoriasProducto;
internal sealed class CategoriaProductoConfiguration : IEntityTypeConfiguration<CategoriaProducto>
{
    public void Configure(EntityTypeBuilder<CategoriaProducto> builder)
    {
        builder.ToTable("mkt_Categorias_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CategoriaId").ValueGeneratedNever();
        builder.Property(x => x.FechaCreacion).HasColumnType("datetime");
        builder.Property(x => x.UltimaModificacion).HasColumnType("datetime");

        builder.HasOne(x => x.Padre)
            .WithMany(x => x.Hijos)
            .HasForeignKey("CategoriaIdPadre")
            .IsRequired(false);

        builder.HasOne(x => x.Imagen).WithMany().HasForeignKey("CatalogoImagenAka");

        builder.HasOne(x => x.UsuarioCreacion).WithMany().HasForeignKey("UsuarioCreacionId");
        builder.Navigation(x => x.UsuarioCreacion).AutoInclude();

        builder.HasOne(x => x.UsuarioModificacion).WithMany().HasForeignKey("UsuarioModificacionId");
        builder.Navigation(x => x.UsuarioModificacion).AutoInclude();

        builder.OwnsMany(x => x.CajasAsignadas, navBuilder =>
        {
            navBuilder.ToTable("mkt_CategoriasPorCaja_td");
            navBuilder.Property<int>("CajaId").ValueGeneratedNever();
            navBuilder.Property<int>("CategoriaId").ValueGeneratedNever();
            navBuilder.HasKey("CajaId", "CategoriaId");
            navBuilder.HasOne(x => x.Caja).WithMany().HasForeignKey("CajaId");
            navBuilder.Navigation(x => x.Caja).AutoInclude();
            navBuilder.WithOwner().HasForeignKey("CategoriaId");
        });

        builder.HasMany(x => x.CanalesVenta).WithMany().UsingEntity(
            "mkt_CategoriasPorCanalVenta_td",
            l => l.HasOne(typeof(CanalVenta)).WithMany().HasForeignKey("CanalVentaId"),
            r => r.HasOne(typeof(CategoriaProducto)).WithMany().HasForeignKey("CategoriaId"),
            j => j.HasKey("CategoriaId", "CanalVentaId"));
        builder.Navigation(x => x.CanalesVenta).AutoInclude();
    }
}
