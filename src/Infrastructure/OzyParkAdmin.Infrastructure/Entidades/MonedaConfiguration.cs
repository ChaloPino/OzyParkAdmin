using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Infrastructure.Entidades;
internal sealed class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
{
    public void Configure(EntityTypeBuilder<Moneda> builder)
    {
        builder.ToTable("cnf_Monedas_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("MonedaId");
    }
}
