using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class FranquiciaUsuarioConfiguration : IEntityTypeConfiguration<FranquiciaUsuario>
{
    public void Configure(EntityTypeBuilder<FranquiciaUsuario> builder)
    {
        builder.ToTable("idt_UsersFranquicias_td");

        builder.HasKey(x => new { x.UserId, x.FranquiciaId });
    }
}
