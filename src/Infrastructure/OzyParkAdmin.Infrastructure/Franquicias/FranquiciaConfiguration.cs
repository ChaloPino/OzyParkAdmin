using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Franquicias;

namespace OzyParkAdmin.Infrastructure.Franquicias;
internal sealed class FranquiciaConfiguration : IEntityTypeConfiguration<Franquicia>
{
    public void Configure(EntityTypeBuilder<Franquicia> builder)
    {
        builder.ToTable("cnf_Franquicias_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("FranquiciaId").ValueGeneratedNever();
        builder.HasOne(x => x.Ciudad).WithMany().HasForeignKey(x => x.CiudadId);
    }
}
