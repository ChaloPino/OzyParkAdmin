using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Infrastructure.CentrosCosto;
internal sealed class DatosEmisorConfiguration : IEntityTypeConfiguration<DatosEmisor>
{
    public void Configure(EntityTypeBuilder<DatosEmisor> builder)
    {
        builder.ToTable("mkt_DatosEmisor_td");
        builder.HasKey(x => x.RutEmisor);
    }
}
