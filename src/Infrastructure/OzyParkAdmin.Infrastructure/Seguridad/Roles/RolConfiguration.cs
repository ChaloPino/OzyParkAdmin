using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Infrastructure.Seguridad.Roles;
internal sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("idt_Roles_td");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("RoleId")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.ConcurrencyStamp)
            .HasMaxLength(256)
            .IsUnicode(false);

        builder.HasMany(x => x.Users)
            .WithOne()
            .HasForeignKey(x => x.RoleId);

        builder.HasMany(x => x.ChildRoles)
            .WithMany()
            .UsingEntity(
                "idt_RolesHierarchy_td",
                l => l.HasOne(typeof(Rol)).WithMany().HasForeignKey("ChildRoleId").HasPrincipalKey(nameof(Rol.Id)),
                r => r.HasOne(typeof(Rol)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Rol.Id)),
                j => j.HasKey("RoleId", "ChildRoleId"));

        builder.Navigation(x => x.Users).AutoInclude();
    }
}
