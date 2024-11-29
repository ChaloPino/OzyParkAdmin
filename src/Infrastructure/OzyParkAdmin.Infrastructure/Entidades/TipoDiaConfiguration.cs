using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Infrastructure.Entidades;
internal sealed class TipoDiaConfiguration : IEntityTypeConfiguration<TipoDia>
{
    public void Configure(EntityTypeBuilder<TipoDia> builder)
    {
        builder.ToTable("cnf_TipoDias_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoDiaId");
    }
}
