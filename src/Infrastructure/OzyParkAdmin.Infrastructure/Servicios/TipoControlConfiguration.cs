using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Infrastructure.Servicios;
internal sealed class TipoControlConfiguration : IEntityTypeConfiguration<TipoControl>
{
    public void Configure(EntityTypeBuilder<TipoControl> builder)
    {
        builder.ToTable("tkt_TiposControles_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoControlId").ValueGeneratedNever();
    }
}
