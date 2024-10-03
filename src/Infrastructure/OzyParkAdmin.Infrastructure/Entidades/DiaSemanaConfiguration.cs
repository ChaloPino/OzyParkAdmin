using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Infrastructure.Entidades;
internal sealed class DiaSemanaConfiguration : IEntityTypeConfiguration<DiaSemana>
{
    public void Configure(EntityTypeBuilder<DiaSemana> builder)
    {
        builder.ToTable("cnf_DiasDeSemana_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DiaSemanaId").ValueGeneratedNever();
    }
}
