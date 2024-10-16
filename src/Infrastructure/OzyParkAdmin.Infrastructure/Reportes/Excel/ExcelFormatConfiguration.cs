using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Excel;

namespace OzyParkAdmin.Infrastructure.Reportes.Excel;
internal sealed class ExcelFormatConfiguration : IEntityTypeConfiguration<ExcelFormat>
{
    public void Configure(EntityTypeBuilder<ExcelFormat> builder)
    {
        _ = builder.ToTable("rep_ExcelFormats_td");

        _ = builder.Property(p => p.Id)
            .HasColumnName("ExcelFormatId")
            .ValueGeneratedNever();

        _ = builder.Property<string>("ExcelCategoryFormatId")
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Format)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.IsBuiltIn)
            .IsRequired(true);

        _ = builder.Property(p => p.NativeFormat)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.HasKey(k => k.Id);

        _ = builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey("ExcelCategoryFormatId");
    }
}
