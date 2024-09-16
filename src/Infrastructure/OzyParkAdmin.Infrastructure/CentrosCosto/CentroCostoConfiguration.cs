using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Infrastructure.CentrosCosto;
internal sealed class CentroCostoConfiguration : IEntityTypeConfiguration<CentroCosto>
{
    public void Configure(EntityTypeBuilder<CentroCosto> builder)
    {
        builder.ToTable("vnt_CentroCosto_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CentroCostoId").ValueGeneratedNever();
        builder.Ignore(x => x.EmisoresEnUso);

        builder.OwnsMany(x => x.Horarios, navBuilder =>
        {
            navBuilder.ToTable("vnt_CentroCostoHorarios_td");
            navBuilder.HasKey(x => new { x.CentroCostoId, x.Fecha });
            navBuilder.WithOwner()
                .HasForeignKey(x => x.CentroCostoId);
        });

        builder.OwnsMany(x => x.Emisores, navBuilder =>
        {
            navBuilder.ToTable("vnt_CentroCostoEmisor_td");
            navBuilder.HasKey(x => new { x.CentroCostoId, x.RutEmisor });
            navBuilder.HasOne(x => x.DatosEmisor).WithMany().HasForeignKey(x => x.RutEmisor);
            navBuilder.Navigation(x => x.DatosEmisor).AutoInclude();
            navBuilder.WithOwner()
                .HasForeignKey(x => x.CentroCostoId);
        });
    }
}
