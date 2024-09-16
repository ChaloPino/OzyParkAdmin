using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Infrastructure.Entidades;
internal sealed class CiudadConfiguration : IEntityTypeConfiguration<Ciudad>
{
    public void Configure(EntityTypeBuilder<Ciudad> builder)
    {
        builder.ToTable("cnf_Ciudades_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CiudadId").ValueGeneratedNever();
        builder.HasOne(x => x.Pais).WithMany().HasForeignKey("PaisId");

    }
}
