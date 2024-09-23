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
    }
}
