using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class UsuarioLoginConfiguration : IEntityTypeConfiguration<UsuarioLogin>
{
    public void Configure(EntityTypeBuilder<UsuarioLogin> builder)
    {
        builder.ToTable("idt_UserLogins_td");

        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.ProviderKey });

        builder.Property(x => x.UserId);

        builder.Property(x => x.LoginProvider)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.ProviderKey)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();
    }
}
