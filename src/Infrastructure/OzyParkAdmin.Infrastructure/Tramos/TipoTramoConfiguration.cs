using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Infrastructure.Tramos;
internal sealed class TipoTramoConfiguration : IEntityTypeConfiguration<TipoTramo>
{
    public void Configure(EntityTypeBuilder<TipoTramo> builder)
    {
        builder.ToTable("tkt_TiposTramos_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoTramoId").ValueGeneratedNever();
    }
}
