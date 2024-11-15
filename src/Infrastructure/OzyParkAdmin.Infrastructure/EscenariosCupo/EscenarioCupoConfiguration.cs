using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Infrastructure.EscenariosCupo;

internal sealed class EscenarioCupoConfiguration : IEntityTypeConfiguration<EscenarioCupo>
{
    public void Configure(EntityTypeBuilder<EscenarioCupo> builder)
    {
        builder.ToTable("cnf_EscenariosCupos_td");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("EscenarioCupoId").ValueGeneratedNever();

        builder.HasOne(x => x.CentroCosto)
               .WithMany()
               .HasForeignKey("CentroCostoId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.CentroCosto).AutoInclude();

        builder.HasOne(x => x.Zona)
               .WithMany()
               .HasForeignKey("ZonaId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.DetallesEscenarioCupo)
               .WithOne(x => x.EscenarioCupo)
               .HasForeignKey(x => x.EscenarioCupoId)
               .OnDelete(DeleteBehavior.Cascade); // Elimina detalles en cascada si se elimina el EscenarioCupo

        builder.Navigation(x => x.DetallesEscenarioCupo).AutoInclude();
    }
}
