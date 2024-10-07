using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OzyParkAdmin.Infrastructure.Plantillas;
internal sealed class DocumentoImpresionConfiguration : IEntityTypeConfiguration<DocumentoImpresion>
{
    public void Configure(EntityTypeBuilder<DocumentoImpresion> builder)
    {
        builder.ToTable("vnt_DocumentosImpresion_to");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DocumentoImpresionId").ValueGeneratedNever();
        builder.Property(x => x.FechaImpresion).HasColumnType("datetime");
    }
}
