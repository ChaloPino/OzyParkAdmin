using NReco.PivotData;
using OzyParkAdmin.Application.Reportes.Pivoted;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
using System.ComponentModel;
using System.Xml;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Pivoted;

internal class PivotTableHtmlWriter : PivotTableWriterBase
{
    private const string ColumnHeaderClassName = "pvtColumn";
    private const string RowHeaderClassName = "pvtRow";
    private const string MeasureHeaderClassName = "pvtMeasure";
    private const string TableClassName = "pvtTable table table-bordered table-hover table-striped table-collapse table-sm dataTable";
    private const string TotalsClassName = "totals";
    private const string TotalValueClassName = "totalValue";
    private const string SubtotalsClassName = "subtotals";
    private const string SubtotalValueClassName = "subtotalValue";
    private const string ColumnSubtotalClassName = "column-subtotal";
    private const string ColumnLabelClassName = "pvtColumnLabel";
    private const string RowSubtotalClassName = "row-subtotal";
    private const string RowLabelClassName = "pvtRowLabel";
    private const string TotalsLabel = "Total";
    private const string Table = "table";
    private const string THead = "thead";
    private const string TBody = "tbody";
    private const string TR = "tr";
    private const string TH = "th";
    private const string TD = "td";
    private const string Class = "class";
    private const string Style = "style";
    private const string RowSpan = "rowspan";
    private const string ColSpan = "colspan";
    private const string DataSortIndex = "data-sort-index";
    private const string DataKeyIndex = "data-key-index";
    private const string DataValueIndex = "data-value-index";
    private const string DataSortMeasure = "data-sort-measure";
    private const string DimSortIndex = "dim-sort-index";
    private const string IndexSeparator = ":";
    private const string CssSeparator = " ";
    private const string StyleSeparator = ";";
    private const string ClassNameFormat = "{0} {1} {2}{3} {4}";
    private const string TotalsClassNameFormat = "{0} {1}";
    private const string ColSpanOne = "1";
    private const string ColSpanTwo = "2";

    private static readonly Func<object?, string?, string?> GetKey = (obj, _) => Convert.ToString(obj);
    private static readonly Func<string, string> GetValue = (str) => str;
    private static readonly Func<IAggregatorFactory, int, string?> GetMeasureHeader = (aggregatorFactory, _) => aggregatorFactory.ToString();
    private static readonly Func<IAggregator?, int, string?> GetMeasureValue = (aggregator, _) => aggregator is null || aggregator.Count <= 0 ? string.Empty : Convert.ToString(aggregator.Value);

    private readonly TextWriter _writer;

    [Flags]
    public enum RenderDimensionLabelType { None = 0, ColumnLabels = 1, RowLabels = 2, ColumnAndRowLabels = ColumnLabels | RowLabels };

    public PivotTableHtmlWriter(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        _writer = writer;
        ColumnHeaderClass = ColumnHeaderClassName;
        RowHeaderClass = RowHeaderClassName;
        MeasureHeaderClass = MeasureHeaderClassName;
        TableClass = TableClassName;
        RenderSortIndexAttr = true;
        RenderKeyIndexAttr = true;
        RenderValueIndexAttr = true;
        RenderDimensionLabel = RenderDimensionLabelType.ColumnAndRowLabels;
        RenderTheadTbody = true;
        SubtotalKeySuffix = TotalsLabel;
        SubtotalRows = false;
        SubtotalColumns = false;
        AllowHtml = false;
        RepeatKeysInGroups = RepeatDimensionKeyType.None;
        RepeatDuplicateKeysAcrossDimensions = RepeatDimensionKeyType.ColumnsAndRows;
    }

    public bool AllowHtml { get; set; }
    public string? ColumnHeaderClass { get; set; }
    public Func<string, string>? FormatDimensionLabel { get; set; }
    public Func<object?, string?, string?>? FormatKey { get; set; }
    public Func<IAggregatorFactory, int, string?>? FormatMeasureHeader { get; set; }
    public Func<IAggregator?, int, string?>? FormatValue { get; set; }
    public string? MeasureHeaderClass { get; set; }
    public RenderDimensionLabelType? RenderDimensionLabel { get; set; }
    public bool RenderKeyIndexAttr { get; set; }
    public bool RenderSortIndexAttr { get; set; }
    public bool RenderTheadTbody { get; set; }
    public bool RenderValueIndexAttr { get; set; }
    public RepeatDimensionKeyType? RepeatDuplicateKeysAcrossDimensions { get; set; }
    public RepeatDimensionKeyType? RepeatKeysInGroups { get; set; }
    public string? RowHeaderClass { get; set; }
    public bool SubtotalColumns { get; set; }
    public string[]? SubtotalDimensions { get; set; }
    public string? SubtotalKeySuffix { get; set; }
    public bool SubtotalRows { get; set; }
    public string? TableClass { get; set; }

    public Action<ValueCellContext>? OnApplyValueCellStyle { get; set; }
    public Action<KeyCellContext>? OnApplyKeyCellStyle { get; set; }
    public Action<XmlWriter>? OnPreWrite { get; set; }
    public Action<XmlWriter>? OnPostWrite { get; set; }
    public Action<XmlWriter>? OnApplyTableStyle { get; set; }

    public virtual void Write(IPivotTable pivotTable)
    {
        XmlWriter xmlWriter = XmlWriter.Create(_writer, new XmlWriterSettings
        {
            Indent = false,
            OmitXmlDeclaration = true,
            CloseOutput = false,
            CheckCharacters = false
        });
        OnPreWriter(xmlWriter);
        WriteXml(pivotTable, xmlWriter);
        OnPostWriter(xmlWriter);
        xmlWriter.Flush();
    }

    protected virtual void OnPreWriter(XmlWriter writer)
    {
        OnPreWrite?.Invoke(writer);
    }

    protected virtual void OnPostWriter(XmlWriter writer)
    {
        OnPostWrite?.Invoke(writer);
    }

    private void WriteXml(IPivotTable pivotTable, XmlWriter xmlWriter)
    {
        object? subtotalDimensions;

        HtmlWriter htmlWriter = new()
        {
            XmlWriter = xmlWriter,
            PivotTable = pivotTable,
            AggregatorsCount = GetAggregatorsCount(pivotTable),
            AggregatorFactories = pivotTable.PivotData.AggregatorFactory is CompositeAggregatorFactory factory ? factory.Factories : [pivotTable.PivotData.AggregatorFactory],
            FormatKey = FormatKey ?? GetKey,
            FormatMeasureHeader = FormatMeasureHeader ?? GetMeasureHeader,
            FormatDimensionLabel = FormatDimensionLabel ?? GetValue,
            WriteValueCell = WriteSimpleValueCell
        };

        if (pivotTable.PivotData.AggregatorFactory is CompositeAggregatorFactory)
        {
            htmlWriter.WriteValueCell = WriteComplexValueCell;
        }

        xmlWriter.WriteStartElement(Table);

        if (!string.IsNullOrEmpty(TableClass))
        {
            xmlWriter.WriteAttributeString(Class, TableClass);
        }

        OnApplyTableStyle?.Invoke(xmlWriter);
        string[] columns = pivotTable.Columns;
        ValueKey[] columnKeys = pivotTable.ColumnKeys;
        bool repeatKeysInGroups = (RepeatKeysInGroups & RepeatDimensionKeyType.Columns) == RepeatDimensionKeyType.Columns;

        subtotalDimensions = SubtotalColumns ? SubtotalDimensions ?? pivotTable.Columns : (object?)null;
        htmlWriter.Columns = PivotMetadata.Create(columns, columnKeys, repeatKeysInGroups, (string[])subtotalDimensions!);
        string[] rows = pivotTable.Rows;
        ValueKey[] rowKeys = pivotTable.RowKeys;
        repeatKeysInGroups = (RepeatKeysInGroups & RepeatDimensionKeyType.Rows) == RepeatDimensionKeyType.Rows;

        subtotalDimensions = SubtotalRows ? SubtotalDimensions ?? pivotTable.Rows : (object?)null;
        htmlWriter.Rows = PivotMetadata.Create(rows, rowKeys, repeatKeysInGroups, (string[])subtotalDimensions!);

        WriteHeader(htmlWriter);

        if (RenderTheadTbody)
        {
            htmlWriter.XmlWriter.WriteStartElement(TBody);
        }

        if (TotalsRowPosition == PivotTableTotalsPosition.First)
        {
            WriteTotals(htmlWriter);
        }

        if (htmlWriter.Rows.Length != 0)
        {
            bool flag1 = true;
            WriteRows(htmlWriter, htmlWriter.Rows[0], ref flag1);
        }

        if (TotalsRowPosition == PivotTableTotalsPosition.Last)
        {
            WriteTotals(htmlWriter);
        }

        if (RenderTheadTbody)
        {
            htmlWriter.XmlWriter.WriteEndElement();
        }

        htmlWriter.XmlWriter.WriteEndElement();
    }

    private void WriteRows(HtmlWriter writer, List<PivotMetadata>? list, ref bool writeRow)
    {
        if (list is not null)
        {
            foreach (PivotMetadata item in list)
            {
                if (item.Length == 0)
                {
                    continue;
                }

                bool a = item.Children != null;

                if (writeRow)
                {
                    writer.XmlWriter.WriteStartElement(TR);
                    writeRow = false;
                }

                if (!item.Viewed)
                {
                    int length = (RepeatDuplicateKeysAcrossDimensions & RepeatDimensionKeyType.Rows) > RepeatDimensionKeyType.None ? 0 : item.GetLength();

                    if (item.IsSubtotal)
                    {
                        length = writer.PivotTable.Rows.Length - item.Index - 1;
                    }

                    bool flag = item.Index + length == writer.PivotTable.Rows.Length - 1;
                    writer.XmlWriter.WriteStartElement(TH);

                    if (!a || (flag && RenderSortIndexAttr && item.IsValue && item.DimKeysCount >= 0))
                    {
                        writer.XmlWriter.WriteAttributeString(DataSortIndex, item.DimKeysCount.ToString());
                    }

                    if (RenderKeyIndexAttr && item.IsDimension)
                    {
                        int num = item.Index + length;
                        writer.XmlWriter.WriteAttributeString(DataKeyIndex, string.Concat(item.DimKeysCount.ToString(), IndexSeparator, num.ToString()));
                    }

                    if (a)
                    {
                        int num1 = item.Length;
                        writer.XmlWriter.WriteAttributeString(RowSpan, num1.ToString());
                    }

                    int num2 = length + (flag ? 2 : 1);

                    if (num2 > 1)
                    {
                        writer.XmlWriter.WriteAttributeString(ColSpan, num2.ToString());
                    }

                    KeyCellContext keyCellContext = new(item)
                    {
                        Dimension = writer.PivotTable.Rows[item.Index],
                        CssClass = RowHeaderClass,
                        HasChildren = a
                    };

                    WriteKeyCell(writer, keyCellContext);
                    writer.XmlWriter.WriteEndElement();
                }

                if (!a)
                {
                    if (TotalsColumnPosition == PivotTableTotalsPosition.First)
                    {
                        WriteSubtotal(writer, item);
                    }

                    if (writer.Columns.Length != 0)
                    {
                        WriteTotalsValue(writer, item, writer.Columns[0], null);
                    }

                    if (TotalsColumnPosition == PivotTableTotalsPosition.Last)
                    {
                        WriteSubtotal(writer, item);
                    }

                    writer.XmlWriter.WriteEndElement();
                    writeRow = true;
                }
                else
                {
                    WriteRows(writer, item.Children, ref writeRow);
                }
            }
        }
    }

    private void WriteSubtotal(HtmlWriter writer, PivotMetadata item)
    {
        if (TotalsColumn || GrandTotal)
        {
            if (TotalsColumn)
            {
                ValueCellContext valueCellContext = new()
                {
                    Aggregator = writer.PivotTable.GetValue(item.ValueKey, null),
                    RowKey = item.ValueKey,
                    RowDimensionCount = item.ValueKeyIndex,
                    RowIndex = item.Index,
                    IsSubTotal = true,
                    CssClass = string.Format(TotalsClassNameFormat, TotalValueClassName, item.IsSubtotal ? SubtotalValueClassName : string.Empty)
                };

                writer.WriteValueCell(writer, TD, valueCellContext);
                return;
            }

            writer.XmlWriter.WriteStartElement(TD);
            writer.XmlWriter.WriteAttributeString(ColSpan, writer.AggregatorsCount.ToString());
            writer.XmlWriter.WriteEndElement();
        }
    }

    private void WriteTotals(HtmlWriter writer)
    {
        if (TotalsRow || GrandTotal)
        {
            writer.XmlWriter.WriteStartElement(TR);
            writer.XmlWriter.WriteStartElement(TH);
            writer.XmlWriter.WriteAttributeString(Class, string.Format(TotalsClassNameFormat, TotalsClassName, RowHeaderClass));
            int length = Math.Max(writer.PivotTable.Rows.Length, 1) + 1;
            writer.XmlWriter.WriteAttributeString(ColSpan, length.ToString());

            if (TotalsRow && RenderSortIndexAttr)
            {
                writer.XmlWriter.WriteAttributeString(DataSortIndex, string.Empty);
            }

            WriteValue(writer.XmlWriter, TotalsRowHeaderText, false);
            writer.XmlWriter.WriteEndElement();

            if (TotalsColumnPosition == PivotTableTotalsPosition.First)
            {
                WriteGrandTotal(writer);
            }

            if (TotalsRow)
            {
                if (writer.Columns.Length != 0)
                {
                    WriteTotalsValue(writer, PivotMetadata.CreateSubtotals(), writer.Columns[0], TotalValueClassName);
                }
            }
            else if (writer.PivotTable.ColumnKeys.Length != 0)
            {
                writer.XmlWriter.WriteStartElement(TD);
                length += writer.PivotTable.ColumnKeys.Length * writer.AggregatorsCount;
                writer.XmlWriter.WriteAttributeString(ColSpan, length.ToString());
                writer.XmlWriter.WriteEndElement();
            }

            if (TotalsColumnPosition == PivotTableTotalsPosition.Last)
            {
                WriteGrandTotal(writer);
            }

            writer.XmlWriter.WriteEndElement();
        }
    }

    private static void WriteTotalsValue(HtmlWriter writer, PivotMetadata pivotMetadata, List<PivotMetadata> list, string? cssClass)
    {
        foreach (PivotMetadata item in list)
        {
            if (item.Length == 0)
            {
                continue;
            }

            if (item.Children == null)
            {
                ValueCellContext valueCellContext = new()
                {
                    Aggregator = writer.PivotTable.GetValue(pivotMetadata.ValueKey, item.ValueKey),
                    CssClass = cssClass
                };

                if (pivotMetadata.IsSubtotal || item.IsSubtotal)
                {
                    valueCellContext.AddCssClass(SubtotalValueClassName);
                    valueCellContext.IsSubTotal = true;
                    valueCellContext.ColumnIndex = item.Index;
                    valueCellContext.RowIndex = pivotMetadata.Index;

                    if (item.IsSubtotal)
                    {
                        valueCellContext.AddCssClass(ColumnSubtotalClassName);
                    }

                    if (pivotMetadata.IsSubtotal)
                    {
                        valueCellContext.AddCssClass(RowSubtotalClassName);
                    }
                }

                valueCellContext.RowKey = pivotMetadata.ValueKey;
                valueCellContext.ColumnKey = item.ValueKey;
                valueCellContext.RowDimensionCount = pivotMetadata.DimKeysCount;
                valueCellContext.ColumnDimensionCount = item.DimKeysCount;
                writer.WriteValueCell(writer, TD, valueCellContext);
            }
            else
            {
                WriteTotalsValue(writer, pivotMetadata, item.Children, cssClass);
            }
        }
    }

    private void WriteGrandTotal(HtmlWriter writer)
    {
        if (GrandTotal || TotalsColumn)
        {
            if (GrandTotal)
            {
                ValueCellContext valueCellContext = new()
                {
                    Aggregator = writer.PivotTable.GetValue(null, null),
                    IsSubTotal = true,
                    CssClass = TotalValueClassName
                };

                writer.WriteValueCell(writer, TD, valueCellContext);
                return;
            }

            writer.XmlWriter.WriteStartElement(TD);
            writer.XmlWriter.WriteAttributeString(ColSpan, writer.AggregatorsCount.ToString());
            writer.XmlWriter.WriteEndElement();
        }
    }

    private void WriteHeader(HtmlWriter writer)
    {
        int num1;

        if (RenderTheadTbody)
        {
            writer.XmlWriter.WriteStartElement(THead);
        }
        int num2 = writer.AggregatorsCount > 1 ? 1 : 0;

        void action()
        {
            writer.XmlWriter.WriteStartElement(TH);
            if (writer.PivotTable.Columns.Length != 0)
            {
                writer.XmlWriter.WriteAttributeString(RowSpan, writer.PivotTable.Columns.Length.ToString());
            }
            if (writer.AggregatorsCount > 1)
            {
                writer.XmlWriter.WriteAttributeString(ColSpan, writer.AggregatorsCount.ToString());
            }
            if (TotalsColumn || GrandTotal)
            {
                writer.XmlWriter.WriteAttributeString(Class, string.Format(TotalsClassNameFormat, TotalsClassName, ColumnHeaderClass));
                if (TotalsColumn && writer.AggregatorsCount <= 1 && RenderSortIndexAttr)
                {
                    writer.XmlWriter.WriteAttributeString(DataSortIndex, string.Empty);
                }
                WriteValue(writer.XmlWriter, TotalsColumnHeaderText, false);
            }
            writer.XmlWriter.WriteEndElement();
        }

        int num3 = Math.Max(writer.PivotTable.Columns.Length, 1) + num2;
        int num4 = Math.Max(writer.PivotTable.Rows.Length, 1) + 1;
        bool flag = writer.PivotTable.Rows.Length != 0 && (RenderDimensionLabel & RenderDimensionLabelType.RowLabels) == RenderDimensionLabelType.RowLabels;
        bool flag1 = writer.PivotTable.Columns.Length != 0 && (RenderDimensionLabel & RenderDimensionLabelType.ColumnLabels) == RenderDimensionLabelType.ColumnLabels;

        void int0(int int_0, string? string_0)
        {
            if (int_0 == 0)
            {
                int a = num3 - (flag ? 1 : 0);
                int num = num4 - (flag1 ? 1 : 0);

                if (a > 0 && num > 0)
                {
                    writer.XmlWriter.WriteStartElement(TH);
                    writer.XmlWriter.WriteAttributeString(RowSpan, a.ToString());
                    writer.XmlWriter.WriteAttributeString(ColSpan, num.ToString());
                    writer.XmlWriter.WriteEndElement();
                }
            }

            if (int_0 == num3 - 1 && flag)
            {
                for (int i = 0; i < writer.PivotTable.Rows.Length; i++)
                {
                    writer.XmlWriter.WriteStartElement(TH);
                    writer.XmlWriter.WriteAttributeString(Class, RowLabelClassName);

                    if (RenderSortIndexAttr)
                    {
                        writer.XmlWriter.WriteAttributeString(DimSortIndex, i.ToString());
                    }

                    if (i == writer.PivotTable.Rows.Length - 1 && !flag1)
                    {
                        writer.XmlWriter.WriteAttributeString(ColSpan, ColSpanTwo);
                    }

                    WriteValue(writer.XmlWriter, writer.FormatDimensionLabel(writer.PivotTable.Rows[i]), false);
                    writer.XmlWriter.WriteEndElement();
                }
            }

            if (flag1)
            {
                writer.XmlWriter.WriteStartElement(TH);

                if (string_0 != null)
                {
                    writer.XmlWriter.WriteAttributeString(Class, ColumnLabelClassName);
                    writer.XmlWriter.WriteAttributeString(ColSpan, ColSpanOne);

                    if (RenderSortIndexAttr)
                    {
                        writer.XmlWriter.WriteAttributeString(DimSortIndex, int_0.ToString());
                    }

                    WriteValue(writer.XmlWriter, string_0, false);
                }

                writer.XmlWriter.WriteEndElement();
            }
        }

        for (int i1 = 0; i1 < writer.PivotTable.Columns.Length; i1++)
        {
            writer.XmlWriter.WriteStartElement(TR);
            int0(i1, writer.FormatDimensionLabel(writer.PivotTable.Columns[i1]));

            if (i1 == 0 && TotalsColumnPosition == PivotTableTotalsPosition.First && (TotalsColumn || GrandTotal))
            {
                action();
            }

            for (int j = 0; j < writer.Columns[i1].Count; j++)
            {
                PivotMetadata item = writer.Columns[i1][j];

                if (item.Length != 0 && !item.Viewed)
                {
                    int length1 = (RepeatDuplicateKeysAcrossDimensions & RepeatDimensionKeyType.Columns) > RepeatDimensionKeyType.None ? 0 : item.GetLength();

                    if (item.IsSubtotal)
                    {
                        length1 = writer.PivotTable.Columns.Length - item.Index - 1;
                    }

                    writer.XmlWriter.WriteStartElement(TH);

                    if (writer.AggregatorsCount <= 1 && i1 + length1 == writer.PivotTable.Columns.Length - 1 && RenderSortIndexAttr && item.IsValue && item.DimKeysCount >= 0)
                    {
                        writer.XmlWriter.WriteAttributeString(DataSortIndex, item.DimKeysCount.ToString());
                    }

                    if (RenderKeyIndexAttr && item.IsDimension)
                    {
                        num1 = i1 + length1;
                        writer.XmlWriter.WriteAttributeString(DataKeyIndex, string.Concat(item.DimKeysCount.ToString(), IndexSeparator, num1.ToString()));
                    }

                    int num5 = item.Length * writer.AggregatorsCount;
                    writer.XmlWriter.WriteAttributeString(ColSpan, num5.ToString());

                    if (length1 > 0)
                    {
                        num1 = length1 + 1;
                        writer.XmlWriter.WriteAttributeString(RowSpan, num1.ToString());
                    }

                    KeyCellContext keyCellContext = new(item)
                    {
                        Dimension = writer.PivotTable.Columns[i1],
                        HasChildren = item.Length > 1,
                        CssClass = ColumnHeaderClass
                    };

                    WriteKeyCell(writer, keyCellContext);
                    writer.XmlWriter.WriteEndElement();
                }
            }

            if (i1 == 0 && TotalsColumnPosition == PivotTableTotalsPosition.Last && (TotalsColumn || GrandTotal))
            {
                action();
            }

            writer.XmlWriter.WriteEndElement();
        }

        if (writer.PivotTable.Columns.Length == 0 && (TotalsColumn || GrandTotal || num3 > 0))
        {
            writer.XmlWriter.WriteStartElement(TR);
            int0(0, null);

            if (TotalsColumn || GrandTotal)
            {
                action();
            }

            writer.XmlWriter.WriteEndElement();
        }

        if (writer.AggregatorsCount > 1)
        {
            writer.XmlWriter.WriteStartElement(TR);
            int0(num3 - 1, null);

            if (TotalsColumnPosition == PivotTableTotalsPosition.First && (TotalsColumn || GrandTotal))
            {
                WriteHeaderTotals(writer, PivotMetadata.CreateTotals(), TotalsClassName, true);
            }

            if (writer.Columns.Length != 0)
            {
                WriteColumnHeader(writer, writer.Columns[0]);
            }

            if (TotalsColumnPosition == PivotTableTotalsPosition.Last && (TotalsColumn || GrandTotal))
            {
                WriteHeaderTotals(writer, PivotMetadata.CreateTotals(), TotalsClassName, true);
            }

            writer.XmlWriter.WriteEndElement();
        }

        if (RenderTheadTbody)
        {
            writer.XmlWriter.WriteEndElement();
        }
    }

    private void WriteColumnHeader(HtmlWriter writer, IList<PivotMetadata> list)
    {
        string? str;

        foreach (PivotMetadata item in list)
        {
            if (item.Length == 0)
            {
                continue;
            }

            if (item.Children == null)
            {
                str = item.IsSubtotal ? SubtotalsClassName : null;
                WriteHeaderTotals(writer, item, str, item.IsValue);
            }
            else
            {
                WriteColumnHeader(writer, item.Children);
            }
        }
    }

    private void WriteHeaderTotals(HtmlWriter writer, PivotMetadata pivotMetadata, string? cssClass, bool isValueDimension)
    {
        for (int i = 0; i < writer.AggregatorsCount; i++)
        {
            writer.XmlWriter.WriteStartElement(TH);
            writer.XmlWriter.WriteAttributeString(Class, string.Format(ClassNameFormat, ColumnHeaderClass, MeasureHeaderClass, MeasureHeaderClass, i.ToString(), cssClass));

            if (RenderSortIndexAttr && isValueDimension)
            {
                writer.XmlWriter.WriteAttributeString(DataSortIndex, pivotMetadata.DimKeysCount < 0 || pivotMetadata.DimKeysCount >= writer.PivotTable.ColumnKeys.Length ? string.Empty : pivotMetadata.DimKeysCount.ToString());
                writer.XmlWriter.WriteAttributeString(DataSortMeasure, i.ToString());
            }

            WriteValue(writer.XmlWriter, writer.FormatMeasureHeader(writer.AggregatorFactories[i], i), false);
            writer.XmlWriter.WriteEndElement();
        }
    }

    private void WriteKeyCell(HtmlWriter writer, KeyCellContext keyCellContext)
    {
        keyCellContext.FormattedKey = writer.FormatKey(keyCellContext.DimensionKey, keyCellContext.Dimension);

        if (keyCellContext.IsSubtotal)
        {
            keyCellContext.AddCssClass(SubtotalsClassName);
            if (SubtotalKeySuffix != null)
            {
                keyCellContext.FormattedKey = string.Concat(keyCellContext.FormattedKey, string.Format(SubtotalKeySuffix, keyCellContext.PivotMetadata.GetDimensions()));
            }
        }

        OnWriteKeyCell(keyCellContext);
        if (keyCellContext.CssClass != null)
        {
            writer.XmlWriter.WriteAttributeString(Class, keyCellContext.CssClass);
        }
        if (keyCellContext.CssStyle != null)
        {
            writer.XmlWriter.WriteAttributeString(Style, keyCellContext.CssStyle);
        }
        WriteValue(writer.XmlWriter, keyCellContext.FormattedKey, keyCellContext.AllowHtml);
    }

    protected virtual void OnWriteKeyCell(KeyCellContext keyCellContext)
    {
        OnApplyKeyCellStyle?.Invoke(keyCellContext);
    }

    private void WriteSimpleValueCell(HtmlWriter writer, string name, ValueCellContext valueCellContext)
    {
        valueCellContext.AggregatorIndex = 0;
        WriteValueCell(writer, name, valueCellContext);
    }

    private void WriteComplexValueCell(HtmlWriter writer, string name, ValueCellContext valueCellContext)
    {
        if (valueCellContext.Aggregator is not CompositeAggregator aggregator)
        {
            WriteSimpleValueCell(writer, name, valueCellContext);
            return;
        }

        for (int i = 0; i < aggregator.Aggregators.Length; i++)
        {
            ValueCellContext aggregators = valueCellContext.Clone();
            aggregators.Aggregator = aggregator.Aggregators[i];
            aggregators.AggregatorIndex = i;
            aggregators.AddCssClass(string.Concat(MeasureHeaderClass, i.ToString()));
            WriteValueCell(writer, name, aggregators);
        }
    }

    private void WriteValueCell(HtmlWriter writer, string name, ValueCellContext valueCellContext)
    {
        int value;
        string empty;
        string empty1;
        writer.XmlWriter.WriteStartElement(name);
        string? value1 = null;

        if (!valueCellContext.ColumnDimensionCount.HasValue || valueCellContext.ColumnDimensionCount.Value < 0)
        {
            empty = string.Empty;
        }
        else
        {
            value = valueCellContext.ColumnDimensionCount.Value;
            empty = value.ToString();
        }

        string column = empty;

        if (!valueCellContext.RowDimensionCount.HasValue || valueCellContext.RowDimensionCount.Value < 0)
        {
            empty1 = string.Empty;
        }
        else
        {
            value = valueCellContext.RowDimensionCount.Value;
            empty1 = value.ToString();
        }

        string row = empty1;

        if (column != string.Empty || row != string.Empty)
        {
            value1 = string.Concat(row, IndexSeparator, column);

            if (valueCellContext.IsSubTotal)
            {
                string[] arguments = [value1, IndexSeparator, GetNumberIndex(valueCellContext.RowIndex, string.Empty), IndexSeparator, GetNumberIndex(valueCellContext.ColumnIndex, string.Empty)];
                value1 = string.Concat(arguments);
            }
        }

        Func<IAggregator?, int, string?> formatValue = FormatValue ?? GetMeasureValue;
        valueCellContext.FormattedValue = formatValue(valueCellContext.Aggregator, valueCellContext.AggregatorIndex);

        if (RenderValueIndexAttr && value1 != null)
        {
            writer.XmlWriter.WriteAttributeString(DataValueIndex, value1);
        }
        OnWriteValueCell(valueCellContext);
        if (valueCellContext.CssClass != null)
        {
            writer.XmlWriter.WriteAttributeString(Class, valueCellContext.CssClass);
        }
        if (valueCellContext.CssStyle != null)
        {
            writer.XmlWriter.WriteAttributeString(Style, valueCellContext.CssStyle);
        }
        WriteValue(writer.XmlWriter, valueCellContext.FormattedValue, valueCellContext.AllowHtml);
        writer.XmlWriter.WriteEndElement();
    }

    protected virtual void OnWriteValueCell(ValueCellContext valueContext)
    {
        OnApplyValueCellStyle?.Invoke(valueContext);
    }

    private void WriteValue(XmlWriter writer, string? value, bool allowHtml = false)
    {
        if (AllowHtml || allowHtml)
        {
            if (value is not null)
            {
                writer.WriteRaw(value);
            }

            return;
        }
        writer.WriteString(value);
    }

    private static string GetNumberIndex(int num, string defaultValue)
    {
        return num < 0 ? defaultValue : num.ToString();
    }

    internal sealed class HtmlWriter
    {
        public XmlWriter XmlWriter = default!;
        public IPivotTable PivotTable = default!;
        public int AggregatorsCount = default!;
        public IAggregatorFactory[] AggregatorFactories = default!;
        public Func<object?, string?, string?> FormatKey = default!;
        public Func<IAggregatorFactory, int, string?> FormatMeasureHeader = default!;
        public Func<string, string> FormatDimensionLabel = default!;
        public Action<HtmlWriter, string, ValueCellContext> WriteValueCell = default!;
        public List<PivotMetadata>[] Columns = default!;
        public List<PivotMetadata>[] Rows = default!;
    }

    public sealed class ValueCellContext
    {
        public ValueKey? RowKey;
        public ValueKey? ColumnKey;
        public IAggregator? Aggregator;
        public int AggregatorIndex;
        public string? CssClass;
        public string? CssStyle;
        public string? FormattedValue;
        public bool AllowHtml;
        internal int? RowDimensionCount;
        internal int? ColumnDimensionCount;
        internal bool IsSubTotal;
        internal int RowIndex = -1;
        internal int ColumnIndex = -1;

        internal ValueCellContext()
        {
        }

        internal ValueCellContext Clone()
        {
            ValueCellContext valueCellContext = new()
            {
                RowKey = RowKey,
                ColumnKey = ColumnKey,
                Aggregator = Aggregator,
                AggregatorIndex = AggregatorIndex,
                CssClass = CssClass,
                FormattedValue = FormattedValue,
                IsSubTotal = IsSubTotal,
                RowDimensionCount = RowDimensionCount,
                ColumnDimensionCount = ColumnDimensionCount,
                RowIndex = RowIndex,
                ColumnIndex = ColumnIndex
            };
            return valueCellContext;
        }

        public void AddCssClass(string cssClass)
        {
            if (CssClass is null)
            {
                CssClass = cssClass;
                return;
            }

            CssClass = string.Concat(CssClass, CssSeparator, cssClass);
        }

        public void AddCssStyle(string cssStyle)
        {
            if (string.IsNullOrWhiteSpace(CssStyle))
            {
                CssStyle = cssStyle;
                return;
            }
            CssStyle = string.Concat(CssStyle, StyleSeparator, cssStyle);
        }
    }

    public sealed class KeyCellContext
    {
        public string? Dimension;
        public string? FormattedKey;
        public string? CssClass;
        public string? CssStyle;
        public bool IsSubtotal;
        public bool AllowHtml;
        public bool HasChildren;
        internal PivotMetadata PivotMetadata;

        internal KeyCellContext(PivotMetadata pivotMetadata)
        {
            PivotMetadata = pivotMetadata;
            IsSubtotal = pivotMetadata.IsSubtotal;
        }

        public int DimensionAxisIndex => PivotMetadata.Index;
        public object? DimensionKey => PivotMetadata.ValueKey?.DimKeys[PivotMetadata.Index];

        public void AddCssClass(string cssClass)
        {
            if (CssClass == null)
            {
                CssClass = cssClass;
                return;
            }
            CssClass = string.Concat(CssClass, CssSeparator, cssClass);
        }

        public void AddCssStyle(string cssStyle)
        {
            if (string.IsNullOrWhiteSpace(CssStyle))
            {
                CssStyle = cssStyle;
                return;
            }
            CssStyle = string.Concat(CssStyle, StyleSeparator, cssStyle);
        }
    }
}