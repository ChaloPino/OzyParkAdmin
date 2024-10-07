using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Plantillas;

namespace OzyParkAdmin.Infrastructure.Plantillas;
internal sealed class PlantillaConfiguration : IEntityTypeConfiguration<Plantilla>
{
    public void Configure(EntityTypeBuilder<Plantilla> builder)
    {
        builder.ToTable("cnf_Plantillas_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("PlantillaId").ValueGeneratedNever();

        builder.HasOne(x => x.Impresora).WithMany().HasForeignKey("ImpresoraId");

        builder.HasMany(x => x.Imagenes).WithMany().UsingEntity(
            "cnf_ImagenesPorPlantilla_td",
            l => l.HasOne(typeof(PlantillaImagen)).WithMany().HasForeignKey("ImagenId"),
            r => r.HasOne(typeof(Plantilla)).WithMany().HasForeignKey("PlantillaId"),
            j => j.HasKey("PlantillaId", "ImagenId"));
    }
}
