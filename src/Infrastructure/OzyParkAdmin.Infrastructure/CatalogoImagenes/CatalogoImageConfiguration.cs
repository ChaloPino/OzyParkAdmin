using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.CatalogoImagenes;

namespace OzyParkAdmin.Infrastructure.CatalogoImagenes;
internal sealed class CatalogoImageConfiguration : IEntityTypeConfiguration<CatalogoImagen>
{
    public void Configure(EntityTypeBuilder<CatalogoImagen> builder)
    {
        builder.ToTable("mkt_CatalogoImagen_td");
        builder.HasKey(x => x.Aka);
    }
}
