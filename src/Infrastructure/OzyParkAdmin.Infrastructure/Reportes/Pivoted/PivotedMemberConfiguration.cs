using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Pivoted;

namespace OzyParkAdmin.Infrastructure.Reportes.Pivoted;
internal sealed class PivotedMemberConfiguration : IEntityTypeConfiguration<PivotedMember>
{
    public void Configure(EntityTypeBuilder<PivotedMember> builder)
    {
        _ = builder.ToTable("rep_PivotedMembers_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Id)
            .HasColumnName("PivotedMemberId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.ColumnId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Header)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.PivotType)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.Property(p => p.AggregationType)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.ShowTotal)
            .IsRequired(false);

        _ = builder.Property(p => p.Order)
            .IsRequired(true);

        _ = builder.Property(p => p.Property)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.PropertyDisplay)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Format)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.SortDirection)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.SortColumnId)
            .IsRequired(false);

        _ = builder.Property(p => p.CustomSortList)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property<int?>("ExcelFormatId")
            .IsRequired(false);

        _ = builder.Property(p => p.CustomExcelFormatId)
            .IsRequired(false);

        _ = builder.Property(p => p.CustomExcelFormat)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.Roles)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .IsRequired(false)
            .HasConversion<RolesConverter>();

        _ = builder.Property(p => p.HasConditionalStyle)
            .IsRequired(true);

        _ = builder.Property(p => p.SuccessStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.SuccessConditionalValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.SuccessAlternateConditionalValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.WarningStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.Property(p => p.WarningConditionalValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.WarningAlternateConditionalValue)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.ErrorStyle)
            .HasConversion<int>()
            .IsRequired(false);

        _ = builder.HasKey(k => new { k.ReportId, k.Id });

        _ = builder.HasOne(p => p.Column)
            .WithMany()
            .HasForeignKey(p => new { p.ReportId, p.ColumnId });

        _ = builder.Navigation(p => p.Column)
            .AutoInclude();

        _ = builder.HasOne(p => p.SortColumn)
            .WithMany()
            .HasForeignKey(p => new { p.ReportId, p.SortColumnId })
            .IsRequired(false);

        _ = builder.Navigation(p => p.SortColumn)
            .AutoInclude();

        _ = builder.HasOne(p => p.ExcelFormat)
            .WithMany()
            .HasForeignKey("ExcelFormatId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.ExcelFormat)
            .AutoInclude();
    }
}
