using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Infrastructure.Tramos;
internal sealed class TramoConfiguration : IEntityTypeConfiguration<Tramo>
{
    public void Configure(EntityTypeBuilder<Tramo> builder)
    {
        builder.ToTable("tkt_Tramos_td");
        builder.HasKey(t => t.Id);
        builder.Property(x => x.Id).HasColumnName("TramoId").ValueGeneratedNever();
        builder.HasOne(x => x.TipoTramo).WithMany().HasForeignKey("TipoTramoId");
    }
}
