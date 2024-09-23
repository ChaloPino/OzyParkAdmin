using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Infrastructure.Servicios;
internal sealed class TipoVigenciaConfiguration : IEntityTypeConfiguration<TipoVigencia>
{
    public void Configure(EntityTypeBuilder<TipoVigencia> builder)
    {
        builder.ToTable("tkt_TiposVigencias_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoVigenciaId").ValueGeneratedNever();
    }
}
