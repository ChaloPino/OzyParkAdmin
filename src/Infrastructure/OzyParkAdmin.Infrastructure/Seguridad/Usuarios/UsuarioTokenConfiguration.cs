using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class UsuarioTokenConfiguration : IEntityTypeConfiguration<UsuarioToken>
{
    public void Configure(EntityTypeBuilder<UsuarioToken> builder)
    {
        builder.ToTable("idt_UserTokens_td");

        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        builder.Property(x => x.UserId);

        builder.Property(x => x.LoginProvider)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Value)
            .IsUnicode(false)
            .IsRequired(false);
    }
}
