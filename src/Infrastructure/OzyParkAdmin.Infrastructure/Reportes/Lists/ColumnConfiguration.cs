﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzyParkAdmin.Domain.Reportes.Listed;

namespace OzyParkAdmin.Infrastructure.Reportes.Lists;
internal sealed class ColumnConfiguration : IEntityTypeConfiguration<Column>
{
    public void Configure(EntityTypeBuilder<Column> builder)
    {
        _ = builder.ToTable("rep_Columns_td");

        _ = builder.Property(p => p.ReportId)
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Id)
            .HasColumnName("ColumnId")
            .ValueGeneratedNever()
            .IsRequired(true);

        _ = builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Header)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(true);

        _ = builder.Property(p => p.Type)
            .HasConversion<int>()
            .IsRequired(true);

        _ = builder.Property(p => p.Format)
            .HasMaxLength(256)
            .IsUnicode(false)
            .IsRequired(false);

        _ = builder.Property(p => p.CanSort)
            .IsRequired(true);

        _ = builder.Property(p => p.AggregationType)
            .IsRequired(false);

        _ = builder.Property(p => p.Order)
            .IsRequired(true);

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

        _ = builder.HasOne(p => p.ExcelFormat)
            .WithMany()
            .HasForeignKey("ExcelFormatId")
            .IsRequired(false);

        _ = builder.Navigation(p => p.ExcelFormat).AutoInclude();
    }
}
