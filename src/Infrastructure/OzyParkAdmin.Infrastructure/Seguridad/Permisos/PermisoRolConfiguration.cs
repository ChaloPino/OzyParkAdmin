using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OzyParkAdmin.Infrastructure.Seguridad.Permisos;
internal sealed class PermisoRolConfiguration : IEntityTypeConfiguration<PermisoRol>
{
    public void Configure(EntityTypeBuilder<PermisoRol> builder)
    {
        builder.ToTable("idt_PermisosRecursosRoles_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("PermisoRolId");

        builder.HasOne(x => x.Rol)
            .WithMany()
            .HasForeignKey("RoleId");

        builder.Navigation(x => x.Rol).AutoInclude();
    }
}
