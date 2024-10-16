using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Pdf;

namespace OzyParkAdmin.Infrastructure.Reportes.Pdf;
internal sealed class PdfReportTemplateConfiguration : IEntityTypeConfiguration<PdfReportTemplate>
{
    public void Configure(EntityTypeBuilder<PdfReportTemplate> builder)
    {
        _ = builder.ToTable("rep_PdfReportTemplates_td");

        _ = builder.Property(p => p.HasHeader)
            .IsRequired(true);

        _ = builder.Property(p => p.HeaderTitle)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Orientation)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.Property(p => p.TitleFontSize)
            .IsRequired(false);

        _ = builder.Property(p => p.FilterHeaderFontSize)
            .IsRequired(false);

        _ = builder.Property(p => p.FilterFontSize)
            .IsRequired(false);

        _ = builder.Property(p => p.HeaderFontSize)
            .IsRequired(false);

        _ = builder.Property(p => p.RowFontSize)
            .IsRequired(false);

        _ = builder.Property(p => p.FooterFontSize)
            .IsRequired(false);

        _ = builder.Property(p => p.RepeatHeaderInEachPage)
            .IsRequired(true);

        _ = builder.Property(p => p.RepeatFooterInEachPage)
            .IsRequired(true);

        _ = builder.ComplexProperty(p => p.Margin, complexBuilder =>
        {
            complexBuilder.Property(p => p.Left).HasColumnName("LeftMargin");
            complexBuilder.Property(p => p.Top).HasColumnName("TopMargin");
            complexBuilder.Property(p => p.Right).HasColumnName("RightMargin");
            complexBuilder.Property(p => p.Bottom).HasColumnName("BottomMargin");
        });
    }
}
