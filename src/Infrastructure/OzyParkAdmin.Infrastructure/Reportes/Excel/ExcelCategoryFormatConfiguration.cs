using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Excel;

namespace OzyParkAdmin.Infrastructure.Reportes.Excel;
internal sealed class ExcelCategoryFormatConfiguration : IEntityTypeConfiguration<ExcelCategoryFormat>
{
    public void Configure(EntityTypeBuilder<ExcelCategoryFormat> builder)
    {
        _ = builder.ToTable("rep_ExcelCategoryFormats_td");

        _ = builder.Property(p => p.Id)
            .HasMaxLength(256)
            .HasColumnName("ExcelCategoryFormatId")
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.CanUseDecimalPositions)
            .IsRequired(true);

        _ = builder.Property(p => p.CanUseThousandsSeparator)
            .IsRequired(true);

        _ = builder.Property(p => p.CanUseSymbol)
            .IsRequired(true);

        _ = builder.Property(p => p.Format)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.HasKey(k => k.Id);
    }
}
