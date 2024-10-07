using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Plantillas;

namespace OzyParkAdmin.Infrastructure.Plantillas;
internal sealed class ImpresoraConfiguration : IEntityTypeConfiguration<Impresora>
{
    public void Configure(EntityTypeBuilder<Impresora> builder)
    {
        builder.ToTable("cnf_Impresoras_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ImpresoraId").ValueGeneratedNever();
    }
}
