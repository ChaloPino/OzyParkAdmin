using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Infrastructure.Servicios;
internal sealed class GrupoEtarioConfiguration : IEntityTypeConfiguration<GrupoEtario>
{
    public void Configure(EntityTypeBuilder<GrupoEtario> builder)
    {
        builder.ToTable("tkt_GruposEtarios_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("GrupoEtarioId").ValueGeneratedNever();
    }
}
