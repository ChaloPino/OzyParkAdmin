using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Infrastructure.Seguridad.Permisos;
internal sealed class PermisoRolCajaConfiguration : IEntityTypeConfiguration<PermisoRolCaja>
{
    public void Configure(EntityTypeBuilder<PermisoRolCaja> builder)
    {
        builder.ToTable("idt_PermisosRolesCaja_td");

        builder.HasKey(x => x.RoleId);

        builder.HasOne(x => x.Rol).WithOne().HasForeignKey<Rol>(x => x.Id);
        builder.Navigation(x => x.Rol).AutoInclude();
    }
}
