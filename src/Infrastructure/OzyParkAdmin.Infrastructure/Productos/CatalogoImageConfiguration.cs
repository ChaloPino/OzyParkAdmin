using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Infrastructure.Productos;
internal sealed class CatalogoImageConfiguration : IEntityTypeConfiguration<CatalogoImagen>
{
    public void Configure(EntityTypeBuilder<CatalogoImagen> builder)
    {
        builder.ToTable("mkt_CatalogoImagen_td");
        builder.HasKey(x => x.Aka);
    }
}
