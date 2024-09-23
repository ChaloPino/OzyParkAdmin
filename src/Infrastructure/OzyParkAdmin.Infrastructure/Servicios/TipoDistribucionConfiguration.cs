using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Infrastructure.Servicios;
internal sealed class TipoDistribucionConfiguration : IEntityTypeConfiguration<TipoDistribucion>
{
    public void Configure(EntityTypeBuilder<TipoDistribucion> builder)
    {
        builder.ToTable("tkt_TiposDistribucion_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoDistribucionId").ValueGeneratedNever();
    }
}
