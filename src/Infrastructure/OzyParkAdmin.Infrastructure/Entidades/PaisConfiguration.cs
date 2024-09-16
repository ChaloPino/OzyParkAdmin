using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Infrastructure.Entidades;
internal sealed class PaisConfiguration : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {
        builder.ToTable("cnf_Paises_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("PaisId").ValueGeneratedNever();
    }
}
