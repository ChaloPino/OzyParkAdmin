using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Infrastructure.Productos;
internal sealed class AgrupacionContableConfiguration : IEntityTypeConfiguration<AgrupacionContable>
{
    public void Configure(EntityTypeBuilder<AgrupacionContable> builder)
    {
        builder.ToTable("ctb_AgrupacionesContables_td");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("AgrupacionContableId").ValueGeneratedNever();
    }
}
