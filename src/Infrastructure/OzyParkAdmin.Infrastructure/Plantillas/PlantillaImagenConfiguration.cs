using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Plantillas;

namespace OzyParkAdmin.Infrastructure.Plantillas;
internal sealed class PlantillaImagenConfiguration : IEntityTypeConfiguration<PlantillaImagen>
{
    public void Configure(EntityTypeBuilder<PlantillaImagen> builder)
    {
        builder.ToTable("cnf_Imagenes_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ImagenId").ValueGeneratedNever();
    }
}
