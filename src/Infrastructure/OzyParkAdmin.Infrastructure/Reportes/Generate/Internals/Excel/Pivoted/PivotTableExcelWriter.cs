using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using NReco.PivotData;
using OzyParkAdmin.Application.Reportes.Pivoted;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
using System.Globalization;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Pivoted;
internal class PivotTableExcelWriter : PivotTableWriterBase
{
    private static readonly Func<IAggregatorFactory, int, string?> GetMeasureHeader = (aggregatorFactory, _) => aggregatorFactory.ToString();
    private static readonly Func<string, string> GetDimensionLabel = (str) => str;
    private static readonly Func<object?, string, object?> GetKey = (obj, _) => obj;

    public PivotTableExcelWriter()
    {
        SubtotalKeySuffix = "Total";
        RepeatDuplicateKeysAcrossDimensions = RepeatDimensionKeyType.ColumnsAndRows;
        RepeatKeysInGroups = RepeatDimensionKeyType.None;
    }

    public int StartCellRow { get; set; }
    public int StartCellColumn { get; set; }
    public bool SubtotalColumns { get; set; }
    public bool SubtotalRows { get; set; }
    public string SubtotalKeySuffix { get; set; }
    public string[]? SubtotalDimensions { get; set; }
    public RepeatDimensionKeyType RepeatKeysInGroups { get; set; }
    public RepeatDimensionKeyType RepeatDuplicateKeysAcrossDimensions { get; set; }
    public Func<IAggregatorFactory, int, string?>? FormatMeasureHeader { get; set; }
    public Func<string, string>? FormatDimensionLabel { get; set; }
    public Func<object?, string, object?>? FormatKey { get; set; }
    public Func<IAggregator, int, object?>? FormatValue { get; set; }
    public Action<Cell, IAggregator, int, int, bool>? OnApplyCellStyle { get; set; }
    public Action<Cell, string, bool>? OnApplyRowHeaderStyle { get; set; }
    public Action<Cell, string, bool>? OnApplyColumnHeaderStyle { get; set; }
    public Action<Cell, int>? OnApplyMeasureHeaderStyles { get; set; }
    public Action<Cell>? OnApplyTotalHeaderStyles { get; set; }
    public Action<Cell>? OnApplyDimensionLabelStyles { get; set; }

    private Cell GetCell(IList<Row> rows, int rowIndex, int columnIndex, int totalColumns)
    {
        Row? row = rows.FirstOrDefault(r => r.RowIndex is not null && r.RowIndex.Value == (uint)rowIndex);

        if (row is null)
        {
            string span = $"{StartCellColumn + 1}:{totalColumns.ToString(CultureInfo.InvariantCulture)}";

            row = new Row
            {
                RowIndex = new UInt32Value((uint)rowIndex),
                Spans = new ListValue<StringValue>([new StringValue(span)]),
                DyDescent = new DoubleValue(0.3)
            };

            for (int i = 0; i < totalColumns; i++)
            {
                Cell c = new()
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i)}{rowIndex}")
                };
                _ = row.AppendChild(c);
            }

            rows.Add(row);
        }

        Cell? cell = (Cell?)row.ChildElements.FirstOrDefault(c => IsCell(c, rowIndex, columnIndex));

        if (cell is null)
        {
            cell = new Cell
            {
                CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(columnIndex)}{rowIndex}")
            };

            bool inserted = false;

            foreach (Cell existingCell in row.ChildElements.OfType<Cell>())
            {
                if (existingCell.CompareTo(cell) > 0)
                {
                    inserted = true;
                    _ = row.InsertBefore(cell, existingCell);
                    break;
                }
            }

            if (!inserted)
            {
                _ = row.AppendChild(cell);
            }
        }

        return cell;
    }

    private static List<Cell> GetCells(IList<Row> rows, int startRow, int startColumn, int endRow, int endColumn)
    {
        IEnumerable<Row> betweenRows = rows.Where(r => r.RowIndex is not null && r.RowIndex.Value >= (uint)startRow && r.RowIndex.Value <= (uint)endRow);

        return betweenRows
            .SelectMany(r => r.ChildElements.OfType<Cell>())
            .Where(c => c.TryExtractReference(out int r, out int column) && column >= startColumn && column <= endColumn)
            .ToList();
    }

    private static bool IsCell(OpenXmlElement element, int rowIndex, int columnIndex)
    {
        return element is Cell cell && cell.CellReference is not null
            && string.Equals(cell.CellReference.Value, $"{ExcelHelper.GetColumnLetter(columnIndex)}{rowIndex}");
    }

    private void AdjustRows(IList<Row> rows, int totalColumns)
    {
        string span = $"{StartCellColumn + 1}:{totalColumns.ToString(CultureInfo.InvariantCulture)}";

        foreach (Row row in rows)
        {
            if (row.Spans?.HasValue == true)
            {
                row.Spans.Items.Clear();
            }

            row.Spans = new ListValue<StringValue>([new StringValue(span)]);
        }
    }

    public int Write(IPivotTable pivotTable, IList<Row> rows, IList<Column> columns, SharedTable sharedTable, IList<MergeCell> mergeCells, SheetDimension sheetDimension)
    {
        object? subtotaldimensions = null;

        ExcelWriter writer = new()
        {
            PivotTable = pivotTable,
            ExcelRows = rows,
            ExcelColumns = columns,
            MergeCells = mergeCells,
            SharedTable = sharedTable,
            AggregatorsCount = GetAggregatorsCount(pivotTable),
            FormatMeasureHeader = FormatMeasureHeader ?? GetMeasureHeader,
            FormatDimensionLabel = FormatDimensionLabel ?? GetDimensionLabel,
            FormatKey = FormatKey ?? GetKey,
            WriteCellValue = WriteSimpleCell
        };

        if (pivotTable.PivotData.AggregatorFactory is CompositeAggregatorFactory)
        {
            writer.WriteCellValue = WriteComplexCell;
        }

        string[] columnsCollection = pivotTable.Columns;
        ValueKey[] columnKeys = pivotTable.ColumnKeys;
        bool repeatKeysInGroups = (RepeatKeysInGroups & RepeatDimensionKeyType.Columns) == RepeatDimensionKeyType.Columns;
        subtotaldimensions = SubtotalColumns ? SubtotalDimensions ?? pivotTable.Columns : (object?)null;
        writer.Columns = PivotMetadata.Create(columnsCollection, columnKeys, repeatKeysInGroups, (string[]?)subtotaldimensions);
        writer.ColumnLength = writer.Columns.Length != 0 ? writer.Columns[0].Sum(b => b.Length) : 0;

        string[] rowCollection = pivotTable.Rows;
        ValueKey[] rowKeys = pivotTable.RowKeys;
        repeatKeysInGroups = (RepeatKeysInGroups & RepeatDimensionKeyType.Rows) == RepeatDimensionKeyType.Rows;
        subtotaldimensions = SubtotalRows ? SubtotalDimensions ?? pivotTable.Rows : (object?)null;

        PivotMetadata[] bArray = writer.ColumnLength != 0 ? PivotMetadata.Project(writer.Columns[0]).ToArray() : [];
        int totalColumns = (bArray.Length * writer.AggregatorsCount) + writer.PivotTable.Rows.Length;

        if (rows.Any())
        {
            AdjustRows(rows, totalColumns);
        }

        BorderContext item = new();
        writer.Rows = PivotMetadata.Create(rowCollection, rowKeys, repeatKeysInGroups, (string[]?)subtotaldimensions);
        writer.RowLength = writer.Rows.Length != 0 ? writer.Rows[0].Sum(b => b.Length) : 0;
        int startCellRow = StartCellRow;
        int startCellColumn = StartCellColumn;
        int length = pivotTable.Rows.Length;

        if (pivotTable.Rows.Length == 0 && (TotalsRow || GrandTotal))
        {
            length++;
        }

        if (TotalsColumnPosition == PivotTableTotalsPosition.First && (TotalsColumn || GrandTotal))
        {
            length += writer.AggregatorsCount;
        }

        int num = TotalsRowPosition != PivotTableTotalsPosition.First || (!TotalsRow && !GrandTotal) ? 0 : 1;

        if (writer.Columns.Length != 0)
        {
            WriteColumns(writer, startCellRow, startCellColumn + length, writer.Columns[0], totalColumns);
            startCellRow += pivotTable.Columns.Length;
        }
        else if (TotalsColumn || GrandTotal || pivotTable.Rows.Length != 0)
        {
            startCellRow++;
        }

        if (TotalsColumn || GrandTotal)
        {
            int num1 = startCellRow - 1;
            int b = TotalsColumnPosition == PivotTableTotalsPosition.First ? startCellColumn + length : startCellColumn + length + (writer.ColumnLength * writer.AggregatorsCount);

            if (TotalsColumnPosition == PivotTableTotalsPosition.First)
            {
                b -= writer.AggregatorsCount;
            }

            Cell cell = GetCell(rows, num1, b, totalColumns);
            WriteCell(sharedTable, cell, TotalsColumnHeaderText);

            if (writer.AggregatorsCount > 1)
            {
                writer.MergeCells.Add(new MergeCell
                {
                    Reference = new StringValue(ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(b), num1, ExcelHelper.GetColumnLetter(b + writer.AggregatorsCount - 1), num1))
                });
            }

            ApplyTotalHeaderStyles(cell);
        }

        if (writer.AggregatorsCount > 1)
        {
            IAggregatorFactory[] factories = ((CompositeAggregatorFactory)pivotTable.PivotData.AggregatorFactory).Factories;
            int num2 = writer.ColumnLength + (TotalsColumn || GrandTotal ? 1 : 0);
            int b1 = startCellColumn + length;

            if (TotalsColumnPosition == PivotTableTotalsPosition.First && (TotalsColumn || GrandTotal))
            {
                b1 -= writer.AggregatorsCount;
            }

            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < writer.AggregatorsCount; j++)
                {
                    Cell item1 = GetCell(rows, startCellRow, (i * writer.AggregatorsCount) + j + b1, totalColumns);
                    WriteCell(sharedTable, item1, writer.FormatMeasureHeader(factories[j], j));
                    ApplyMeasureHeaderStyles(item1, j);
                }
            }
            startCellRow++;
        }

        if (startCellRow > StartCellRow)
        {
            for (int k = 0; k < pivotTable.Rows.Length; k++)
            {
                Cell cell = GetCell(rows, startCellRow - 1, startCellColumn + k, totalColumns);
                WriteCell(sharedTable, cell, writer.FormatDimensionLabel(pivotTable.Rows[k]));
                ApplyDimensionLabelStyle(cell);
            }
        }
        item.ColumnKeys = writer.ColumnLength > 0
            ? GetCells(rows, StartCellRow, startCellColumn + length, startCellRow + num - 1, startCellColumn + length + (writer.ColumnLength * writer.AggregatorsCount) - 1)
            : GetCells(rows, StartCellRow, startCellColumn + length - 1, Math.Max(startCellRow - 1, StartCellRow), startCellColumn + length - 1);

        if (writer.RowLength != 0)
        {
            WriteRows(writer, startCellRow + num, startCellColumn, writer.Rows[0], bArray, totalColumns);
        }

        if (TotalsRow || GrandTotal)
        {
            int length1 = startCellColumn;
            if (pivotTable.Rows.Length != 0)
            {
                length1 = length1 + pivotTable.Rows.Length - 1;
            }
            Cell cell = GetCell(rows, startCellRow + (TotalsRowPosition == PivotTableTotalsPosition.First ? 0 : writer.RowLength), length1, totalColumns);
            WriteCell(sharedTable, cell, TotalsRowHeaderText);
            ApplyTotalHeaderStyles(cell);
        }

        if (TotalsRow)
        {
            int num3 = startCellColumn + length;
            int num4 = (TotalsRowPosition == PivotTableTotalsPosition.First ? 0 : writer.RowLength) + startCellRow;

            for (int i = 0; i < bArray.Length; i++)
            {
                writer.WriteCellValue(rows, sharedTable, num4, (i * writer.AggregatorsCount) + num3, pivotTable.GetValue(null, bArray[i].ValueKey), totalColumns, true);
            }
        }

        item.RowKeys = writer.RowLength > 0
            ? GetCells(rows, startCellRow, StartCellColumn, startCellRow + writer.RowLength + num - 1, startCellColumn + length - 1)
            : GetCells(rows, startCellRow - 1, StartCellColumn, startCellRow + num - 1, Math.Max(startCellColumn + length - 1, StartCellColumn));

        if (GrandTotal)
        {
            int num5 = TotalsColumnPosition == PivotTableTotalsPosition.First ? startCellColumn + length - writer.AggregatorsCount : (writer.ColumnLength * writer.AggregatorsCount) + startCellColumn + length;
            int num6 = (TotalsRowPosition == PivotTableTotalsPosition.First ? 0 : writer.RowLength) + startCellRow;
            writer.WriteCellValue(rows, sharedTable, num6, num5, pivotTable.GetValue(null, null), totalColumns, true);
        }

        sheetDimension.Reference = new StringValue(ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), 1, ExcelHelper.GetColumnLetter(totalColumns), (TotalsRowPosition == PivotTableTotalsPosition.First ? 0 : writer.RowLength) + startCellRow));

        return totalColumns;
    }

    private void ApplyDimensionLabelStyle(Cell cell)
    {
        OnApplyDimensionLabelStyles?.Invoke(cell);
    }

    private void ApplyTotalHeaderStyles(Cell cell)
    {
        OnApplyTotalHeaderStyles?.Invoke(cell);
    }

    private void ApplyMeasureHeaderStyles(Cell cell, int index)
    {
        OnApplyMeasureHeaderStyles?.Invoke(cell, index);
    }

    private void WriteColumns(ExcelWriter writer, int rowIndex, int columnIndex, IList<PivotMetadata> pivotMetadatas, int totalColumns)
    {
        int num = 0;

        foreach (PivotMetadata item in pivotMetadatas)
        {
            int b = item.Length * writer.AggregatorsCount;

            if (item.Length > 0 && !item.Viewed)
            {
                int length = (RepeatDuplicateKeysAcrossDimensions & RepeatDimensionKeyType.Columns) > RepeatDimensionKeyType.None ? 0 : item.GetLength();

                if (item.IsSubtotal)
                {
                    length = writer.PivotTable.Columns.Length - item.Index - 1;
                }

                object? dimKeys = item.ValueKey?.DimKeys[item.Index];
                Cell cell = GetCell(writer.ExcelRows, rowIndex, columnIndex + num, totalColumns);
                object? a = writer.FormatKey(dimKeys, writer.PivotTable.Columns[item.Index]);

                if (item.IsSubtotal && SubtotalKeySuffix != null)
                {
                    a = string.Concat(a, string.Format(SubtotalKeySuffix, item.GetDimensions()));
                }

                WriteCell(writer.SharedTable, cell, a);
                ApplyColumnHeaderStyles(cell, writer.PivotTable.Columns[item.Index], item.IsSubtotal);

                if (b > 1 || length > 0)
                {
                    _ = cell.TryExtractReference(out int startRow, out int startColumn);

                    writer.MergeCells.Add(new MergeCell
                    {
                        Reference = new StringValue(ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(startColumn), startRow, ExcelHelper.GetColumnLetter(startColumn + b - 1), startRow + length))
                    });
                }
            }

            if (item.Children != null)
            {
                WriteColumns(writer, rowIndex + 1, columnIndex + num, item.Children, totalColumns);
            }
            num += b;
        }
    }

    private void ApplyColumnHeaderStyles(Cell cell, string columnName, bool isSubtotal)
    {
        OnApplyColumnHeaderStyle?.Invoke(cell, columnName, isSubtotal);
    }

    private void WriteRows(ExcelWriter writer, int rowIndex, int columnIndex, IList<PivotMetadata> list, PivotMetadata[] header, int totalColumns)
    {
        int num = 0;

        foreach (PivotMetadata item in list)
        {
            int num1 = item.Length;

            if (item.Length > 0 && !item.Viewed)
            {
                int length = (RepeatDuplicateKeysAcrossDimensions & RepeatDimensionKeyType.Rows) > RepeatDimensionKeyType.None ? 0 : item.GetLength();

                if (item.IsSubtotal)
                {
                    length = writer.PivotTable.Rows.Length - item.Index - 1;
                }

                object? dimKeys = item.ValueKey?.DimKeys[item.Index];
                Cell cell = GetCell(writer.ExcelRows, rowIndex + num, columnIndex, totalColumns);
                object? a = writer.FormatKey(dimKeys, writer.PivotTable.Rows[item.Index]);

                if (item.IsSubtotal && SubtotalKeySuffix != null)
                {
                    a = string.Concat(a, string.Format(SubtotalKeySuffix, item.GetDimensions()));
                }

                WriteCell(writer.SharedTable, cell, a);
                ApplyRowHeaderStyles(cell, writer.PivotTable.Rows[item.Index], item.IsSubtotal);

                if (num1 > 1 || length > 0)
                {
                    _ = cell.TryExtractReference(out int startRow, out int startColumn);
                    writer.MergeCells.Add(new MergeCell
                    {
                        Reference = new StringValue(ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(startColumn), startRow, ExcelHelper.GetColumnLetter(startColumn + length), startRow + num1 - 1))
                    });
                }
            }

            if (item.Children == null)
            {
                int int1 = columnIndex + writer.PivotTable.Rows.Length - item.Index;

                if (TotalsColumn)
                {
                    int length1 = (header.Length * writer.AggregatorsCount) + int1;

                    if (TotalsColumnPosition == PivotTableTotalsPosition.First)
                    {
                        length1 = int1;
                        int1 += writer.AggregatorsCount;
                    }

                    writer.WriteCellValue(writer.ExcelRows, writer.SharedTable, rowIndex + num, length1, writer.PivotTable.GetValue(item.ValueKey, null), totalColumns, true);
                }

                for (int i = 0; i < header.Length; i++)
                {
                    writer.WriteCellValue(writer.ExcelRows, writer.SharedTable, rowIndex + num, (i * writer.AggregatorsCount) + int1, writer.PivotTable.GetValue(item.ValueKey, header[i].ValueKey), totalColumns, header[i].IsSubtotal || item.IsSubtotal);
                }
            }
            else
            {
                WriteRows(writer, rowIndex + num, columnIndex + 1, item.Children, header, totalColumns);
            }

            num += num1;
        }
    }

    private void ApplyRowHeaderStyles(Cell cell, string rowName, bool isSubtotal)
    {
        OnApplyRowHeaderStyle?.Invoke(cell, rowName, isSubtotal);
    }

    private void WriteSimpleCell(IList<Row> rows, SharedTable sharedTable, int rowIndex, int colIndex, IAggregator aggregator, int totalColumns, bool isSubtotal)
    {
        Cell item = GetCell(rows, rowIndex, colIndex, totalColumns);
        object? value = FormatValue != null ? FormatValue(aggregator, 0) : aggregator.Value;
        WriteCell(sharedTable, item, value);
        ApplyValueStyles(item, aggregator, 0, rowIndex, isSubtotal);
    }

    private void WriteComplexCell(IList<Row> rows, SharedTable sharedTable, int rowIndex, int colIndex, IAggregator aggregator, int totalColumns, bool isSubtotal)
    {
        CompositeAggregator compositeAggregator = (CompositeAggregator)aggregator;

        for (int i = 0; i < compositeAggregator.Aggregators.Length; i++)
        {
            Cell item = GetCell(rows, rowIndex, colIndex + i, totalColumns);
            IAggregator aggr = compositeAggregator.Aggregators[i];

            if (aggr.Count > 0)
            {
                object? value = FormatValue != null ? FormatValue(aggr, i) : aggr.Value;
                WriteCell(sharedTable, item, value);
            }

            ApplyValueStyles(item, aggr, i, rowIndex, isSubtotal);
        }
    }

    private static void WriteCell(SharedTable sharedTable, Cell cell, object? value)
    {
        if (value is not null)
        {
            if (value is string str)
            {
                value = sharedTable.AddString(str);
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }

            string? strValue = value.ToString();

            if (strValue is not null)
            {
                cell.CellValue = new CellValue(strValue);
            }
        }
    }

    protected virtual void ApplyValueStyles(Cell cell, IAggregator aggregator, int aggrIndex, int index, bool isSubtotal)
    {
        OnApplyCellStyle?.Invoke(cell, aggregator, aggrIndex, index, isSubtotal);
    }

    private sealed class ExcelWriter
    {
        internal IPivotTable PivotTable = default!;
        internal IList<Row> ExcelRows = default!;
        internal int AggregatorsCount = default!;
        internal Func<IAggregatorFactory, int, string?> FormatMeasureHeader = default!;
        internal Func<string, string> FormatDimensionLabel = default!;
        internal Func<object?, string, object?> FormatKey = default!;
        internal Action<IList<Row>, SharedTable, int, int, IAggregator, int, bool> WriteCellValue = default!;
        internal List<PivotMetadata>[] Columns = default!;
        internal int ColumnLength = default!;
        internal List<PivotMetadata>[] Rows = default!;
        internal int RowLength = default!;
        internal IList<Column> ExcelColumns = default!;
        internal SharedTable SharedTable = default!;
        internal IList<MergeCell> MergeCells = default!;
    }

    public struct BorderContext
    {
        public IEnumerable<Cell> ColumnKeys { get; internal set; }
        public IEnumerable<Cell> RowKeys { get; internal set; }
        public IEnumerable<Cell> Table { get; internal set; }
    }
}
