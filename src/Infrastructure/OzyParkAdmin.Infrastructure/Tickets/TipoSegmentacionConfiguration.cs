using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Tickets;

namespace OzyParkAdmin.Infrastructure.Tickets;
internal sealed class TipoSegmentacionConfiguration : IEntityTypeConfiguration<TipoSegmentacion>
{
    public void Configure(EntityTypeBuilder<TipoSegmentacion> builder)
    {
        builder.ToTable("cnf_TipoSegmentaciones_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoSegmentacionId").ValueGeneratedNever();
    }
}
