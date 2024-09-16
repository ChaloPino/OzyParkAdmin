using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.ToTable("idt_UserRoles_td");

        builder.HasKey(x => new { x.UserId, x.RoleId });
    }
}
