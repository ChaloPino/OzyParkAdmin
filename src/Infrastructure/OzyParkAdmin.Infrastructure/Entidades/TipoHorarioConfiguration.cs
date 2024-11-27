using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Infrastructure.Entidades;
internal sealed class TipoHorarioConfiguration : IEntityTypeConfiguration<TipoHorario>
{
    public void Configure(EntityTypeBuilder<TipoHorario> builder)
    {
        builder.ToTable("cnf_TipoHorarios_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TipoHorarioId");
    }
}
