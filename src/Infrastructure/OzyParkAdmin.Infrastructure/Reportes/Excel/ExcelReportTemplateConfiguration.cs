using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Reportes.Excel;

namespace OzyParkAdmin.Infrastructure.Reportes.Excel;
internal sealed class ExcelReportTemplateConfiguration : IEntityTypeConfiguration<ExcelReportTemplate>
{
    public void Configure(EntityTypeBuilder<ExcelReportTemplate> builder)
    {
        _ = builder.ToTable("rep_ExcelReportTemplates_td");

        _ = builder.Property(p => p.HasHeader)
            .IsRequired(true);

        _ = builder.Property(p => p.HeaderTitle)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);
    }
}
