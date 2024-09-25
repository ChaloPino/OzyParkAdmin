using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Infrastructure.Productos;
internal sealed class TipoProductoConfiguration : IEntityTypeConfiguration<TipoProducto>
{
    public void Configure(EntityTypeBuilder<TipoProducto> builder)
    {
        builder.ToTable("mkt_TiposProducto_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoProductoId").ValueGeneratedNever();
    }
}
