using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class CentroCostoUsuarioConfiguration : IEntityTypeConfiguration<CentroCostoUsuario>
{
    public void Configure(EntityTypeBuilder<CentroCostoUsuario> builder)
    {
        builder.ToTable("cnf_UsuariosPorCentroCosto_td");

        builder.HasKey(x => new { x.UserId, x.CentroCostoId });
    }
}
