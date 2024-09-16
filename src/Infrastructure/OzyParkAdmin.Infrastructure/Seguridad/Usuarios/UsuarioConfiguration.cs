using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;
internal sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("idt_Users_td");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(x => x.UserName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(256)
            .IsUnicode(true);

        builder.Property(x => x.FriendlyName)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsUnicode(false);

        builder.Property(x => x.EmailConfirmed);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(256)
            .IsUnicode(false);

        builder.Property(x => x.PhoneNumberConfirmed);

        builder.Property(x => x.AccessFailedCount);

        builder.Property(x => x.LockoutEnabled);

        builder.Property(x => x.LockoutEndDateUtc)
            .HasColumnType("datetime");

        builder.Property(x => x.SecurityStamp)
            .HasMaxLength(256)
            .IsRequired(true)
            .IsUnicode(false);

        builder.Property(x => x.TwoFactorEnabled);

        builder.Property(x => x.ChangePasswordNextLogon);

        builder.Property(x => x.ConcurrencyStamp)
            .HasMaxLength(256)
            .IsUnicode(false);

        builder.HasMany(x => x.Claims)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Logins)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Tokens)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.CentrosCosto)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Franquicias)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.Navigation(x => x.Claims).AutoInclude();
        builder.Navigation(x => x.Roles).AutoInclude();
        builder.Navigation(x => x.Logins).AutoInclude();
        builder.Navigation(x => x.Tokens).AutoInclude();
        builder.Navigation(x => x.CentrosCosto).AutoInclude();
        builder.Navigation(x => x.Franquicias).AutoInclude();
    }
}
