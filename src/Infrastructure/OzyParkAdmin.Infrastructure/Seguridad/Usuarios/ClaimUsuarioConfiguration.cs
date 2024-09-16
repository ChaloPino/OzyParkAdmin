using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class ClaimUsuarioConfiguration : IEntityTypeConfiguration<ClaimUsuario>
{
    public void Configure(EntityTypeBuilder<ClaimUsuario> builder)
    {
        builder.ToTable("idt_UserClaims_td");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("UserClaimId");

        builder.Property(x => x.UserId);

        builder.Property(x => x.ClaimType)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.ClaimValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();
    }
}
