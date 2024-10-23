using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.OmisionesCupo;

namespace OzyParkAdmin.Infrastructure.OmisionesCupo;
internal sealed class IgnoraEscenarioCupoExclusionConfiguration : IEntityTypeConfiguration<IgnoraEscenarioCupoExclusion>
{
    public void Configure(EntityTypeBuilder<IgnoraEscenarioCupoExclusion> builder)
    {
        builder.ToTable("cnf_IgnoraEscenarioCupoExclusion_td");

        builder.Property<int>("EscenarioCupoId")
            .ValueGeneratedNever();

        builder.Property<int>("CanalVentaId")
            .ValueGeneratedNever();

        builder.HasKey("EscenarioCupoId", "CanalVentaId", nameof(IgnoraEscenarioCupoExclusion.FechaIgnorada));

        builder.Property(x => x.FechaIgnorada);

        builder.HasOne(x => x.EscenarioCupo)
            .WithMany()
            .HasForeignKey("EscenarioCupoId");

        builder.Navigation(x => x.EscenarioCupo).AutoInclude();

        builder.HasOne(x => x.CanalVenta)
            .WithMany()
            .HasForeignKey("CanalVentaId");

        builder.Navigation(x => x.CanalVenta).AutoInclude();
    }
}
